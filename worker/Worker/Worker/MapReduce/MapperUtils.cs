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
	public class MapperUtils
	{
		private static ILog log = LogManager.GetLogger (typeof(MapperUtils));

		public static void clear (){
		
		}

		public static void mapperFunction ()
		{
			String dllPath = Path.Combine (MapReduceUtils.GetWorkingDirectory (), MapReduceUtils.USERDLL_NAME);

			System.Console.WriteLine ("dllpath "+dllPath);

			Assembly assembly = Assembly.LoadFrom(dllPath);

			if (assembly == null) {
				System.Console.WriteLine ("puste assembly");
				throw new Exception ("Assembly is empty!");
			}

			AppDomain.CurrentDomain.Load(assembly.GetName());
			//Type mapClass = assembly.GetType("ExampleMapper.Mapper");
			Type mapClass = assembly.GetTypes().Where(t=>typeof( ApiMaperReducer.ApiMapper).IsAssignableFrom(t)).First();

			if (mapClass == null) {

				System.Console.WriteLine ("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				throw new Exception ("Mapper class not found!");
			}

			ApiMaperReducer.ApiMapper mapper = (ApiMaperReducer.ApiMapper)Activator.CreateInstance (mapClass);

			if (mapper == null) {
				System.Console.WriteLine ("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
			}

			mapper.setListOfNodes (StatusConfigContainer.reducersIps );

			/*
			var propperMapper = Activator.CreateInstance(t);
			var methodSetIP = t.GetMethod("setListOfNodes");

			methodSetIP.Invoke (propperMapper,new object[]{listOfNodes});
			*/

			foreach (int chunk in StatusConfigContainer.chunksToProcess) {
				//String filePath = Path.Combine (MapReduceUtils.GetWorkingDirectory (), chunk.ToString() + DfsUtils.CHUNKID_SEPARATOR + fileNameIn);

				// Read the file and display it line by line.
				mapper.chunk = chunk;
				Chunk readedChunk = DfsWorkerUtils.readChunk (StatusConfigContainer.fileNameIn, chunk);

				foreach(string line in readedChunk.data)
				{
					System.Console.WriteLine ("$$ " + line);
					//var methodRun = t.GetMethod("run");
					//methodRun.Invoke (propperMapper,new object[]{line});
					mapper.map (line);
				}


				mapper.endWork ();
			}

			//wyslij koniec;
			//var methodEndWork = t.GetMethod("endWork");
			//methodEndWork.Invoke (propperMapper,null);
			StatusConfigContainer.Status = StatusType.WAITING_FOR_REDUCE;


		}

		public static void startWork ()
		{
			Thread Tmapper = new Thread (new ThreadStart (mapperFunction));
			Tmapper.Start ();
			StatusConfigContainer.Status = StatusType.MAPPER;
		}
	}
}

