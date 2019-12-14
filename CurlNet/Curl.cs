using CurlNet.Enums;
using CurlNet.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Text;
using CurlNet.Memory;
// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable ConvertToConstant.Global

namespace CurlNet
{
	public sealed class Curl : IDisposable
	{
		private static CurlCode _initialized = CurlCode.NotInitialized;
		private static IMemoryHolder _memoryHolder;

		private readonly NativeMemory _memory;
		private readonly CurlNative.WriteFunctionCallback _writeFunctionCallback;
		private readonly CurlNative.ProgressFunctionCallback _progressFunctionCallback;

		private bool _disposed;
		private byte[] _buffer;
		private int _offset;

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
				_downloadProgress = value;
				DownloadProgressChange?.Invoke(this, value);
			}
		}

		public Progress UploadProgress
		{
			get => _uploadProgress;
			private set
			{
				_uploadProgress = value;
				UploadProgressChange?.Invoke(this, value);
			}
		}

		public static bool Initialize(bool poolMemory)
		{
			if (_initialized != CurlCode.Ok)
			{
				_initialized = CurlNative.GlobalInit(CurlGlobal.Default);
				if (poolMemory)
					_memoryHolder = new MemoryPool();
				else
					_memoryHolder = new MemoryFactory();
			}

			return _initialized == CurlCode.Ok;
		}

		public static void Deinitialize()
		{
			if (_initialized != CurlCode.NotInitialized)
			{
				CurlNative.GlobalCleanup();
				_memoryHolder.Clear();
				_memoryHolder = null;
				_initialized = CurlCode.NotInitialized;
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
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.UserAgent, userAgent), this);
		}

		private void SetIpMode(IpResolveMode mode)
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.UserAgent, mode), this);
		}

		private string ToText(ArraySegment<byte> data, Encoding encoding)
		{
			if (UseBom)
			{
				return BomUtil.GetEncoding(data, encoding, out int offset).GetString(data.Array, data.Offset + offset, data.Count - offset);
			}
			return encoding.GetString(data.Array, data.Offset, data.Count);
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

			_buffer = new byte[bufferSize];
			_memory = _memoryHolder.GetMemory();
			_writeFunctionCallback = WriteCallback;
			_progressFunctionCallback = ProgressCallback;

			SetOptions();
			UserAgent = CurlVersion;
			IpMode = IpResolveMode.Whatever;
		}

		private void SetOptions()
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.ErrorBuffer, _memory.ErrorBuffer), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.WriteData, this), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.WriteFunction, _writeFunctionCallback), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.NoProgress, false), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.XferInfoData, this), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.XferInfoFunction, _progressFunctionCallback), this);
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
			_memoryHolder.FreeMemory(_memory);
			_disposed = true;
		}

		public void Reset()
		{
			_offset = 0;
			CurlNative.EasyReset(_memory.Curl);

			SetOptions();
			SetUseragent(_userAgent);
			SetIpMode(IpMode);
		}

		public string GetString(string url)
		{
			return GetString(url, Encoding.UTF8);
		}

		public string GetString(string url, Encoding encoding)
		{
			return ToText(GetBytes(url), encoding);
		}

		public ArraySegment<byte> GetBytes(string url)
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.Url, url), this);
			CurlException.ThrowIfNotOk(CurlNative.EasyPerform(_memory.Curl), this);
			CurlException.ThrowIfNotOk(CurlNative.EasyGetInfo(_memory.Curl, CurlInfo.ResponseCode, out int response), this);
			Response = response;

			return new ArraySegment<byte>(_buffer, 0, _offset);
		}

		public string PostString(string url, string data)
		{
			return PostString(url, data, Encoding.UTF8);
		}

		public string PostString(string url, string data, Encoding encoding)
		{
			IntPtr text = IntPtr.Zero;
			try
			{
				text = MarshalString.StringToUtf8(data, out int length);
				return ToText(Post(url, text, length), encoding);
			}
			finally
			{
				MarshalString.FreeIfNotZero(text);
			}
		}

		public string PostString(string url, byte[] data)
		{
			return PostString(url, data, Encoding.UTF8);
		}

		public string PostString(string url, byte[] data, Encoding encoding)
		{
			IntPtr bytes = IntPtr.Zero;
			try
			{
				bytes = Marshal.AllocHGlobal(data.Length);
				return ToText(Post(url, bytes, data.Length), encoding);
			}
			finally
			{
				MarshalString.FreeIfNotZero(bytes);
			}
		}

		public ArraySegment<byte> PostBytes(string url, string data)
		{
			IntPtr text = IntPtr.Zero;
			try
			{
				text = MarshalString.StringToUtf8(data, out int length);
				return Post(url, text, length);
			}
			finally
			{
				MarshalString.FreeIfNotZero(text);
			}
		}

		public ArraySegment<byte> PostBytes(string url, byte[] data)
		{
			IntPtr bytes = IntPtr.Zero;
			try
			{
				bytes = Marshal.AllocHGlobal(data.Length);
				return Post(url, bytes, data.Length);

			}
			finally
			{
				MarshalString.FreeIfNotZero(bytes);
			}
		}

		private ArraySegment<byte> Post(string url, IntPtr data, int size)
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.Url, url), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.PostFieldSize, size), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.PostFields, data), this);
			CurlException.ThrowIfNotOk(CurlNative.EasyPerform(_memory.Curl), this);
			CurlException.ThrowIfNotOk(CurlNative.EasyGetInfo(_memory.Curl, CurlInfo.ResponseCode, out int response), this);
			Response = response;

			return new ArraySegment<byte>(_buffer, 0, _offset);
		}

		[MonoPInvokeCallback(typeof(CurlNative.WriteFunctionCallback))]
		private static UIntPtr WriteCallback(IntPtr pointer, UIntPtr size, UIntPtr nmemb, Curl curl)
		{
			int length = (int)size * (int)nmemb;
			if (curl._buffer.Length < curl._offset + length)
			{
				Array.Resize(ref curl._buffer, Math.Max(2 * curl._buffer.Length, curl._offset + length));
			}
			Marshal.Copy(pointer, curl._buffer, curl._offset, length);
			curl._offset += length;
			return new UIntPtr((uint)length);
		}

		[MonoPInvokeCallback(typeof(CurlNative.ProgressFunctionCallback))]
		private static short ProgressCallback(Curl clientp, IntPtr dltotal, IntPtr dlnow, IntPtr ultotal, IntPtr ulnow)
		{
			int downloaded = (int)dlnow;
			int downloadTotal = (int)dltotal;
			if (downloaded != 0 && downloadTotal != 0)
			{
				clientp.DownloadProgress = new Progress(downloaded, downloadTotal);
			}

			int uploaded = (int)ulnow;
			int uploadTotal = (int)ultotal;
			if (uploaded != 0 && uploadTotal != 0)
			{
				clientp.UploadProgress = new Progress(uploaded, uploadTotal);
			}
			return 0;
		}

		public readonly struct Progress : IEquatable<Progress>
		{
			public readonly int Done;
			public readonly int Total;

			public Progress(int done, int total)
			{
				Done = done;
				Total = total;
			}

			public bool Equals(Progress other)
			{
				return Done == other.Done && Total == other.Total;
			}

			public override bool Equals(object obj)
			{
				return obj is Progress other && Equals(other);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					return (Done * 397) ^ Total;
				}
			}

			public static bool operator ==(Progress left, Progress right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(Progress left, Progress right)
			{
				return !left.Equals(right);
			}
		}
	}
}
