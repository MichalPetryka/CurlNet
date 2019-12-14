using System;
using System.Runtime.InteropServices;
using System.Text;
#if !NETSTANDARD1_1
using CurlNet.Exceptions;
#endif
// ReSharper disable MemberCanBePrivate.Global

namespace CurlNet
{
	internal static unsafe class MarshalString
	{
		internal static IntPtr StringToUtf8(string input)
		{
			if (input == null) return IntPtr.Zero;
#if NETSTANDARD1_1
			byte[] data = Encoding.UTF8.GetBytes(input);
			IntPtr buffer = Marshal.AllocHGlobal(data.Length + 1);
			Marshal.Copy(data, 0, buffer, data.Length);
			((byte*)buffer.ToPointer())[data.Length + 1] = 0;
			return buffer;
#else
			fixed (char* pInput = input)
			{
				int length = Encoding.UTF8.GetByteCount(input);
				IntPtr buffer = Marshal.AllocHGlobal(length + 1);
				byte* pResult = (byte*)buffer.ToPointer();
				int bytesWritten = Encoding.UTF8.GetBytes(pInput, input.Length, pResult, length);
				if (bytesWritten != length) throw new MarshalStringException();
				pResult[length] = 0;
				return buffer;
			}
#endif
		}

		internal static IntPtr StringToUtf8(string input, out int length)
		{
			if (input == null)
			{
				length = 0;
				return IntPtr.Zero;
			}
#if NETSTANDARD1_1
			byte[] data = Encoding.UTF8.GetBytes(input);
			length = data.Length;
			IntPtr buffer = Marshal.AllocHGlobal(length + 1);
			Marshal.Copy(data, 0, buffer, length);
			((byte*)buffer.ToPointer())[length + 1] = 0;
			return buffer;
#else
			fixed (char* pInput = input)
			{
				length = Encoding.UTF8.GetByteCount(input);
				IntPtr buffer = Marshal.AllocHGlobal(length + 1);
				byte* pResult = (byte*)buffer.ToPointer();
				int bytesWritten = Encoding.UTF8.GetBytes(pInput, input.Length, pResult, length);
				if (bytesWritten != length) throw new MarshalStringException();
				pResult[length] = 0;
				return buffer;
			}
#endif
		}

		internal static void FreeIfNotZero(IntPtr text)
		{
			if (text != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(text);
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
#if NET35 || NETSTANDARD1_1
			byte[] data = new byte[len];
			Marshal.Copy((IntPtr)pStringUtf8, data, 0, len);
			return Encoding.UTF8.GetString(data, 0, len);
#else
			return Encoding.UTF8.GetString(pStringUtf8, len);
#endif
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
#if NET35 || NETSTANDARD1_1
			byte[] data = new byte[len];
			Marshal.Copy((IntPtr)pString, data, 0, len);
			return encoding.GetString(data, 0, len);
#else
			return encoding.GetString(pString, len);
#endif
		}
	}
}
