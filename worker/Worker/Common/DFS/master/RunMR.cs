using System;
using ServiceStack;

namespace Master
{
	[Route("/MR/run")]
	public class RunMR:IReturn<RunMRResponse>
	{
		public byte[] fileWithDll{ get; set; }

		public string fileNameIn{ get; set; }

		public string fileNameOut{ get; set; }
	}

	public class RunMRResponse
	{
		public string Status{ get; set; }
		//Do przerobienia na jakieś Enum
	}
}

