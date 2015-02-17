using System;
using System.Collections.Generic;
using ServiceStack;
using Common;

namespace Worker
{
	[Route("/MR/prepareTask")]
	public class PrepareMRTask:IReturn<PrepareMRTaskResponse>
	{
		public byte[] fileWithDll{ get; set; }

		public string fileNameIn{ get; set; }

		public string fileNameOut{ get; set; }

		public List<TaskAssigment> taskAssigments{ get; set; }
	}

	public class PrepareMRTaskResponse
	{
		public string Status{ get; set; }
		//Do przerobienia na jakieś Enum
	}
}

