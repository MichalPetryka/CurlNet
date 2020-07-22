using CurlNet.Enums;
using CurlNet.Enums.Share;
using System;
using System.Collections.Generic;
using System.Threading;
using CurlNet.Exceptions;

namespace CurlNet
{
	internal static class CurlShare
	{
		public static CurlNative.ShareHandle Handle;
		public static SharedData Flags;

		private static readonly object LocksLock = new object();
		private static readonly Dictionary<CurlLockData, object> Locks = new Dictionary<CurlLockData, object>();

		private static readonly CurlNative.LockFunctionCallback LockFunction = Lock;
		private static readonly CurlNative.UnlockFunctionCallback UnlockFunction = Unlock;

		public static void Initialize(SharedData data)
		{
			Flags = data;
			if (data == SharedData.None)
			{
				Handle = CurlNative.ShareHandle.Null;
				return;
			}

			Handle = CurlNative.ShareInit();
			if (Handle == CurlNative.ShareHandle.Null)
			{
				return;
			}

			ThrowIfNotOk(Handle.ShareSetOpt(CurlShareOption.LockFunc, LockFunction));
			ThrowIfNotOk(Handle.ShareSetOpt(CurlShareOption.UnlockFunc, UnlockFunction));

			if (data.HasFlagFast(SharedData.Cookie))
				ThrowIfNotOk(Handle.ShareSetOpt(CurlShareOption.Share, CurlLockData.Cookie));

			if (data.HasFlagFast(SharedData.Dns))
				ThrowIfNotOk(Handle.ShareSetOpt(CurlShareOption.Share, CurlLockData.Dns));

			if (data.HasFlagFast(SharedData.SslSession))
				ThrowIfNotOk(Handle.ShareSetOpt(CurlShareOption.Share, CurlLockData.SslSession));

			if (data.HasFlagFast(SharedData.Connection))
				ThrowIfNotOk(Handle.ShareSetOpt(CurlShareOption.Share, CurlLockData.Connect));

			if (data.HasFlagFast(SharedData.Psl))
				ThrowIfNotOk(Handle.ShareSetOpt(CurlShareOption.Share, CurlLockData.Psl));
		}

		public static void Close()
		{
			if (Handle != CurlNative.ShareHandle.Null)
			{
				Handle.ShareCleanup();
			}
		}

		private static void Lock(CurlNative.CurlHandle handle, CurlLockData size, CurlLockAccess nmemb, IntPtr userdata)
		{
			object obj;
			lock (LocksLock)
			{
				if (!Locks.TryGetValue(size, out obj))
				{
					obj = Locks[size] = new object();
				}
			}

			Monitor.Enter(obj);
		}

		private static void Unlock(CurlNative.CurlHandle handle, CurlLockData size, IntPtr userdata)
		{
			object obj;
			lock (LocksLock)
			{
				obj = Locks[size];
			}

			Monitor.Exit(obj);
		}

		internal static void ThrowIfNotOk(CurlShareCode code)
		{
			if (code != CurlShareCode.Ok)
			{
				throw new CurlShareOptionException(code);
			}
		}
	}
}
