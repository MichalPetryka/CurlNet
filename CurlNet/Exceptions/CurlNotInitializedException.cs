using System;

namespace CurlNet.Exceptions
{
	public class CurlNotInitializedException : Exception
	{
		public CurlNotInitializedException() : base("Curl Global not initialized")
		{

		}
	}
}
