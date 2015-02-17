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
		public String Result{ get; set; }
	}
}

