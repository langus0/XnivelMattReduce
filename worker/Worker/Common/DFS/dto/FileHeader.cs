using System;
using System.Collections.Generic;
using Worker;

namespace Common
{
	public class FileHeader
	{
		public String fileName{ get; set; }

		private List<ChunkHeader> chunksHeaders{ get; set; }

		public bool corrupted {
			get {
				if (chunksHeaders.Count == 0) {
					return true;
				}

				HashSet<int> idsOfChunks = getUniqChunkIds ();
				int max = int.MinValue;
				foreach (var chunkId in idsOfChunks) {
					max = Math.Max (max, chunkId);
				}
				return idsOfChunks.Count != max;
			}
		}


		public long sizeInBytes {
			get {
				long sum = 0;
				foreach (var chunk in chunksHeaders) {
					sum += chunk.sizeInBytes;
				}
				return sum;
			}
		}

		private HashSet<int> getUniqChunkIds(){
			HashSet<int> idsOfChunks = new HashSet<int> ();
			foreach (var chunk in chunksHeaders) {
				idsOfChunks.Add (chunk.chunkId);
				if (chunk.chunkId < 0) {
					throw new ArgumentException("Chunk has negative id!");
				}
			}
			return idsOfChunks;
		}
	}
}

