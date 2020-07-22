using System;
using System.IO;
using System.Text;
using CurlNet.Enums;
using Xunit;
using Xunit.Abstractions;

namespace CurlNet.Tests
{
	[Collection("Initialize")]
	public abstract class GetTestsAbstract
	{
		protected ITestOutputHelper Output;

		[Fact]
		public virtual void ShareTest()
		{
			Assert.Equal(CurlNative.ShareHandle.Null, CurlShare.Handle);
			Assert.Equal(SharedData.None, CurlShare.Flags);
		}

		[Theory]
		[InlineData("TestFiles/Utf8.txt")]
		[InlineData("TestFiles/LargeUtf8Bom.txt")]
		public void ProgressEventTest(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(false, true);
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				int progress = 0;

				void OnCurlOnDownloadProgressChange(Curl sender, Curl.Progress curprogress)
				{
					Output.WriteLine(curprogress.ToString());
					// ReSharper disable HeapView.BoxingAllocation
					Assert.True(progress < curprogress.Done, $"{progress} >= {curprogress.Done}");
					// ReSharper restore HeapView.BoxingAllocation
					progress = curprogress.Done;
				}

				curl.DownloadProgressChange += OnCurlOnDownloadProgressChange;
				Assert.Equal(File.ReadAllText(path, encoding), curl.GetString(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding));
				Assert.NotEqual(0, progress);
			}
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
					curl.GetString(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding);
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
				string s1 = curl.GetString(new Uri(Path.GetFullPath(path1)).AbsoluteUri);
				Output.WriteLine(s1);
				Assert.Equal(File.ReadAllText(path1), s1);
				curl.Reset();
				string s2 = curl.GetString(new Uri(Path.GetFullPath(path2)).AbsoluteUri);
				Output.WriteLine(s2);
				Assert.Equal(File.ReadAllText(path2), s2);
			}
		}

		[Theory]
		[InlineData("TestFiles/Utf8.txt")]
		public void Utf8Test(string path)
		{
			UTF8Encoding encoding = new UTF8Encoding(false, true);
			using (Curl curl = new Curl())
			{
				string s = curl.GetString(new Uri(Path.GetFullPath(path)).AbsoluteUri, encoding);
				Output.WriteLine(s);
				Assert.Equal(File.ReadAllText(path, encoding), s);
			}
		}

		[Theory]
		[InlineData("TestFiles/Utf8.txt")]
		[InlineData("TestFiles/LargeUtf8Bom.txt")]
		[InlineData("TestFiles/Utf8Bom.txt")]
		[InlineData("TestFiles/Ucs2BeBom.txt")]
		[InlineData("TestFiles/Ucs2LeBom.txt")]
		public void BomTest(string path)
		{
			using (Curl curl = new Curl())
			{
				curl.UseBom = true;
				string s = curl.GetString(new Uri(Path.GetFullPath(path)).AbsoluteUri);
				Output.WriteLine(s);
				Assert.Equal(File.ReadAllText(path), s);
			}
		}
	}
}
