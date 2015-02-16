using System;
using System.Collections.Generic;

namespace Common
{
	public class ChunksGroup
	{
		public string fileName{ get; set; }
		public int chunkId{ get; set; }
		public long sizeInBytes{ get; set; }
		public HashSet<String> storedInWorkers = new HashSet<string> ();

		public ChunksGroup (ChunkHeader header, string worker)
		{
			fileName = header.fileName;
			chunkId = header.chunkId;
			storedInWorkers.Add (worker);
		}

		public int Replicas {
			get{ return storedInWorkers.Count;}
		}

		public void merge (ChunksGroup chunk)
		{
			if (!chunk.Equals (this)) {
				throw new ArgumentException ("Merging diffrent chunks!");
			}
			if (chunk.sizeInBytes != sizeInBytes) {
				throw new ArgumentException ("Merging chunks error: the same name and id, but diffrent SIZE!");
			}
			storedInWorkers.UnionWith (chunk.storedInWorkers);
		}

		public override bool Equals (System.Object obj)
		{
			if (obj == null) {
				return false;
			}
			ChunksGroup p = obj as ChunksGroup;
			if ((System.Object)p == null) {
				return false;
			}
			return (fileName == p.fileName) && (chunkId == p.chunkId);
		}

		public override int GetHashCode ()
		{
			return (fileName + chunkId.ToString()).GetHashCode ();
		
		}
	}
}

