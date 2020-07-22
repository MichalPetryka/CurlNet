using CurlNet.Exceptions;
using System;
using System.Runtime.InteropServices;

namespace CurlNet.Memory
{
	internal sealed class NativeMemory : IDisposable
	{
		private const int ErrorBufferSize = 256;

		internal CurlNative.CurlHandle Curl;
		internal IntPtr ErrorBuffer;

		internal NativeMemory()
		{
			ErrorBuffer = Marshal.AllocCoTaskMem(ErrorBufferSize);
			Curl = CurlNative.EasyInit();
			if (Curl == CurlNative.CurlHandle.Null)
			{
				throw new CurlEasyInitializeException("Curl Easy failed to initialize!");
			}
		}

		internal string GetError()
		{
			return MarshalString.NativeToString(ErrorBuffer);
		}

		private void ReleaseUnmanagedResources()
		{
			ErrorBuffer.Free();
			if (Curl != CurlNative.CurlHandle.Null)
			{
				Curl.EasyCleanup();
			}
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~NativeMemory()
		{
			ReleaseUnmanagedResources();
		}
	}
}
