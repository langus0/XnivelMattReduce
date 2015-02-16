using System;
using ServiceStack;

namespace Worker
{
	[Route("/dfs/deleteChunk/{FileName}/{chunkId}", "DELETE")]
	public class DeleteChunk :IReturn<DeleteChunkResponse>
	{

		public string FileName{ get; set; }

		public int chunkId{ get; set; }
	}

	public class DeleteChunkResponse{

	}
	
}

