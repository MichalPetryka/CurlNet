using CurlNet.Enums;
using System;
using System.Runtime.InteropServices;
using CurlNet.Enums.Share;

// ReSharper disable MemberCanBePrivate.Global

namespace CurlNet
{
	internal static class CurlNative
	{
		private const string Libcurl = "libcurl";

		[DllImport(Libcurl, EntryPoint = "curl_global_init")]
		internal static extern CurlCode GlobalInit(CurlGlobal flags);

		[DllImport(Libcurl, EntryPoint = "curl_easy_init")]
		internal static extern CurlHandle EasyInit();

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, int value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, long value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, IpResolveMode value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, ref int value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, IntPtr value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, ShareHandle value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, WriteFunctionCallback value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, ReadFunctionCallback value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_setopt")]
		internal static extern CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, ProgressFunctionCallback value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_getinfo")]
		internal static extern CurlCode EasyGetInfo(this CurlHandle handle, CurlInfo info, out IntPtr value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_getinfo")]
		internal static extern CurlCode EasyGetInfo(this CurlHandle handle, CurlInfo info, out OffT value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_getinfo")]
		internal static extern CurlCode EasyGetInfo(this CurlHandle handle, CurlInfo info, out int value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_getinfo")]
		internal static extern CurlCode EasyGetInfo(this CurlHandle handle, CurlInfo info, out double value);

		[DllImport(Libcurl, EntryPoint = "curl_easy_perform")]
		internal static extern CurlCode EasyPerform(this CurlHandle handle);

		[DllImport(Libcurl, EntryPoint = "curl_easy_strerror")]
		private static extern IntPtr EasyStrError(CurlCode errornum);

		[DllImport(Libcurl, EntryPoint = "curl_version")]
		private static extern IntPtr GetCurlVersion();

		[DllImport(Libcurl, EntryPoint = "curl_easy_reset")]
		internal static extern void EasyReset(this CurlHandle handle);

		[DllImport(Libcurl, EntryPoint = "curl_easy_cleanup")]
		internal static extern void EasyCleanup(this CurlHandle handle);

		[DllImport(Libcurl, EntryPoint = "curl_global_cleanup")]
		internal static extern void GlobalCleanup();

		[DllImport(Libcurl, EntryPoint = "curl_share_init")]
		internal static extern ShareHandle ShareInit();

		[DllImport(Libcurl, EntryPoint = "curl_share_setopt")]
		internal static extern CurlShareCode ShareSetOpt(this ShareHandle handle, CurlShareOption option, CurlLockData data);

		[DllImport(Libcurl, EntryPoint = "curl_share_setopt")]
		internal static extern CurlShareCode ShareSetOpt(this ShareHandle handle, CurlShareOption option, LockFunctionCallback data);

		[DllImport(Libcurl, EntryPoint = "curl_share_setopt")]
		internal static extern CurlShareCode ShareSetOpt(this ShareHandle handle, CurlShareOption option, UnlockFunctionCallback data);

		[DllImport(Libcurl, EntryPoint = "curl_share_cleanup")]
		internal static extern void ShareCleanup(this ShareHandle handle);

		[DllImport(Libcurl, EntryPoint = "curl_share_strerror")]
		private static extern IntPtr ShareStrError(CurlShareCode errornum);

		internal delegate OffT WriteFunctionCallback(IntPtr buffer, OffT size, OffT nmemb, ref int instance);
		internal delegate OffT ReadFunctionCallback(IntPtr buffer, OffT size, OffT nmemb, ref int instance);

		internal delegate int ProgressFunctionCallback(ref int instance, OffT dltotal, OffT dlnow, OffT ultotal, OffT ulnow);

		internal delegate void LockFunctionCallback(CurlHandle handle, CurlLockData size, CurlLockAccess nmemb, IntPtr userData);
		internal delegate void UnlockFunctionCallback(CurlHandle handle, CurlLockData size, IntPtr userData);

		internal static CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, string value)
		{
			IntPtr text = IntPtr.Zero;
			try
			{
				text = MarshalString.StringToNative(value);
				return EasySetOpt(handle, option, text);
			}
			finally
			{
				text.Free();
			}
		}

		internal static CurlCode EasySetOpt(this CurlHandle handle, CurlOption option, bool value)
		{
			return EasySetOpt(handle, option, value ? 1 : 0);
		}

		internal static CurlCode EasyGetInfo(this CurlHandle handle, CurlInfo info, out string value)
		{
			CurlCode result = EasyGetInfo(handle, info, out IntPtr text);
			value = MarshalString.NativeToString(text);
			return result;
		}

		internal static string GetVersion()
		{
			return MarshalString.NativeToString(GetCurlVersion());
		}

		internal static string GetErrorMessage(CurlCode code)
		{
			return MarshalString.NativeToString(EasyStrError(code));
		}

		internal static string GetShareError(CurlShareCode code)
		{
			return MarshalString.NativeToString(ShareStrError(code));
		}

		[StructLayout(LayoutKind.Sequential)]
		internal readonly struct ShareHandle : IEquatable<ShareHandle>
		{
			public static readonly ShareHandle Null = new ShareHandle(IntPtr.Zero);
			private readonly IntPtr _handle;

			private ShareHandle(IntPtr ptr)
			{
				_handle = ptr;
			}

			#region IEquatable
			public bool Equals(ShareHandle other)
			{
				return _handle == other._handle;
			}

			public override bool Equals(object obj)
			{
				return obj is ShareHandle other && Equals(other);
			}

			public override int GetHashCode()
			{
				return _handle.GetHashCode();
			}

			public static bool operator ==(ShareHandle left, ShareHandle right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(ShareHandle left, ShareHandle right)
			{
				return !left.Equals(right);
			}
			#endregion

			public override string ToString()
			{
				return _handle.ToString();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal readonly struct CurlHandle : IEquatable<CurlHandle>
		{
			public static readonly CurlHandle Null = new CurlHandle(IntPtr.Zero);
			private readonly IntPtr _handle;

			private CurlHandle(IntPtr ptr)
			{
				_handle = ptr;
			}

			#region IEquatable
			public bool Equals(CurlHandle other)
			{
				return _handle == other._handle;
			}

			public override bool Equals(object obj)
			{
				return obj is CurlHandle other && Equals(other);
			}

			public override int GetHashCode()
			{
				return _handle.GetHashCode();
			}

			public static bool operator ==(CurlHandle left, CurlHandle right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(CurlHandle left, CurlHandle right)
			{
				return !left.Equals(right);
			}
			#endregion

			public override string ToString()
			{
				return _handle.ToString();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal readonly struct OffT : IEquatable<OffT>
		{
			private readonly UIntPtr _value;

			internal OffT(uint value)
			{
				_value = new UIntPtr(value);
			}

			#region IEquatable
			public bool Equals(OffT other)
			{
				return _value == other._value;
			}

			public override bool Equals(object obj)
			{
				return obj is OffT other && Equals(other);
			}

			public override int GetHashCode()
			{
				return _value.GetHashCode();
			}

			public static bool operator ==(OffT left, OffT right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(OffT left, OffT right)
			{
				return !left.Equals(right);
			}

			public static implicit operator int(OffT value)
			{
				return (int)value._value;
			}
			#endregion

			public override string ToString()
			{
				return _value.ToString();
			}
		}
	}
}
