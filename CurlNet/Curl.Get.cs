using System;
using System.Text;
using CurlNet.Enums;
using CurlNet.Exceptions;

namespace CurlNet
{
	public sealed partial class Curl
	{
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
			this.ThrowIfNotOk(_memory.Curl.EasySetOpt(CurlOption.Url, url));
			Perform();
			this.ThrowIfNotOk(_memory.Curl.EasyGetInfo(CurlInfo.ResponseCode, out int response));
			Response = response;

			return new ArraySegment<byte>(_downloadBuffer, 0, _downloadOffset);
		}
	}
}