using System;

namespace CurlNet.Enums
{
	[Flags]
	public enum SharedData
	{
		None = 0,
		Cookie = 1,
		Dns = 2,
		SslSession = 4,
		Connection = 8,
		Psl = 16
	}

	internal static class SharedDataExtensions
	{
		public static bool HasFlagFast(this SharedData value, SharedData flag)
		{
			return (value & flag) == flag;
		}
	}
}
