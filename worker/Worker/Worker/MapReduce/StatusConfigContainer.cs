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

	//	private static ILog log = LogManager.GetLogger (typeof(StatusConfigContainer));

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
			MapperRunner.clear ();
			ReducerRunner.clear ();
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

