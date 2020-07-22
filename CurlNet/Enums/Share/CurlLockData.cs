namespace CurlNet.Enums.Share
{
	internal enum CurlLockData
	{
		None = 0,
		/*  CURL_LOCK_DATA_SHARE is used internally to say that
		 *  the locking is just made to change the internal state of the share
		 *  itself.
		 */
		Share,
		Cookie,
		Dns,
		SslSession,
		Connect,
		Psl,
		Last
	}
}
