using System;

namespace Common
{
	public class Chunk
	{
		public ChunkHeader header{ get; set; }
		public byte[] data{ get; set; }
	}
}

