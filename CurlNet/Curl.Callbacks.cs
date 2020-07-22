using System;
using System.Runtime.InteropServices;
using CurlNet.Enums;
using CurlNet.Exceptions;

namespace CurlNet
{
	public sealed partial class Curl
	{
		[MonoPInvokeCallback(typeof(CurlNative.WriteFunctionCallback))]
		private static CurlNative.OffT WriteCallback(IntPtr pointer, CurlNative.OffT size, CurlNative.OffT nmemb, ref int instance)
		{
			Curl curl;
			lock (CallbackLock)
				curl = Requests[instance];

			int length = size * nmemb;
			int bufferLength = curl._downloadOffset + length;
			if (!curl._contentLengthFetched)
			{
				curl.ThrowIfNotOk(curl._memory.Curl.EasyGetInfo(CurlInfo.ContentLengthDownloadT, out CurlNative.OffT requestedLength));
				bufferLength = Math.Max(bufferLength, requestedLength);
				curl._contentLengthFetched = true;
			}
			if (curl._downloadBuffer.Length < bufferLength)
			{
				Array.Resize(ref curl._downloadBuffer, Math.Max(2 * curl._downloadBuffer.Length, bufferLength));
			}

			Marshal.Copy(pointer, curl._downloadBuffer, curl._downloadOffset, length);
			curl._downloadOffset += length;
			return new CurlNative.OffT((uint)length);
		}

		[MonoPInvokeCallback(typeof(CurlNative.ReadFunctionCallback))]
		private static CurlNative.OffT ReadCallback(IntPtr pointer, CurlNative.OffT size, CurlNative.OffT nmemb, ref int instance)
		{
			Curl curl;
			lock (CallbackLock)
				curl = Requests[instance];
			int length = size * nmemb;
			int dataLeft = curl._uploadBuffer.Length - curl._uploadOffset;
			int toCopy = Math.Min(length, dataLeft);
			Marshal.Copy(curl._uploadBuffer, curl._uploadOffset, pointer, toCopy);
			curl._uploadOffset += toCopy;
			return new CurlNative.OffT((uint)(curl._uploadBuffer.Length - curl._uploadOffset));
		}

		[MonoPInvokeCallback(typeof(CurlNative.ProgressFunctionCallback))]
		private static int ProgressCallback(ref int instance, CurlNative.OffT dltotal, CurlNative.OffT dlnow, CurlNative.OffT ultotal, CurlNative.OffT ulnow)
		{
			Curl curl;
			lock (CallbackLock)
				curl = Requests[instance];

			int downloaded = dlnow;
			int downloadTotal = dltotal;
			if (downloaded != 0 && downloadTotal != 0)
			{
				curl.DownloadProgress = new Progress(downloaded, downloadTotal);
			}

			int uploaded = ulnow;
			int uploadTotal = ultotal;
			if (uploaded != 0 && uploadTotal != 0)
			{
				curl.UploadProgress = new Progress(uploaded, uploadTotal);
			}

			return 0;
		}
	}
}