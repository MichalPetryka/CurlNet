using System;
// ReSharper disable UnusedParameter.Local

namespace CurlNet
{
	internal class MonoPInvokeCallbackAttribute : Attribute
	{
#pragma warning disable IDE0060 // Usuń nieużywany parametr
		public MonoPInvokeCallbackAttribute(Type type) { }
#pragma warning restore IDE0060 // Usuń nieużywany parametr
	}
}
