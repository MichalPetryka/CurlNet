namespace CurlNet.Enums
{
	internal static class CurlInfoType
	{
		public const uint String = 0x100000;
		public const uint Long = 0x200000;
		public const uint Double = 0x300000;
		public const uint Slist = 0x400000;
		public const uint Pointer = 0x400000;
		public const uint Socket = 0x500000;
		public const uint OffT = 0x600000;
		public const uint Mask = 0x0fffff;
		public const uint Typemask = 0xf00000;
	}
}
