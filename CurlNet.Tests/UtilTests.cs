using CurlNet.Enums;
using Xunit;
using Xunit.Abstractions;

namespace CurlNet.Tests
{
	[Collection("Initialize")]
	public class UtilTests
	{
		private readonly ITestOutputHelper _output;
		public UtilTests(ITestOutputHelper output) => _output = output;

		[Theory]
		[InlineData(CurlCode.Ok, "No error")]
		[InlineData(CurlCode.UnknownOption, "An unknown option was passed in to libcurl")]
		[InlineData(CurlCode.Obsolete20, "Unknown error")]
		public void ErrorTest(CurlCode code, string result)
		{
			string message = CurlNative.GetErrorMessage(code);
			Assert.NotNull(message);
			Assert.NotEqual("", message);
			_output.WriteLine(message);
			Assert.Equal(result, message);
		}

		[Fact]
		public void VersionTest()
		{
			string version = Curl.CurlVersion;
			Assert.NotNull(version);
			Assert.NotEqual("", version);
			_output.WriteLine(version);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void InitializeTest(bool pool)
		{
			Assert.True(Curl.Initialize(pool));
			Curl.Deinitialize();
		}
	}
}
