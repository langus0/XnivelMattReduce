using System;
using System.Linq;
using Common;
using System.Collections.Generic;
using ServiceStack.Logging;
using System.Threading;
using System.Reflection;
using System.IO;

namespace Worker
{
	public class MapperRunner
	{
		private static ILog log = LogManager.GetLogger (typeof(MapperRunner));

		public static void clear (){
		
		}

		public static void map ()
		{
			String dllPath = Path.Combine (MapReduceUtils.GetWorkingDirectory (), MapReduceUtils.USERDLL_NAME);

			log.Debug ("Dll in  "+dllPath);

			Assembly assembly = Assembly.LoadFrom(dllPath);

			if (assembly == null) {
				throw new Exception ("Assembly is empty!");
			}

//			AppDomain.CurrentDomain.Load(assembly.GetName());
			Type mapClass = assembly.GetTypes().Where(t=>typeof( ApiMaperReducer.ApiMapper).IsAssignableFrom(t)).First();

			if (mapClass == null) {
				throw new Exception ("Mapper class not found!");
			}

			ApiMaperReducer.ApiMapper mapper = (ApiMaperReducer.ApiMapper)Activator.CreateInstance (mapClass);
			mapper.setListOfNodes (StatusConfigContainer.reducersIps );

			foreach (int chunk in StatusConfigContainer.chunksToProcess) {
				log.InfoFormat ("Mapping chunk {0}...", chunk);
				// Read the file and display it line by line.
				mapper.chunk = chunk;
				mapper.init (chunk);
				Chunk readedChunk = DfsWorkerUtils.readChunk (StatusConfigContainer.fileNameIn, chunk);

				foreach(string line in readedChunk.data)
				{
					log.DebugFormat ("Reading line: {0}" , line);
					mapper.map (line);
				}
				mapper.endWork ();
			}
		
			log.Info ("Mapper ended its work!");
			StatusConfigContainer.Status = StatusType.WAITING_FOR_REDUCE;


		}

		public static void startMapper()
		{
			log.Info ("Running mapper...");
			Thread Tmapper = new Thread (new ThreadStart (map));
			Tmapper.Start ();
			StatusConfigContainer.Status = StatusType.MAPPER;
		}
	}


}

