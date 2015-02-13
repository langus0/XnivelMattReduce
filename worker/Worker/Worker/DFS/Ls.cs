using System;
using System.Collections.Generic;
using ServiceStack;

namespace Worker
{
	[Route("/dfs/ls")]
	public class Ls
	{
	}

	public class LsResponse
	{
		public List<ChunkHeader> Result { get; set; }
	}
}

