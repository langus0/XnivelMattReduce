using ServiceStack;
using Common;

namespace Worker
{
	[Route("/dfs/getchunk/{FileName}/{chunkId}", "GET")]
	public class GetChunk: IReturn<GetChunkResponse>
	{

		public string FileName{ get; set; }

		public int chunkId{ get; set; }
	}

	public class GetChunkResponse
	{
		public Chunk Result { get; set; }
	}
}

