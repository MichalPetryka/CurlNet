using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Global

namespace CurlNet.Exceptions
{
	public class ThreadStaticOccupiedException : Exception
	{
		private const string ThreadStaticOccupiedMessage = "Thread static curl variable is already occupied!";

		public readonly Curl PresentCurl;

		internal ThreadStaticOccupiedException(Curl curl) : base(ThreadStaticOccupiedMessage)
		{
			PresentCurl = curl;
		}
	}
}
