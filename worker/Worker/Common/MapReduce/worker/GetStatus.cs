using System;
using ServiceStack;

namespace Worker
{
	[Route("/getStatus")]
	public class GetStatus
	{
	}

	public class GetStatusResponse
	{
		public String CPUproc{ get; set; }
		public String MEMproc{ get; set; }
	}
}

