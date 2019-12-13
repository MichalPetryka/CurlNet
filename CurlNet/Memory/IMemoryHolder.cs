namespace CurlNet.Memory
{
	internal interface IMemoryHolder
	{
		NativeMemory GetMemory();
		void FreeMemory(NativeMemory memory);
		void Clear();
	}
}
