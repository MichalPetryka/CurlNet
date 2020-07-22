using CurlNet.Enums;
using CurlNet.Exceptions;
using CurlNet.Memory;
using System;
using System.Collections.Generic;
using System.Text;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace CurlNet
{
	public sealed partial class Curl : IDisposable
	{
		private static readonly object CallbackLock = new object();
		private static readonly Dictionary<int, Curl> Requests = new Dictionary<int, Curl>();
		private static readonly Random IdGenerator = new Random();

		private static readonly object InitializationLock = new object();
		private static CurlCode _initialized = CurlCode.NotInitialized;

		internal static IMemoryHolder MemoryHolder;

		private readonly NativeMemory _memory;

		private static readonly CurlNative.WriteFunctionCallback WriteFunctionCallback = WriteCallback;
		private static readonly CurlNative.ReadFunctionCallback ReadFunctionCallback = ReadCallback;
		private static readonly CurlNative.ProgressFunctionCallback ProgressFunctionCallback = ProgressCallback;

		private bool _disposed;
		private bool _contentLengthFetched;
		private byte[] _downloadBuffer;
		private int _downloadOffset;
		private byte[] _uploadBuffer;
		private int _uploadOffset;

		private string _userAgent;
		private IpResolveMode _ipMode;
		private Progress _downloadProgress;
		private Progress _uploadProgress;

		public static readonly string CurlVersion = CurlNative.GetVersion();

		public bool UseBom = false;

		public delegate void ProgressHandler(Curl sender, Progress progress);
		public event ProgressHandler DownloadProgressChange;
		public event ProgressHandler UploadProgressChange;

		public int Response { get; private set; }

		public string UserAgent
		{
			get => _userAgent;
			set
			{
				_userAgent = value;
				SetUseragent(value);
			}
		}

		public IpResolveMode IpMode
		{
			get => _ipMode;
			set
			{
				_ipMode = value;
				SetIpMode(value);
			}
		}

		public Progress DownloadProgress
		{
			get => _downloadProgress;
			private set
			{
				if (_downloadProgress != value)
				{
					_downloadProgress = value;
					DownloadProgressChange?.Invoke(this, value);
				}
			}
		}

		public Progress UploadProgress
		{
			get => _uploadProgress;
			private set
			{
				if (_uploadProgress != value)
				{
					_uploadProgress = value;
					UploadProgressChange?.Invoke(this, value);
				}
			}
		}

		public static bool Initialize(bool poolMemory, SharedData sharedData = SharedData.None)
		{
			lock (InitializationLock)
			{
				if (_initialized != CurlCode.Ok)
				{
					_initialized = CurlNative.GlobalInit(CurlGlobal.Default);
					if (poolMemory)
						MemoryHolder = new MemoryPool();
					else
						MemoryHolder = new MemoryFactory();
					CurlShare.Initialize(sharedData);
				}

				return _initialized == CurlCode.Ok;
			}
		}

		public static void Deinitialize()
		{
			lock (InitializationLock)
			{
				if (_initialized != CurlCode.NotInitialized)
				{
					CurlShare.Close();
					CurlNative.GlobalCleanup();
					MemoryHolder.Clear();
					MemoryHolder = null;
					_initialized = CurlCode.NotInitialized;
				}
			}
		}

		internal string GetError(CurlCode code)
		{
			if (code == CurlCode.NotInitialized)
			{
				return CurlNotInitializedException.NotInitializedMessage;
			}

			string error = _memory.GetError();
			return string.IsNullOrEmpty(error) ? CurlNative.GetErrorMessage(code) : error;
		}

		private void SetUseragent(string userAgent)
		{
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.UserAgent, userAgent));
		}

		private void SetIpMode(IpResolveMode mode)
		{
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.UserAgent, mode));
		}

		private string ToText(ArraySegment<byte> data, Encoding encoding)
		{
			// ReSharper disable once ConvertIfStatementToReturnStatement
			if (UseBom)
			{
				return BomUtil.GetEncoding(data, encoding, out int offset).GetString(data.Array!, data.Offset + offset, data.Count - offset);
			}
			return encoding.GetString(data.Array!, data.Offset, data.Count);
		}

		private void Perform()
		{
			int id = 0;
			lock (CallbackLock)
			{
				while (Requests.ContainsKey(id))
				{
					id = IdGenerator.Next();
				}

				Requests[id] = this;
			}
			try
			{
				this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.WriteData, ref id));
				this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.ReadData, ref id));
				this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.XferInfoData, ref id));
				this.ThrowIfNotOk(_memory.Curl.EasyPerform());
			}
			finally
			{
				_contentLengthFetched = false;
				lock (CallbackLock)
				{
					Requests.Remove(id);
				}
			}
		}

		public Curl() : this(16384)
		{
		}

		public Curl(int bufferSize)
		{
			if (_initialized != CurlCode.Ok)
			{
				if (_initialized == CurlCode.NotInitialized)
				{
					throw new CurlNotInitializedException();
				}

				throw new CurlException(_initialized, this);
			}

			_downloadBuffer = new byte[bufferSize];
			_memory = MemoryHolder.Get();

			SetOptions();
			UserAgent = CurlVersion;
			IpMode = IpResolveMode.Whatever;
		}

		private void SetOptions()
		{
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.ErrorBuffer, _memory.ErrorBuffer));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.Share, CurlShare.Handle));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.NoProgress, false));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.AcceptEncoding, ""));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.WriteFunction, WriteFunctionCallback));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.ReadFunction, ReadFunctionCallback));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.XferInfoFunction, ProgressFunctionCallback));
		}

		~Curl()
		{
			Cleanup();
		}

		public void Dispose()
		{
			Cleanup();
			GC.SuppressFinalize(this);
		}

		private void Cleanup()
		{
			if (_disposed)
			{
				return;
			}
			MemoryHolder.Free(_memory);
			_disposed = true;
		}

		public void Reset()
		{
			_downloadOffset = 0;
			_uploadOffset = 0;

			_memory.Curl.EasyReset();

			SetOptions();
			SetUseragent(_userAgent);
			SetIpMode(IpMode);
		}
	}
}
