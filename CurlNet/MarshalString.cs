using System;
using System.Runtime.InteropServices;
using System.Text;
using CurlNet.Exceptions;
// ReSharper disable MemberCanBePrivate.Global

namespace CurlNet
{
	internal static unsafe class MarshalString
	{
		internal static IntPtr StringToUtf8(string input)
		{
			if (input == null) return IntPtr.Zero;
			fixed (char* pInput = input)
			{
				int length = Encoding.UTF8.GetByteCount(pInput, input.Length);
				IntPtr buffer = Marshal.AllocHGlobal(length + 1);
				byte* pResult = (byte*)buffer.ToPointer();
				int bytesWritten = Encoding.UTF8.GetBytes(pInput, input.Length, pResult, length);
				if (bytesWritten != length) throw new MarshalStringException("String UTF-8 encoding failed!");
				pResult[length] = 0;
				return buffer;
			}
		}

		internal static string Utf8ToString(IntPtr pStringUtf8)
		{
			return Utf8ToString((byte*)pStringUtf8.ToPointer());
		}
		
		internal static string Utf8ToString(byte* pStringUtf8)
		{
			if (pStringUtf8 == null) return null;
			int len = 0;
			while (pStringUtf8[len] != 0) len++;
			return Encoding.UTF8.GetString(pStringUtf8, len);
		}

		internal static string NativeToString(IntPtr pString, Encoding encoding)
		{
			return NativeToString((byte*)pString.ToPointer(), encoding);
		}

		internal static string NativeToString(byte* pString, Encoding encoding)
		{
			if (pString == null) return null;
			int len = 0;
			while (pString[len] != 0) len++;
			return encoding.GetString(pString, len);
		}
	}
}
