using System;

namespace CurlNet.Exceptions
{
	public class CurlNotInitializedException : Exception
	{
		internal const string NotInitializedMessage = "Curl Global not initialized";
		public CurlNotInitializedException() : base(NotInitializedMessage)
		{

		}
	}
}
