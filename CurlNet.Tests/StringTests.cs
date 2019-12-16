using System;
using Xunit;

namespace CurlNet.Tests
{
	public class StringTests
	{
		[Theory]
		[InlineData(null)]
		public void Utf8NullTest(string text)
		{
			Assert.Equal(IntPtr.Zero, MarshalString.StringToNative(text));
		}

		[Theory]
		[InlineData("")]
		[InlineData("basic ansi test")]
		[InlineData("CaSe_ĆćĘę-test")]
		[InlineData("0123456789")]
		[InlineData("ąćęłńóśźż")]
		[InlineData("中文")]
		[InlineData("𠜎")]
		[InlineData("�")]
		[InlineData("$¢€𐍈")]
		public void Utf8NotNullTest(string text)
		{
			IntPtr pointer = IntPtr.Zero;
			try
			{
				pointer = MarshalString.StringToNative(text);
				Assert.NotEqual(IntPtr.Zero, pointer);
			}
			finally
			{
				pointer.FreeIfNotZero();
			}
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("basic ansi test")]
		[InlineData("CaSe_ĆćĘę-test")]
		[InlineData("0123456789")]
		[InlineData("ąćęłńóśźż")]
		[InlineData("中文")]
		[InlineData("𠜎")]
		[InlineData("�")]
		[InlineData("$¢€𐍈")]
		public void Utf8MarshalTest(string text)
		{
			IntPtr pointer = IntPtr.Zero;
			try
			{
				pointer = MarshalString.StringToNative(text);
				Assert.Equal(text, MarshalString.NativeToString(pointer));
			}
			finally
			{
				pointer.FreeIfNotZero();
			}
		}
	}
}
