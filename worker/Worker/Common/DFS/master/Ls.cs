using System;
using System.Collections.Generic;
using ServiceStack;
using Common;

namespace Master
{
	//[Authenticate]
	[Route("/dfs/ls")]
	public class Ls : IReturn<LsResponse>
	{
	}

	public class LsResponse
	{
		public List<FileHeader> Result { get; set; }
		public List<string> inactiveWorkers{get;set;}
	}
}

