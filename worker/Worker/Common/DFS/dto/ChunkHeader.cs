using System;
using System.IO;

namespace Common
{
	public class ChunkHeader
	{
		public String fileName{ get; set; }

		public int chunkId{ get; set; }

		public long sizeInBytes{ get; set; }

		public ChunkHeader (FileInfo fileInfo)
		{
			var tuple = DfsUtils.splitFileName (fileInfo.Name);
			chunkId = tuple.Item2;
			fileName = tuple.Item1;
			sizeInBytes = fileInfo.Length;
		}

		public ChunkHeader ()
		{
		}
	}
}

