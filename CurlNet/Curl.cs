﻿using CurlNet.Enums;
using CurlNet.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Text;
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
		private const int ErrorBufferSize = 256;
		private static CurlCode _initialized = CurlCode.NotInitialized;

		private readonly IntPtr _curl;
		private readonly IntPtr _errorBuffer;

		private byte[] _buffer;
		private int _offset;
		private bool _disposed;

		private string _userAgent;
		private IpResolveMode _ipMode;
		private Progress _downloadProgress;
		private Progress _uploadProgress;

		public static readonly string CurlVersion = CurlNative.GetVersion();

		public bool UseBom = false;

		public delegate void ProgressHandler(Curl sender, Progress progress);
		public event ProgressHandler DownloadProgressChange;
		public event ProgressHandler UploadProgressChange;

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

		internal string GetError(CurlCode code)
		{
			if (code == CurlCode.NotInitialized)
			{
				return "Curl Global not initialized";
			}

			string error = MarshalString.Utf8ToString(_errorBuffer);
			return string.IsNullOrEmpty(error) ? CurlNative.GetErrorMessage(code) : error;
		}

		private void SetUseragent(string userAgent)
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.Useragent, userAgent), this);
		}

		private void SetIpMode(IpResolveMode mode)
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.Useragent, mode), this);
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
			IpMode = IpResolveMode.Whatever;
		}

		private void SetOptions()
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.Errorbuffer, _errorBuffer), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.WriteData, this), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.WriteFunction, WriteCallback), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.NoProgress, 0), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.XferInfoData, this), this);
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.XferInfoFunction, ProgressCallback), this);
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
			MarshalString.FreeIfNotZero(_errorBuffer);
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
			SetIpMode(IpMode);
		}

		public string GetText(string url)
		{
			return GetText(url, Encoding.UTF8);
		}

		public string GetText(string url, Encoding encoding)
		{
			ArraySegment<byte> bytes = GetBytes(url);
			if (UseBom)
			{
				return BomUtil.GetEncoding(bytes, encoding, out int offset).GetString(bytes.Array, bytes.Offset + offset, bytes.Count - offset);
			}
			return encoding.GetString(bytes.Array, bytes.Offset, bytes.Count);
		}

		public ArraySegment<byte> GetBytes(string url)
		{
			CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.Url, url), this);
			CurlException.ThrowIfNotOk(CurlNative.EasyPerform(_curl), this);

			return new ArraySegment<byte>(_buffer, 0, _offset);
		}

		public string Post(string url, string data)
		{
			return Post(url, data, Encoding.UTF8);
		}

		public string Post(string url, string data, Encoding encoding)
		{
			IntPtr post = IntPtr.Zero;
			try
			{
				CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.Url, url), this);
				post = MarshalString.StringToUtf8(data, out int length);
				CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.Postfieldsize, length), this);
				CurlException.ThrowIfNotOk(CurlNative.EasySetOpt(_curl, CurlOption.Postfields, post), this);
				CurlException.ThrowIfNotOk(CurlNative.EasyPerform(_curl), this);

				return encoding.GetString(_buffer, 0, _offset);
			}
			finally
			{
				MarshalString.FreeIfNotZero(post);
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
