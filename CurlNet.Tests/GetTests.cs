using System;
using System.IO;
using System.Text;
using Xunit;

namespace CurlNet.Tests
{
	public class GetTests
	{
		[Theory]
		[InlineData("TestFiles/Windows1250.txt")]
		public void InvalidEncodingTest(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(false, true);
			Assert.True(Curl.Initialize());
			Assert.Throws<DecoderFallbackException>(() =>
			{
				using (Curl curl = new Curl())
				{
					curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding);
				}
			});
			Curl.Deinitialize();
		}

		[Theory]
		[InlineData("TestFiles/Utf8.txt")]
		public void Utf8Test(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(false, true);
			Assert.True(Curl.Initialize());
			using (Curl curl = new Curl())
			{
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
			}
			Curl.Deinitialize();
		}

		[Theory]
		[InlineData("TestFiles/Utf8Bom.txt")]
		public void Utf8BomTest(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(true, true);
			Assert.True(Curl.Initialize());
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
			}
			Curl.Deinitialize();
		}

		[Theory]
		[InlineData("TestFiles/Ucs2BeBom.txt")]
		public void Ucs2BeBomTest(string path)
		{
			UnicodeEncoding encoding = new UnicodeEncoding(true, true, true);
			Assert.True(Curl.Initialize());
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
			}
			Curl.Deinitialize();
		}

		[Theory]
		[InlineData("TestFiles/Ucs2LeBom.txt")]
		public void Ucs2LeBomTest(string path)
		{
			UnicodeEncoding encoding = new UnicodeEncoding(false, true, true);
			Assert.True(Curl.Initialize());
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
			}
			Curl.Deinitialize();
		}
	}
}
