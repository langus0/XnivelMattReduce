using System;
using System.Collections.Generic;
using ServiceStack;

namespace Master
{
	[Route("/getStatus")]
	public class GetStatus:IReturn<GetStatusResponse>
	{
	}

	public class GetStatusResponse
	{
		public List<Worker.GetStatusResponse> statuses{ get; set; }

		public List<string> inactiveWorkers{ get; set; }
	}
}

