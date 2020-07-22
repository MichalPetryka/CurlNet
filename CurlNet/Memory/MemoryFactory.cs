namespace CurlNet.Memory
{
	internal class MemoryFactory : IMemoryHolder
	{
		public NativeMemory Get()
		{
			return new NativeMemory();
		}

		public void Free(NativeMemory memory)
		{
			memory.Dispose();
		}

		public void Clear() { }
	}
}
