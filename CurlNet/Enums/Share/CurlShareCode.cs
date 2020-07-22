namespace CurlNet.Enums.Share
{
	public enum CurlShareCode
	{
		Ok,  /* all is fine */
		BadOption, /* 1 */
		InUse,     /* 2 */
		Invalid,    /* 3 */
		NoMem,      /* 4 out of memory */
		NotBuiltIn, /* 5 feature not present in lib */
		Last        /* never use */
	}
}
