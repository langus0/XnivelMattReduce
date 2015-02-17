using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using ServiceStack;
using ServiceStack.Logging;

namespace Master
{
	public class MapReduceUtils
	{
		private static ILog log = LogManager.GetLogger (typeof(MapReduceUtils));

		public static void runMRJob (RunMR runMRRequest, List<TaskAssigment> assigments)
		{
			foreach (var assigment in assigments) {
				Worker.RunMRTask request = new Worker.RunMRTask {
					fileNameIn = runMRRequest.fileNameIn,
					fileNameOut = runMRRequest.fileNameOut,
					fileWithDll = runMRRequest.fileWithDll,
					taskAssigments = assigments
				};
				var client = new JsonServiceClient (assigment.workerIP);
				Worker.RunMRTaskResponse response = null;
				log.InfoFormat ("Sending task to {0}", assigment.workerIP);
				response = client.Put (request);
			}
		}

		public static List<TaskAssigment> scheduleMRJob (string fileIn, string fileOut)
		{
			var listResult = DfsUtils.listFiles ();
			var listOfActiveWorker = WorkersUtil.listOfWorkers.Except (listResult.Item2).ToList ();
			var files = listResult.Item1;

			if (files.ContainsKey (fileOut)) {
				throw new ArgumentException ("Output file already exists!");
			}
			if (!files.ContainsKey (fileIn)) {
				throw new ArgumentException ("Input file does not exists!");
			}
			if (files [fileIn].corrupted) {
				throw new ArgumentException ("Input file is corrupted: starting MapReduce is inpossible!");
			}

			return assignTaskToWorkers (files [fileIn], listOfActiveWorker);

		}

		private static List<TaskAssigment> assignTaskToWorkers (FileHeader file, List<string> workers)
		{
			log.InfoFormat ("Scheduling MR Job on workers {0}", workers.ToJson ());
			var assigments = convertWorkersToEmptyAssigments (workers);
			var chunks = file.getChunksSorted ();
			List<int> chunksToAssign = Enumerable.Range (0, chunks.Count).ToList ();
			//If chunk is stored in only 1 worker - we have no choice
			for (int i=0; i<chunks.Count; i++) {
				if (chunks [i].storedInWorkers.Count == 1) {
					string worker = chunks [i].storedInWorkers.First ();
					chunksToAssign.Remove (i);
					assigments [worker].listOfChunksToProcess.Add (i);
					log.DebugFormat ("Assign chunk {0} to worker {1} ({2})", i, assigments [worker].workerId, worker);
				}
			}
			//next schedule chunks to the worker which has less chunks assigned (at the moment)
			foreach (var chunkId in chunksToAssign) {
				var worker = (from w in chunks [chunkId].storedInWorkers
				              orderby assigments [w].listOfChunksToProcess.Count
						      select w).First ();
				assigments [worker].listOfChunksToProcess.Add (chunkId);
				log.DebugFormat ("Assign chunk {0} to worker {1} ({2})", chunkId, assigments [worker].workerId, worker);
			}

			return assigments.Values.ToList ();
		}

		private static Dictionary<string,TaskAssigment> convertWorkersToEmptyAssigments (List<string>workers)
		{
			Dictionary<string,TaskAssigment> assigments = new Dictionary<string,TaskAssigment> (workers.Count);
			foreach (var worker in workers) {
				assigments [worker] = new TaskAssigment {
					workerIP = worker,
					workerId = assigments.Count,
					listOfChunksToProcess = new List<int> ()
				};
			}
			return assigments;
		}
	}
}

