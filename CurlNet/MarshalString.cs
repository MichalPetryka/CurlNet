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
			IntPtr data = Marshal.AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, data, bytes.Length);
			return data;
		}

		internal static IntPtr StringToNative(string input)
		{
			return StringToNative(input, Encoding.UTF8, out _);
		}

		internal static IntPtr StringToNative(string input, out int length)
		{
			return StringToNative(input, Encoding.UTF8, out length);
		}

		internal static IntPtr StringToNative(string input, Encoding encoding)
		{
			return StringToNative(input, encoding, out _);
		}

		internal static IntPtr StringToNative(string input, Encoding encoding, out int length)
		{
			if (input == null)
			{
				length = 0;
				return IntPtr.Zero;
			}
			fixed (char* pInput = input)
			{
				length = encoding.GetByteCount(input);
				IntPtr buffer = Marshal.AllocHGlobal(length + 1);
				byte* pResult = (byte*)buffer.ToPointer();
				int bytesWritten = encoding.GetBytes(pInput, input.Length, pResult, length);
				if (bytesWritten != length) throw new MarshalStringException();
				pResult[length] = 0;
				return buffer;
			}
		}

		internal static void FreeIfNotZero(this IntPtr pointer)
		{
			if (pointer != IntPtr.Zero)
				Marshal.FreeHGlobal(pointer);
		}

		internal static string NativeToString(IntPtr pStringUtf8)
		{
			return NativeToString(pStringUtf8, Encoding.UTF8);
		}
		
		internal static string NativeToString(byte* pString)
		{
			return NativeToString(pString, Encoding.UTF8);
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
