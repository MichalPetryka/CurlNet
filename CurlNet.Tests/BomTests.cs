using System;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace CurlNet.Tests
{
	public class BomTests
	{
		private readonly ITestOutputHelper _output;
		public BomTests(ITestOutputHelper output) => _output = output;

		[Theory]
		[InlineData("TestFiles/Utf8.txt")]
		public void Utf8Test(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(false, true);
			Assert.Equal(BomUtil.GetEncoding(new ArraySegment<byte>(File.ReadAllBytes(path)), encoding, out _).CodePage, encoding.CodePage);
		}

		[Theory]
		[InlineData("TestFiles/Utf8Bom.txt")]
		public void Utf8BomTest(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(true, true);
			UnicodeEncoding badencoding = new UnicodeEncoding(true, true, true);
			Assert.Equal(BomUtil.GetEncoding(new ArraySegment<byte>(File.ReadAllBytes(path)), encoding, out _).CodePage, encoding.CodePage);
			Assert.NotEqual(BomUtil.GetEncoding(new ArraySegment<byte>(File.ReadAllBytes(path)), badencoding, out _).CodePage, badencoding.CodePage);
		}

		[Theory]
		[InlineData("TestFiles/Ucs2BeBom.txt")]
		public void Ucs2BeBomTest(string path)
		{
			UnicodeEncoding encoding = new UnicodeEncoding(true, true, true);
			UTF8Encoding badencoding = new UTF8Encoding(false, true);
			Assert.Equal(BomUtil.GetEncoding(new ArraySegment<byte>(File.ReadAllBytes(path)), encoding, out _).CodePage, encoding.CodePage);
			Assert.NotEqual(BomUtil.GetEncoding(new ArraySegment<byte>(File.ReadAllBytes(path)), badencoding, out _).CodePage, badencoding.CodePage);
		}

		[Theory]
		[InlineData("TestFiles/Ucs2LeBom.txt")]
		public void Ucs2LeBomTest(string path)
		{
			UnicodeEncoding encoding = new UnicodeEncoding(false, true, true);
			UTF8Encoding badencoding = new UTF8Encoding(false, true);
			Assert.Equal(BomUtil.GetEncoding(new ArraySegment<byte>(File.ReadAllBytes(path)), encoding, out _).CodePage, encoding.CodePage);
			Assert.NotEqual(BomUtil.GetEncoding(new ArraySegment<byte>(File.ReadAllBytes(path)), badencoding, out _).CodePage, badencoding.CodePage);
		}

		[Theory]
		[InlineData("TestFiles/Utf8.txt")]
		[InlineData("TestFiles/LargeUtf8Bom.txt")]
		[InlineData("TestFiles/Utf8Bom.txt")]
		[InlineData("TestFiles/Ucs2BeBom.txt")]
		[InlineData("TestFiles/Ucs2LeBom.txt")]
		public void SameBomTest(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(false, true);
			byte[] bytes = File.ReadAllBytes(path);
			Encoding detected = BomUtil.GetEncoding(new ArraySegment<byte>(bytes), encoding, out int offset);
			ReadOnlySpan<byte> preamble = detected.Preamble;

			string bom = "";
			for (int i = 0; i < preamble.Length; i++)
				bom += " " + preamble[i].ToString("X2");
			_output.WriteLine(bom.Trim());

			Assert.Equal(preamble.Length, offset);
			for (int i = 0; i < offset; i++)
			{
				Assert.Equal(preamble[i], bytes[i]);
			}
		}
	}
}
