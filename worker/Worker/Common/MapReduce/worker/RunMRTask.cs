using System;
using ServiceStack;

namespace Worker
{
	[Route("/MR/runTask")]
	public class RunMRTask:IReturn<RunMRTaskResponse>
	{
	}

	public class RunMRTaskResponse
	{
	}
}

