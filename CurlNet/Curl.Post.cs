using System;
using System.Text;
using CurlNet.Enums;
using CurlNet.Exceptions;

namespace CurlNet
{
	public sealed partial class Curl
	{
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
				bytes.Free();
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
				bytes.Free();
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
				bytes.Free();
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
				bytes.Free();
			}
		}

		private ArraySegment<byte> Post(string url, IntPtr data, int size)
		{
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.Url, url));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.PostFieldSize, size));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.PostFields, data));
			Perform();
			this.ThrowIfNotOk(_memory.Curl.EasyGetInfo(CurlInfo.ResponseCode, out int response));
			Response = response;

			return new ArraySegment<byte>(_downloadBuffer, 0, _downloadOffset);
		}
	}
}