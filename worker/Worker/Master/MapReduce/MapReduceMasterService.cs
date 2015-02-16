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
			return new RunMRResponse ();
		}
	}
}

