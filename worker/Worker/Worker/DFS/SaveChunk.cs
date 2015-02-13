using ServiceStack;


namespace Worker
{

	[Route("/dfs/savechunk", "PUT")]
	public class SaveChunk{

		public string FileName { get; set;}
		public int chunkId{ get; set;}
		public byte[] data{ get; set;}
	}

	public class SaveChunkResponse
	{
	}
}

