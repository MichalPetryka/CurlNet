namespace CurlNet.Memory
{
	internal interface IMemoryHolder
	{
		NativeMemory Get();
		void Free(NativeMemory memory);
		void Clear();
	}
}
