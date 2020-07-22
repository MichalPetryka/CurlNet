using CurlNet.Enums.Share;
using System;

namespace CurlNet.Exceptions
{
	public class CurlShareOptionException : Exception
	{
		public CurlShareCode Error { get; }

		// ReSharper disable once HeapView.BoxingAllocation
		internal CurlShareOptionException(CurlShareCode error) : base(CurlNative.GetShareError(error))
		{
			Error = error;
		}
	}
}
