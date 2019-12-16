using System;

namespace CurlNet.Exceptions
{
	public class CurlNotInitializedException : Exception
	{
		internal const string NotInitializedMessage = "Curl Global not initialized";
		internal CurlNotInitializedException() : base(NotInitializedMessage)
		{

		}
	}
}
