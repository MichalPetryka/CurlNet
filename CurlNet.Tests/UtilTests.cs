using CurlNet.Enums;
using Xunit;

namespace CurlNet.Tests
{
	[Collection("Initialize")]
	public class UtilTests
	{
		[Theory]
		[InlineData("No error", CurlCode.Ok)]
		[InlineData("An unknown option was passed in to libcurl", CurlCode.UnknownOption)]
		[InlineData("Unknown error", CurlCode.Obsolete20)]
		public void ErrorTest(string result, CurlCode code)
		{
			Assert.Equal(result, CurlNative.GetErrorMessage(code));
		}

		[Fact]
		public void VersionTest()
		{
			string version = Curl.CurlVersion;
			Assert.NotNull(version);
			Assert.False(version == "", "Version is empty");
		}

		[Fact]
		public void InitializeTest()
		{
			Assert.True(Curl.Initialize(true));
			Curl.Deinitialize();
		}
	}
}
