using System;
using System.Runtime.InteropServices;
using Xunit;

namespace CurlNet.Tests
{
	public class StringTests
	{
		[Theory]
		[InlineData(null)]
		public void Utf8NullTest(string text)
		{
			Assert.Equal(IntPtr.Zero, MarshalString.StringToUtf8(text));
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
			IntPtr pointer = MarshalString.StringToUtf8(text);
			try
			{
				Assert.NotEqual(IntPtr.Zero, pointer);
			}
			finally
			{
				if (pointer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pointer);
				}
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
			IntPtr pointer = MarshalString.StringToUtf8(text);
			string marshalled;
			try
			{
				marshalled = MarshalString.Utf8ToString(pointer);
			}
			finally
			{
				if (pointer != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pointer);
				}
			}
			Assert.Equal(text, marshalled);
		}
	}
}
