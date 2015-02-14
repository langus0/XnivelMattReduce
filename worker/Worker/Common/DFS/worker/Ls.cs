using System;
using System.Collections.Generic;
using ServiceStack;
using Common;

namespace Worker
{
	[Route("/dfs/ls")]
	public class Ls: IReturn<LsResponse>
	{
	}

	public class LsResponse
	{
		public List<ChunkHeader> Result { get; set; }
	}
}

