#if !NETSTANDARD1_1
using System;

namespace CurlNet.Exceptions
{
	public class MarshalStringException : Exception
	{
		public MarshalStringException() : base("String UTF-8 encoding failed!")
		{

		}
	}
}
#endif
