namespace CurlNet.Enums.Share
{
	internal enum CurlShareOption
	{
		None,  /* don't use */
		Share,   /* specify a data type to share */
		Unshare, /* specify which data type to stop sharing */
		LockFunc,   /* pass in a 'curl_lock_function' pointer */
		UnlockFunc, /* pass in a 'curl_unlock_function' pointer */
		UserdData,   /* pass in a user data pointer used in the lock/unlock
						   callback functions */
		Last  /* never use */
	}
}
