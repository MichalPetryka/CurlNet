using System;
using CurlNet.Memory;
using Xunit;
using Xunit.Abstractions;

namespace CurlNet.Tests
{
	[Collection("Initialize")]
	public class GetTestsPooled : GetTestsAbstract, IDisposable
	{
		public GetTestsPooled(ITestOutputHelper output)
		{
			Output = output;
			Curl.Initialize(true);
		}

		[Fact]
		public void PoolTest()
		{
			Assert.Equal(typeof(MemoryPool), Curl.MemoryHolder.GetType());
		}

		public void Dispose()
		{
			Curl.Deinitialize();
		}
	}
}
