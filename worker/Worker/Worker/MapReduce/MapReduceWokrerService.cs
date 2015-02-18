using System;
using ServiceStack;
using ServiceStack.Logging;

namespace Worker
{
	public class MapReduceWokrerService:Service
	{
		private static ILog log = LogManager.GetLogger (typeof(MapReduceWokrerService));
		public object Any (PrepareMRTask request)
		{
			log.Info ("Prepare MapReduce... ");
			MapReduceUtils.clearWorkingDirectory ();
			MapReduceUtils.saveDll (request.fileWithDll);
			StatusConfigContainer.reset ();
			StatusConfigContainer.prepare (request.fileNameIn, request.fileNameOut, request.taskAssigments);
			return new PrepareMRTaskResponse ();
		}

		public object Any (RunMRTask request)
		{
			log.Info ("Run MapReduce... ");
			MapperRunner.startMapper ();
			return new RunMRTaskResponse ();
		}

		public object Put (SendMappedData request)
		{
			log.DebugFormat ("Recived new data from mappers <{0}, {1}>", request.key, request.value);
			ReducerRunner.saveMapResult (request);
			return new SendMappedDataResponse ();
		}

		public object Put (SendMapperEndWork request)
		{
			log.DebugFormat("Mappers ended work on chunk {0}",request.chunk);
			ReducerRunner.newEndMapper (request.chunk);
			if (ReducerRunner.recivedFromAllEndMapper ()) {
				ReducerRunner.runReduce ();
			}
			return new SendMapperEndWorkResponse ();
		}

	}
}

