using System;
using ServiceStack;

namespace Worker
{
	public class MapReduceWokrerService:Service
	{
		public object Any (PrepareMRTask request)
		{
			System.Console.WriteLine ("reciv PREPAREMRTASK");
			MapReduceUtils.clearWorkingDirectory ();
			MapReduceUtils.saveDll (request.fileWithDll);
			StatusConfigContainer.prepare (request.fileNameIn, request.fileNameOut, request.taskAssigments);
			return new PrepareMRTaskResponse ();
		}

		public object Any (RunMRTask request)
		{
			System.Console.WriteLine ("reciv RUNMRTASK");
			StatusConfigContainer.startWork ();
			return new RunMRTaskResponse ();
		}
	}
}

