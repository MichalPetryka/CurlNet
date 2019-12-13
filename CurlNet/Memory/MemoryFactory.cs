namespace CurlNet.Memory
{
	internal class MemoryFactory : IMemoryHolder
	{
		public NativeMemory GetMemory()
		{
			return new NativeMemory();
		}

		public void FreeMemory(NativeMemory memory)
		{
			memory.Dispose();
		}

		public void Clear() { }
	}
}
