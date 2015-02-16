using System.Collections.Generic;
using ServiceStack;
using Common;

namespace Master
{
	[Route("/dfs/saveFile", "PUT")]
	public class SaveFile : IReturn<SaveFileResponse>
	{
		public File file{ get; set; }
		public int numOfReplicas{ get; set; }
	}

	public class SaveFileResponse
	{
		public List<string> inactiveWorkers { get; set; }
	}
}

