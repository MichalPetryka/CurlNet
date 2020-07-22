namespace CurlNet.Enums.Share
{
	internal enum CurlLockAccess
	{
		None = 0,   /* unspecified action */
		Shared = 1, /* for read perhaps */
		Single = 2, /* for write perhaps */
		Last        /* never use */
	}
}
