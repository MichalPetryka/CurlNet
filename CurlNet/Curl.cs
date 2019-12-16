using CurlNet.Enums;
using CurlNet.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Text;
using CurlNet.Memory;
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace CurlNet
{
	public sealed partial class Curl : IDisposable
	{
		private static CurlCode _initialized = CurlCode.NotInitialized;
		private static IMemoryHolder _memoryHolder;

		[ThreadStatic]
		private static Curl _localCurl;

		private readonly NativeMemory _memory;
		private readonly CurlNative.WriteFunctionCallback _writeFunctionCallback;
		private readonly CurlNative.ReadFunctionCallback _readFunctionCallback;
		private readonly CurlNative.ProgressFunctionCallback _progressFunctionCallback;

		private bool _disposed;
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
				_downloadProgress = value;
				if (DownloadProgressChange != null)
				{
					_localCurl = null;
					DownloadProgressChange(this, value);
					_localCurl = this;
				}
			}
		}

		public Progress UploadProgress
		{
			get => _uploadProgress;
			private set
			{
				_uploadProgress = value;
				if (UploadProgressChange != null)
				{
					_localCurl = null;
					UploadProgressChange(this, value);
					_localCurl = this;
				}
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

		private void Perform()
		{
			_localCurl = this;
			try
			{
				CurlException.ThrowIfNotOk(CurlNative.EasyPerform(_memory.Curl), this);
			}
			finally
			{
				_localCurl = null;
			}
		}

		private static void ThrowIfActive()
		{
			if (_localCurl != null)
				throw new ThreadStaticOccupiedException(_localCurl);
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
			_memory = _memoryHolder.GetMemory();
			_writeFunctionCallback = WriteCallback;
			_readFunctionCallback = ReadCallback;
			_progressFunctionCallback = ProgressCallback;

			SetOptions();
			UserAgent = CurlVersion;
			IpMode = IpResolveMode.Whatever;
		}

		private void SetOptions()
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.ErrorBuffer, _memory.ErrorBuffer), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.NoProgress, false), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.WriteData, IntPtr.Zero), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.ReadData, IntPtr.Zero), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.XferInfoData, IntPtr.Zero), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.WriteFunction, _writeFunctionCallback), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.ReadFunction, _readFunctionCallback), this);
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
			_downloadOffset = 0;
			_uploadOffset = 0;

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

		private ArraySegment<byte> GetBytes(string url)
		{
			ThrowIfActive();
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.Url, url), this);
			Perform();
			CurlException.ThrowIfNotOk(CurlNative.EasyGetInfo(_memory.Curl, CurlInfo.ResponseCode, out int response), this);
			Response = response;

			return new ArraySegment<byte>(_downloadBuffer, 0, _downloadOffset);
		}

		public string PostString(string url, string data)
		{
			return PostString(url, data, Encoding.UTF8);
		}

		public string PostString(string url, string data, Encoding encoding)
		{
			IntPtr bytes = IntPtr.Zero;
			try
			{
				bytes = MarshalString.StringToNative(data, encoding, out int length);
				return ToText(Post(url, bytes, length), encoding);
			}
			finally
			{
				bytes.FreeIfNotZero();
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
				bytes = data.ToIntPtr();
				return ToText(Post(url, bytes, data.Length), encoding);
			}
			finally
			{
				bytes.FreeIfNotZero();
			}
		}

		public ArraySegment<byte> PostBytes(string url, string data)
		{
			return PostBytes(url, data, Encoding.UTF8);
		}

		public ArraySegment<byte> PostBytes(string url, string data, Encoding encoding)
		{
			IntPtr bytes = IntPtr.Zero;
			try
			{
				bytes = MarshalString.StringToNative(data, encoding, out int length);
				return Post(url, bytes, length);
			}
			finally
			{
				bytes.FreeIfNotZero();
			}
		}

		public ArraySegment<byte> PostBytes(string url, byte[] data)
		{
			IntPtr bytes = IntPtr.Zero;
			try
			{
				bytes = data.ToIntPtr();
				return Post(url, bytes, data.Length);
			}
			finally
			{
				bytes.FreeIfNotZero();
			}
		}

		private ArraySegment<byte> Post(string url, IntPtr data, int size)
		{
			ThrowIfActive();
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.Url, url), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.PostFieldSize, size), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.PostFields, data), this);
			Perform();
			CurlException.ThrowIfNotOk(CurlNative.EasyGetInfo(_memory.Curl, CurlInfo.ResponseCode, out int response), this);
			Response = response;

			return new ArraySegment<byte>(_downloadBuffer, 0, _downloadOffset);
		}

		public string PutString(string url, string data)
		{
			return PutString(url, data, Encoding.UTF8);
		}

		public string PutString(string url, string data, Encoding encoding)
		{
			return ToText(PutBytes(url, encoding.GetBytes(data)), encoding);
		}

		public string PutString(string url, byte[] data)
		{
			return PutString(url, data, Encoding.UTF8);
		}

		public string PutString(string url, byte[] data, Encoding encoding)
		{
			return ToText(PutBytes(url, data), encoding);
		}

		public ArraySegment<byte> PutBytes(string url, string data)
		{
			return PutBytes(url, data, Encoding.UTF8);
		}

		public ArraySegment<byte> PutBytes(string url, string data, Encoding encoding)
		{
			return PutBytes(url, encoding.GetBytes(data));
		}

		private ArraySegment<byte> PutBytes(string url, byte[] bytes)
		{
			ThrowIfActive();
			_uploadBuffer = bytes;
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.Url, url), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.Upload, true), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_memory.Curl, CurlOption.Infilesize, bytes.Length), this);
			Perform();
			CurlException.ThrowIfNotOk(CurlNative.EasyGetInfo(_memory.Curl, CurlInfo.ResponseCode, out int response), this);
			Response = response;

			return new ArraySegment<byte>(_downloadBuffer, 0, _downloadOffset);
		}

		[MonoPInvokeCallback(typeof(CurlNative.WriteFunctionCallback))]
		private static UIntPtr WriteCallback(IntPtr pointer, UIntPtr size, UIntPtr nmemb, IntPtr userData)
		{
			Curl curl = _localCurl;
			int length = (int)size * (int)nmemb;
			if (curl._downloadBuffer.Length < curl._downloadOffset + length)
			{
				Array.Resize(ref curl._downloadBuffer, Math.Max(2 * curl._downloadBuffer.Length, curl._downloadOffset + length));
			}
			Marshal.Copy(pointer, curl._downloadBuffer, curl._downloadOffset, length);
			curl._downloadOffset += length;
			return new UIntPtr((uint)length);
		}

		[MonoPInvokeCallback(typeof(CurlNative.ReadFunctionCallback))]
		private static UIntPtr ReadCallback(IntPtr pointer, UIntPtr size, UIntPtr nmemb, IntPtr userData)
		{
			Curl curl = _localCurl;
			int length = (int)size * (int)nmemb;
			int dataLeft = curl._uploadBuffer.Length - curl._uploadOffset;
			int toCopy = Math.Min(length, dataLeft);
			Marshal.Copy(curl._uploadBuffer, curl._uploadOffset, pointer, toCopy);
			curl._uploadOffset += toCopy;
			return new UIntPtr((uint)(curl._uploadBuffer.Length - curl._uploadOffset));
		}

		[MonoPInvokeCallback(typeof(CurlNative.ProgressFunctionCallback))]
		private static short ProgressCallback(IntPtr clientp, IntPtr dltotal, IntPtr dlnow, IntPtr ultotal, IntPtr ulnow)
		{
			int downloaded = (int)dlnow;
			int downloadTotal = (int)dltotal;
			if (downloaded != 0 && downloadTotal != 0)
			{
				_localCurl.DownloadProgress = new Progress(downloaded, downloadTotal);
			}

			int uploaded = (int)ulnow;
			int uploadTotal = (int)ultotal;
			if (uploaded != 0 && uploadTotal != 0)
			{
				_localCurl.UploadProgress = new Progress(uploaded, uploadTotal);
			}
			return 0;
		}
	}
}
