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

		public static List<int> chunksToProcess { get ; set; }

		public static int numberOfNodes { get ; set; }
		public static List<string> listOfNodes { get ; set; }

		public static Thread Tmapper;

		public static void mapperFunction(){
			String dllPath = Path.Combine (MapReduceUtils.GetWorkingDirectory (), MapReduceUtils.USERDLL_NAME);

			Assembly assembly = Assembly.LoadFrom(dllPath);
			AppDomain.CurrentDomain.Load(assembly.GetName());
			Type t = assembly.GetType("Mapper");

			var propperMapper = Activator.CreateInstance(t);
			var methodSetIP = t.GetMethod("setListOfNodes");
			methodSetIP.Invoke (propperMapper,new object[]{listOfNodes});




			foreach (int chunk in chunksToProcess) {
				string line;
				String filePath = Path.Combine (MapReduceUtils.GetWorkingDirectory (), chunk.ToString() + DfsUtils.CHUNKID_SEPARATOR + fileNameIn);

				// Read the file and display it line by line.
				System.IO.StreamReader file = 
					new System.IO.StreamReader(filePath);
				while((line = file.ReadLine()) != null)
				{
					var methodRun = t.GetMethod("run");
					methodRun.Invoke (propperMapper,new object[]{line});
				}

				file.Close();

			}
			//wyslij koniec;
			var methodEndWork = t.GetMethod("endWork");
			methodEndWork.Invoke (propperMapper,null);
			Status = StatusType.WAITING_FOR_REDUCE;
		}

		public static void startWork(){
			Tmapper = new Thread (new ThreadStart (mapperFunction));
			Tmapper.Start ();
			Status = StatusType.MAPPER;
		}

		public static void createlistOfNodes(List<TaskAssigment> assigments)
		{
			listOfNodes=(from assigment in assigments
			 select assigment.workerIP).ToList ();
		}

		public static void countNumberOfNodes(List<TaskAssigment> assigments)
		{
			numberOfNodes = (from assigment in assigments
			                   group assigment by assigment.workerIP into g
			                   select  g
			).Count ();
		}

		public static string[] reducersIps {
			get {
				return (from assigment in assigments
				        orderby assigment.workerId
				        select assigment.workerIP).ToArray ();
			}
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

