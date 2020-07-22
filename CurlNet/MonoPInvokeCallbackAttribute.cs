using System;
// ReSharper disable All

namespace CurlNet
{
	internal class MonoPInvokeCallbackAttribute : Attribute
	{
		public Type Type;

		public MonoPInvokeCallbackAttribute(Type type)
		{
			Type = type;
		}
	}
}
