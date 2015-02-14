using System;
using System.Collections.Generic;
using System.Linq;
using Worker;

namespace Common
{
	public class FileHeader
	{
		public String fileName{ get; set; }

		private HashSet<ChunksGroup> chunks{ get; set; }

		public int accessibleReplicas{ get {
				return 	(from chunk in chunks 
						select chunk.Replicas).Min ();

			} }

		public int chunksCount {
			get {
				var max = (from chunk in chunks 
				             select chunk.chunkId).Max ();
				//+1 beacuse chunks are numerated from 0
				return max + 1;
			}
		}

		public bool corrupted {
			get {
				if (chunks.Count == 0) {
					return true;
				}
				return chunks.Count != chunksCount;
			}
		}

		public long sizeInBytes {
			get {
				long sum = 0;
				foreach (var chunk in chunks) {
					sum += chunk.sizeInBytes;
				}
				return sum;
			}
		}

		public FileHeader (ChunksGroup chunk)
		{
			fileName = chunk.fileName;
			chunks = new HashSet<ChunksGroup> ();
			chunks.Add (chunk);
		}

		public List<ChunksGroup> getChunksSorted ()
		{
			var chunksOrdered = from chunk in chunks 
						  orderby chunk.chunkId
			              select chunk;
			return chunksOrdered.ToList();
		}

		public void addChunk (ChunksGroup chunk)
		{
			if (fileName != chunk.fileName) {
				throw new ArgumentException ("Intent to add chunk to diffrent file");
			}
			if (chunk.chunkId < 0) {
				throw new ArgumentException ("Chunk has negative id!");
			}
			if (chunks.Contains (chunk)) {
				foreach (var c in chunks) {
					if (c.Equals (chunk)) {
						c.merge (chunk);
					}
				}
			} else {
				chunks.Add (chunk);
			}

		}
	}
}

