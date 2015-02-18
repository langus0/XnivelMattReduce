using System;
using ServiceStack;
using Common;
using Worker;

namespace Master
{
	public class MapReduceMasterService:Service
	{
		
		public object Any (RunMR request)
		{
			var taskAssigment = MapReduceUtils.scheduleMRJob (request.fileNameIn, request.fileNameOut);
			MapReduceUtils.runMRJob (request, taskAssigment);
			return new RunMRResponse ();
		}

		public object Any (GetStatus request)
		{
			var result = MapReduceUtils.getStatuses ();
			return new GetStatusResponse {inactiveWorkers =result.Item2, statuses = result.Item1
			};
		}
	}
}

