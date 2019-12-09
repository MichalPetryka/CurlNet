using CurlNet.Enums;
using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace CurlNet.Exceptions
{
	public class CurlException : Exception
	{
		public CurlCode Code { get; }
		public Curl Instance { get; }

		public CurlException(CurlCode code, Curl instance) : base(instance.GetError(code))
		{
			Code = code;
			Instance = instance;
		}
	}
}
