using System;
using System.Collections.Generic;
using ServiceStack;

namespace Worker
{
	[Route("/MR/runTask")]
	public class RunMRTask:IReturn<RunMRResponse>{
		public byte[] fileWithDll{ get; set; }
		public string fileNameIn{get;set;}
		public string fileNameOut{get;set;}
		public List<TaskAssigment> taskAssigments{ get; set;}


	}

	public class RunMRTaskResponse{
		public string Status{get;set;}//Do przerobienia na jakieś Enum
	}
}

