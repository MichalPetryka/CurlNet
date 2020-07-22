using System;
using System.Text;
using CurlNet.Enums;
using CurlNet.Exceptions;

namespace CurlNet
{
	public sealed partial class Curl
	{
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
			_uploadBuffer = bytes;
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.Url, url));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.Upload, true));
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.Infilesize, bytes.Length));
			Perform();
			this.ThrowIfNotOk(_memory.Curl.EasyGetInfo(CurlInfo.ResponseCode, out int response));
			Response = response;

			return new ArraySegment<byte>(_downloadBuffer, 0, _downloadOffset);
		}
	}
}