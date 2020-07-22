using System;
using System.Runtime.InteropServices;
using System.Text;
using CurlNet.Exceptions;
// ReSharper disable MemberCanBePrivate.Global

namespace CurlNet
{
	internal static unsafe class MarshalString
	{
		internal static byte[] ToArray(this IntPtr pointer, int length)
		{
			byte[] data = new byte[length];
			Marshal.Copy(pointer, data, 0, length);
			return data;
		}

		internal static IntPtr ToIntPtr(this byte[] bytes)
		{
			IntPtr data = Marshal.AllocCoTaskMem(bytes.Length);
			Marshal.Copy(bytes, 0, data, bytes.Length);
			return data;
		}

		internal static IntPtr StringToNative(string input)
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1
			return Marshal.StringToCoTaskMemUTF8(input);
#else
			return StringToNative(input, Encoding.UTF8, out _);
#endif
		}

		internal static IntPtr StringToNative(string input, Encoding encoding, out int length)
		{
			if (input == null)
			{
				length = 0;
				return IntPtr.Zero;
			}
			length = encoding.GetByteCount(input);
			IntPtr buffer = Marshal.AllocCoTaskMem(length + 1);
			byte* pResult = (byte*)buffer.ToPointer();
			int bytesWritten;
			fixed (char* pInput = input) 
				bytesWritten = encoding.GetBytes(pInput, input.Length, pResult, length);
			if (bytesWritten != length) throw new MarshalStringException();
			pResult[length] = 0;
			return buffer;
		}

		internal static void Free(this IntPtr pointer)
		{
			Marshal.FreeCoTaskMem(pointer);
		}

		internal static string NativeToString(IntPtr pStringUtf8)
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1
			return Marshal.PtrToStringUTF8(pStringUtf8);
#else
			return NativeToString((byte*)pStringUtf8.ToPointer(), Encoding.UTF8);
#endif
		}

		internal static string NativeToString(byte* pString)
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1
			return Marshal.PtrToStringUTF8(new IntPtr(pString));
		}
#else
			return NativeToString(pString, Encoding.UTF8);
		}

		private static string NativeToString(byte* pString, Encoding encoding)
		{
			if (pString == null) return null;
			int len = 0;
			while (pString[len] != 0) len++;
			return encoding.GetString(pString, len);
		}
#endif
	}
}
