using System;
using System.IO;
using System.Text;
using Xunit;

namespace CurlNet.Tests
{
	[Collection("Initialize")]
	public class GetTestsPooled : IDisposable
	{
		public GetTestsPooled()
		{
			Curl.Initialize(true);
		}

		public void Dispose()
		{
			Curl.Deinitialize();
		}

		[Theory]
		[InlineData("TestFiles/Windows1250.txt")]
		public void InvalidEncodingTest(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(false, true);
			Assert.Throws<DecoderFallbackException>(() =>
			{
				using (Curl curl = new Curl())
				{
					curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding);
				}
			});
		}

		[Theory]
		[InlineData("TestFiles/Utf8Bom.txt", "TestFiles/Ucs2LeBom.txt")]
		public void ResetTest(string path1, string path2)
		{
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				Assert.Equal(File.ReadAllText(path1), curl.GetText(new Uri(Path.GetFullPath(path1)).AbsoluteUri));
				curl.Reset();
				Assert.Equal(File.ReadAllText(path2), curl.GetText(new Uri(Path.GetFullPath(path2)).AbsoluteUri));
			}
		}

		[Theory]
		[InlineData("TestFiles/Utf8.txt")]
		public void Utf8Test(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(false, true);
			using (Curl curl = new Curl())
			{
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
			}
		}

		[Theory]
		[InlineData("TestFiles/Utf8Bom.txt")]
		public void Utf8BomTest(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(true, true);
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
			}
		}

		[Theory]
		[InlineData("TestFiles/Ucs2BeBom.txt")]
		public void Ucs2BeBomTest(string path)
		{
			UnicodeEncoding encoding = new UnicodeEncoding(true, true, true);
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
			}
		}

		[Theory]
		[InlineData("TestFiles/Ucs2LeBom.txt")]
		public void Ucs2LeBomTest(string path)
		{
			UnicodeEncoding encoding = new UnicodeEncoding(false, true, true);
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetText(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
			}
		}
	}
}
