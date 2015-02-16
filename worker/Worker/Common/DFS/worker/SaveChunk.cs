using ServiceStack;
using Common;

namespace Worker
{

	[Route("/dfs/savechunk", "PUT")]
	public class SaveChunk :IReturn<SaveChunkResponse>{

		public Chunk chunk{ get; set;}
		//public string FileName { get; set;}
		//public int chunkId{ get; set;}
		//public byte[] data{ get; set;}
	}

	public class SaveChunkResponse{

	}

}

