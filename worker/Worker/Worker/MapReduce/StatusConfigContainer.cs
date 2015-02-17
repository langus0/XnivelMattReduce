using System;
using System.Linq;
using Common;
using System.Collections.Generic;

namespace Worker
{
	public static class StatusConfigContainer
	{

		public static Common.StatusType Status{ get; set; }

		public static string myIP{ get; set; }

		public static List<TaskAssigment> assigments;

		public static string fileNameIn{ get; set; }

		public static string fileNameOut{ get; set; }

		public static List<int> chunksToProcess { get ; set; }

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
			myIP = ip;
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

