using System;
using ServiceStack;

namespace Worker
{
	[Route("/getStatus")]
	public class GetStatus:IReturn<GetStatusResponse>
	{
	}

	public class GetStatusResponse
	{
		public String CPUproc{ get; set; }
		public String MEMproc{ get; set; }
		public String Status{ get; set; }
		public String IP{ get; set; }
	}
}

