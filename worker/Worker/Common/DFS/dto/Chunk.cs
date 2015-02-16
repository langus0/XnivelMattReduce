using System;
using System.Collections.Generic;
using System.IO;
namespace Common
{
	public class Chunk : ChunkHeader
	{
		//public ChunkHeader header{ get; set; }

		public List<string> data{ get; set; }

		public Chunk ()
		{
		}

		public Chunk (FileInfo fileInfo, List<string> data):base(fileInfo)
		{
			//this.header = new ChunkHeader { fileName = name, chunkId = id};
			this.data = data;
		}
	}
}

