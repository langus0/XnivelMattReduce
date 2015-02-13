using System;
using System.Collections.Generic;
using ServiceStack;
using Common;
namespace Master
{
		[Route("/dfs/ls")]
		public class Ls
		{
		}

		public class LsResponse
		{
			public List<FileHeader> Result { get; set; }
		}
	
}

