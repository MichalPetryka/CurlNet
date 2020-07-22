using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace CurlNet.Exceptions
{
	public class MarshalStringException : Exception
	{
		internal const string MarshalStringMessage = "String UTF-8 encoding failed!";
		internal MarshalStringException() : base(MarshalStringMessage)
		{

		}
	}
}
