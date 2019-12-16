using System;
// ReSharper disable UnusedParameter.Local
// ReSharper disable NotAccessedField.Global
// ReSharper disable MemberCanBePrivate.Global

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
