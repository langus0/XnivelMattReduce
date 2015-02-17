using System;
using ServiceStack;

namespace Worker
{
	public class MapReduceWokrerService:Service
	{
		public object Any (PrepareMRTask request)
		{
			MapReduceUtils.clearWorkingDirectory ();
			MapReduceUtils.saveDll (request.fileWithDll);
			StatusConfigContainer.prepare (request.fileNameIn, request.fileNameOut, request.taskAssigments);
			return new PrepareMRTaskResponse ();
		}

		public object Any (RunMRTask request)
		{
			return new RunMRTaskResponse ();
		}
	}
}

