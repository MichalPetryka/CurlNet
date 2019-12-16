#if !NETSTANDARD1_1
using System;

namespace CurlNet.Exceptions
{
	public class MarshalStringException : Exception
	{
		private const string MarshalStringMessage = "String UTF-8 encoding failed!";
		public MarshalStringException() : base(MarshalStringMessage)
		{

		}
	}
}
#endif
