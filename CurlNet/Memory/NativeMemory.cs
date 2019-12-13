using CurlNet.Exceptions;
using System;
using System.Runtime.InteropServices;

namespace CurlNet.Memory
{
	internal sealed class NativeMemory : IDisposable
	{
		private const int ErrorBufferSize = 256;

		internal IntPtr Curl;
		internal IntPtr ErrorBuffer;

		internal NativeMemory()
		{
			ErrorBuffer = Marshal.AllocHGlobal(ErrorBufferSize);
			Curl = CurlNative.EasyInit();
			if (Curl == IntPtr.Zero)
			{
				throw new CurlEasyInitializeException("Curl Easy failed to initialize!");
			}
		}

		internal string GetError()
		{
			return MarshalString.Utf8ToString(ErrorBuffer);
		}

		private void ReleaseUnmanagedResources()
		{
			MarshalString.FreeIfNotZero(ErrorBuffer);
			if (Curl != IntPtr.Zero)
			{
				CurlNative.EasyCleanup(Curl);
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
