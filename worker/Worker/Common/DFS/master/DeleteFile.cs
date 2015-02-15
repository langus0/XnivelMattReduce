using System.Collections.Generic;
using ServiceStack;
using Common;

namespace Master
{
	[Route("/dfs/deleteFile/{FileName}", "DELETE")]
	public class DeleteFile
	{
		public string FileName{ get; set; }
	}

	public class DeleteFileResponse
	{
		public List<string> inactiveWorkers { get; set; }
	}
}

