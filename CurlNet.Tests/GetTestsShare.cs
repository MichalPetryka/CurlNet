using CurlNet.Enums;
using System;
using Xunit;
using Xunit.Abstractions;

namespace CurlNet.Tests
{
	[Collection("Initialize")]
	public class GetTestsShare : GetTestsAbstract, IDisposable
	{
		public GetTestsShare(ITestOutputHelper output)
		{
			Output = output;
			Curl.Initialize(true, SharedData.Cookie | SharedData.Dns | SharedData.SslSession | SharedData.Connection/* | SharedData.Psl*/); // supplied dll doesn't support psl
		}

		public override void ShareTest()
		{
			Assert.NotEqual(CurlNative.ShareHandle.Null, CurlShare.Handle);
			Assert.NotEqual(SharedData.None, CurlShare.Flags);
			// ReSharper disable once HeapView.BoxingAllocation
			Output.WriteLine(CurlShare.Flags.ToString());
		}

		public void Dispose()
		{
			Curl.Deinitialize();
		}
	}
}
