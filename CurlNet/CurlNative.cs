using CurlNet.Enums;
using System;
using System.Runtime.InteropServices;

namespace CurlNet
{
	internal static class CurlNative
	{
		private const string Libcurl = "Native/libcurl";

		[DllImport(Libcurl, EntryPoint = "curl_global_init")]
		internal static extern CurlCode GlobalInit(CurlGlobal flags);

		[DllImport(Libcurl, EntryPoint = "curl_easy_init")]
		internal static extern IntPtr EasyInit();

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, int value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, IpResolveMode value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, IntPtr value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, Curl value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, WriteFunctionCallback value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, ProgressFunctionCallback value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_perform")]
		internal static extern CurlCode EasyPerform(IntPtr handle);

		[DllImport(Libcurl, EntryPoint = "curl_easy_strerror")]
		private static extern IntPtr EasyStrError(CurlCode errornum);

		[DllImport(Libcurl, EntryPoint = "curl_version")]
		private static extern IntPtr GetCurlVersion();

		[DllImport(Libcurl, EntryPoint = "curl_easy_reset")]
		internal static extern void EasyReset(IntPtr handle);

		[DllImport(Libcurl, EntryPoint = "curl_easy_cleanup")]
		internal static extern void EasyCleanup(IntPtr handle);

		[DllImport(Libcurl, EntryPoint = "curl_global_cleanup")]
		internal static extern void GlobalCleanup();

		internal delegate UIntPtr WriteFunctionCallback(IntPtr buffer, UIntPtr size, UIntPtr nmemb, Curl userdata);
		internal delegate short ProgressFunctionCallback(Curl clientp, IntPtr dltotal, IntPtr dlnow, IntPtr ultotal, IntPtr ulnow);

		internal static CurlCode EasySetOpt(IntPtr handle, CurlOption option, string value)
		{
			IntPtr text = IntPtr.Zero;
			try
			{
				text = MarshalString.StringToUtf8(value);
				return EasySetOpt(handle, option, text);
			}
			finally
			{
				MarshalString.FreeIfNotZero(text);
			}
		}

		internal static string GetVersion()
		{
			return MarshalString.Utf8ToString(GetCurlVersion());
		}

		internal static string GetErrorMessage(CurlCode code)
		{
			return MarshalString.Utf8ToString(EasyStrError(code));
		}
	}
}
