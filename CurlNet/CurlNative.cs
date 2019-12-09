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
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, IntPtr value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, Curl value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(IntPtr handle, CurlOption option, WriteFunctionCallback value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_perform")]
		internal static extern CurlCode EasyPerform(IntPtr handle);

		[DllImport(Libcurl, EntryPoint = "curl_easy_strerror")]
		internal static extern IntPtr EasyStrError(CurlCode errornum);

		[DllImport(Libcurl, EntryPoint = "curl_version")]
		internal static extern IntPtr GetVersion();

		[DllImport(Libcurl, EntryPoint = "curl_easy_reset")]
		internal static extern void EasyReset(IntPtr handle);

		[DllImport(Libcurl, EntryPoint = "curl_easy_cleanup")]
		internal static extern void EasyCleanup(IntPtr handle);

		[DllImport(Libcurl, EntryPoint = "curl_global_cleanup")]
		internal static extern void GlobalCleanup();

		internal delegate UIntPtr WriteFunctionCallback(IntPtr buffer, UIntPtr size, UIntPtr nmemb, Curl userdata);
	}
}
