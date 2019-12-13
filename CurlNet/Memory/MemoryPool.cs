using System.Collections.Generic;

namespace CurlNet.Memory
{
	internal class MemoryPool : IMemoryHolder
	{
		private static readonly Queue<NativeMemory> Pool = new Queue<NativeMemory>();
		private static readonly object LockObject = new object();

		public NativeMemory GetMemory()
		{
			lock (LockObject)
			{
				if (Pool.Count > 0)
				{
					return Pool.Dequeue();
				}
			}
			return new NativeMemory();
		}

		public void FreeMemory(NativeMemory memory)
		{
			CurlNative.EasyReset(memory.Curl);
			lock (LockObject)
			{
				Pool.Enqueue(memory);
			}
		}

		public void Clear()
		{
			lock (LockObject)
			{
				while (Pool.Count > 0)
				{
					Pool.Dequeue().Dispose();
				}

				Pool.Clear();
			}}
	}
}
