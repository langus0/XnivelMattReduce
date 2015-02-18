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
	public static class StatusConfigContainer
	{

		private static ILog log = LogManager.GetLogger (typeof(StatusConfigContainer));

		public static Common.StatusType Status{ get; set; }

		public static string myIP{ get ; set; }

		public static List<TaskAssigment> assigments;

		public static string fileNameIn{ get; set; }

		public static string fileNameOut{ get; set; }

		public static string[] reducersIps {
			get {
				return (from assigment in assigments
				        orderby assigment.workerId
				        select assigment.workerIP).ToArray ();
			}
		}

		public static List<int> chunksToProcess { get ; set; }

		public static int totalNumberOfChunks{ get {
				return (from assigment in assigments
				        select assigment.listOfChunksToProcess.Count).Sum ();
			} }


		public static Thread Tmapper;

		public static void mapperFunction ()
		{
			String dllPath = Path.Combine (MapReduceUtils.GetWorkingDirectory (), MapReduceUtils.USERDLL_NAME);

			System.Console.WriteLine ("dllpath "+dllPath);

			Assembly assembly = Assembly.LoadFrom(dllPath);

			if (assembly == null) {
				System.Console.WriteLine ("puste assembly");
			}

			AppDomain.CurrentDomain.Load(assembly.GetName());
			Type mapClass = assembly.GetType("ExampleMapper.Mapper");

			if (mapClass == null) {
				System.Console.WriteLine ("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			}

			ApiMaperReducer.ApiMapper mapper = (ApiMaperReducer.ApiMapper)Activator.CreateInstance (mapClass);

			if (mapper == null) {
				System.Console.WriteLine ("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
			}

			mapper.setListOfNodes (reducersIps );

			/*
			var propperMapper = Activator.CreateInstance(t);
			var methodSetIP = t.GetMethod("setListOfNodes");

			methodSetIP.Invoke (propperMapper,new object[]{listOfNodes});
			*/

			foreach (int chunk in chunksToProcess) {
				//String filePath = Path.Combine (MapReduceUtils.GetWorkingDirectory (), chunk.ToString() + DfsUtils.CHUNKID_SEPARATOR + fileNameIn);

				// Read the file and display it line by line.

				Chunk readedChunk = DfsWorkerUtils.readChunk (fileNameIn, chunk);

				foreach(string line in readedChunk.data)
				{
					System.Console.WriteLine ("$$ " + line);
					//var methodRun = t.GetMethod("run");
					//methodRun.Invoke (propperMapper,new object[]{line});
					mapper.map (line);
				}



			}
			/*
			//wyslij koniec;
			var methodEndWork = t.GetMethod("endWork");
			methodEndWork.Invoke (propperMapper,null);
			*/
			//mapper.endWork ();
			Status = StatusType.WAITING_FOR_REDUCE;
		}

		public static void startWork ()
		{
			Tmapper = new Thread (new ThreadStart (mapperFunction));
			Tmapper.Start ();
			Status = StatusType.MAPPER;
		}
		/*To samo zwraca reducersIPs
		public static void createlistOfNodes (List<TaskAssigment> assigments)
		{
			listOfNodes = (from assigment in assigments
			 select assigment.workerIP).ToList ();
		}
*/
		/*
		 * //Assigment jest per worker więc assigmentów jest tyle ile assigmentów!
		public static void countNumberOfNodes (List<TaskAssigment> assigments)
		{

			numberOfNodes = (from assigment in assigments
			                   group assigment by assigment.workerIP into g
			                   select  g
			).Count ();
		}


		public static int workerId {
			get {
				if (Status == StatusType.IDLE)
					return -1;
				return (from assigment in assigments
					where assigment.workerIP == myIP
				    select assigment.workerId).First ();
			}
		}

		public static void init (string ip)
		{
			myIP = MRUtil.normalizeIP (ip);
			reset ();
		}

		public static void reset ()
		{
			Status = StatusType.IDLE;
			fileNameIn = null;
			fileNameOut = null;
			assigments = null;
		}

		public static void prepare (string fileNameIn, string fileNameOut, List<TaskAssigment> assigments)
		{
			StatusConfigContainer.fileNameIn = fileNameIn;
			StatusConfigContainer.fileNameOut = fileNameOut;
			StatusConfigContainer.assigments = assigments;

			chunksToProcess = (from assigment in assigments
			                 where assigment.workerIP == myIP
			                 select assigment.listOfChunksToProcess).First ();
			Status = StatusType.PREPARED;
		}
	}
}

