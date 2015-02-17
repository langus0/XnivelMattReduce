using System;
using ServiceStack;
using ServiceStack.Logging;
using System.IO;
using Common;

namespace Worker
{
	public class Reducer: Service
	{
		private static ILog log = LogManager.GetLogger (typeof(Reducer));

		public object Put (SendMappedData request)
		{
			ReducerUtils.saveMapResult (request);
			return new SendMappedDataResponse ();
		}

		public object Put (SendMapperEndWork request)
		{
			ReducerUtils.newEndMapper (request.chunk);
			if (ReducerUtils.recivedFromAllEndMapper ()) {
				ReducerUtils.runReduce ();
			}
			/*
			if (ReducerUtils.newEndMapper (request.chunk)) {
				//sendtoall
			} else {
				if (ReducerUtils.recivedAllFromOneEndMapper (request.chunk, 1)) {
					//wywal z listy zadan tego mapera i jesli jakis watek na nim dziala to go zabij
				}
			}*/
			return new SendMapperEndWorkResponse ();
		}
	}
}

