using CurlNet.Enums;
using CurlNet.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace CurlNet
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public sealed class Curl : IDisposable
	{
		private const int ErrorBufferSize = 256;
		private static CurlCode _initialized = CurlCode.NotInitialized;

		private readonly IntPtr _curl;
		private readonly IntPtr _errorBuffer;

		private byte[] _buffer;
		private int _offset;
		private bool _disposed;

		public static readonly string CurlVersion = MarshalString.Utf8ToString(CurlNative.GetVersion());

		public bool UseBom = false;

		private string _userAgent;
		public string UserAgent
		{
			get => _userAgent;
			set
			{
				_userAgent = value;
				SetUseragent(value);
			}
		}

		public static bool Initialize()
		{
			if (_initialized != CurlCode.Ok)
			{
				_initialized = CurlNative.GlobalInit(CurlGlobal.Default);
			}

			return _initialized == CurlCode.Ok;
		}

		public static void Deinitialize()
		{
			if (_initialized != CurlCode.NotInitialized)
			{
				CurlNative.GlobalCleanup();
				_initialized = CurlCode.NotInitialized;
			}
		}

		public static string GetVersion()
		{
			return MarshalString.Utf8ToString(CurlNative.GetVersion());
		}

		internal string GetError(CurlCode code)
		{
			if (code == CurlCode.NotInitialized)
			{
				return "Curl Global not initialized";
			}

			string error = MarshalString.Utf8ToString(_errorBuffer);
			return string.IsNullOrEmpty(error) ? MarshalString.Utf8ToString(CurlNative.EasyStrError(code)) : error;
		}

		private void SetUseragent(string userAgent)
		{
			IntPtr useragentpointer = IntPtr.Zero;
			try
			{
				useragentpointer = MarshalString.StringToUtf8(userAgent);
				CurlCode result1 = CurlNative.EasySetOpt(_curl, CurlOption.Useragent, useragentpointer);
				if (result1 != CurlCode.Ok)
				{
					throw new CurlException(result1, this);
				}
			}
			finally
			{
				if (useragentpointer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(useragentpointer);
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
			_buffer = new byte[bufferSize];
			_errorBuffer = Marshal.AllocHGlobal(ErrorBufferSize);
			_curl = CurlNative.EasyInit();
			if (_curl == IntPtr.Zero)
			{
				throw new CurlEasyInitializeException("Curl Easy failed to initialize!");
			}

			SetOptions();
			UserAgent = CurlVersion;
		}

		private void SetOptions()
		{
			CurlCode result = CurlNative.EasySetOpt(_curl, CurlOption.Errorbuffer, _errorBuffer);
			if (result != CurlCode.Ok)
			{
				throw new CurlException(result, this);
			}

			CurlCode result1 = CurlNative.EasySetOpt(_curl, CurlOption.WriteData, this);
			if (result1 != CurlCode.Ok)
			{
				throw new CurlException(result1, this);
			}

			CurlCode result2 = CurlNative.EasySetOpt(_curl, CurlOption.WriteFunction, WriteCallback);
			if (result2 != CurlCode.Ok)
			{
				throw new CurlException(result2, this);
			}
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
			if (_errorBuffer != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(_errorBuffer);
			}
			if (_curl != IntPtr.Zero)
			{
				CurlNative.EasyCleanup(_curl);
			}
			_disposed = true;
		}

		public void Reset()
		{
			_offset = 0;
			CurlNative.EasyReset(_curl);

			SetOptions();
			SetUseragent(_userAgent);
		}

		public string GetText(string url)
		{
			return GetText(url, Encoding.UTF8);
		}

		public string GetText(string url, Encoding encoding)
		{
			ArraySegment<byte> bytes = GetBytes(url);
			// ReSharper disable once ConvertIfStatementToReturnStatement
			// ReSharper disable AssignNullToNotNullAttribute
			if (UseBom)
			{
				return BomUtil.GetEncoding(bytes, encoding, out int offset).GetString(bytes.Array, bytes.Offset + offset, bytes.Count - offset);
			}
			return encoding.GetString(bytes.Array, bytes.Offset, bytes.Count);
			// ReSharper restore AssignNullToNotNullAttribute
		}

		public ArraySegment<byte> GetBytes(string url)
		{
			IntPtr urlpointer = IntPtr.Zero;

			try
			{
				urlpointer = MarshalString.StringToUtf8(url);
				CurlCode result = CurlNative.EasySetOpt(_curl, CurlOption.Url, urlpointer);
				if (result != CurlCode.Ok)
				{
					throw new CurlException(result, this);
				}

				CurlCode result1 = CurlNative.EasyPerform(_curl);
				if (result1 != CurlCode.Ok)
				{
					throw new CurlException(result1, this);
				}

				return new ArraySegment<byte>(_buffer, 0, _offset);
			}
			finally
			{
				if (urlpointer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(urlpointer);
				}
			}
		}

		public string Post(string url, string data)
		{
			return Post(url, data, Encoding.UTF8);
		}

		public string Post(string url, string data, Encoding encoding)
		{
			IntPtr urlpointer = IntPtr.Zero;
			IntPtr datapointer = IntPtr.Zero;

			try
			{
				urlpointer = MarshalString.StringToUtf8(url);
				CurlCode result = CurlNative.EasySetOpt(_curl, CurlOption.Url, urlpointer);
				if (result != CurlCode.Ok)
				{
					throw new CurlException(result, this);
				}

				datapointer = MarshalString.StringToUtf8(data);
				CurlCode result1 = CurlNative.EasySetOpt(_curl, CurlOption.Postfields, datapointer);
				if (result1 != CurlCode.Ok)
				{
					throw new CurlException(result1, this);
				}

				CurlCode result2 = CurlNative.EasyPerform(_curl);
				if (result2 != CurlCode.Ok)
				{
					throw new CurlException(result2, this);
				}
				
				return encoding.GetString(_buffer, 0, _offset);
			}
			finally
			{
				if (urlpointer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(urlpointer);
				}
				if (datapointer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(datapointer);
				}
			}
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
	}
}
