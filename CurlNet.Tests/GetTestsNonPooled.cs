using System;
using CurlNet.Memory;
using Xunit;
using Xunit.Abstractions;

namespace CurlNet.Tests
{
	[Collection("Initialize")]
	public class GetTestsNonPooled : GetTestsAbstract, IDisposable
	{
		public GetTestsNonPooled(ITestOutputHelper output)
		{
			Output = output;
			Curl.Initialize(false);
		}

		[Fact]
		public void PoolTest()
		{
			Assert.Equal(typeof(MemoryFactory), Curl.MemoryHolder.GetType());
		}

		public void Dispose()
		{
			Curl.Deinitialize();
		}
	}
}
