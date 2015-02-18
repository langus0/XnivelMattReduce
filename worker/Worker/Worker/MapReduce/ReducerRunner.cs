using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Linq;
using ServiceStack.Logging;
using Common;

namespace Worker
{
	public class ReducerRunner
	{
		private static ILog log = LogManager.GetLogger (typeof(ReducerRunner));
		private static Dictionary<string, int> keyList = new Dictionary<string, int> ();
		private static Dictionary<int, int> recivedEndList = new Dictionary<int, int> ();

		private const string MAPER_RESULT_EXTENSION = ".key";

		public static void clear(){
			keyList = new Dictionary<string, int> ();
			recivedEndList = new Dictionary<int, int> ();
			SaveQueue.clear ();
		}

		public static void saveMapResult (SendMappedData request)
		{

			var path = Path.Combine (MapReduceUtils.GetWorkingDirectory (), getIdOfKey (request.key) + MAPER_RESULT_EXTENSION);
			SaveQueue.append (path, request.value);
				//System.IO.File.AppendAllText (path, request.value + Environment.NewLine);
	
		}

		private static int getIdOfKey(string key){
			lock (keyList) {
				if (!keyList.ContainsKey (key)) {
					keyList.Add (key,keyList.Count);

				}
				return keyList [key];}
		}
		/*
		 * public static void saveMapResult (SendMappedData request)
		{
			if (isUnseenId (request.chunk,  request.key, request.id)) {

				var path = Path.Combine (MapReduceUtils.GetWorkingDirectory (), request.key + MAPER_RESULT_EXTENSION);
				System.IO.File.AppendAllText (path, request.value + Environment.NewLine);
			}
		}
		
		private static bool isUnseenId (int chunk, string key, int id)
		{
			var chunkAndKey = chunk + "#" + key;
			if (!keyList.ContainsKey (chunkAndKey)) {
				keyList.Add (chunkAndKey, 1);
				return true;
			}
			if (keyList [chunkAndKey] == id - 1) {
				keyList [chunkAndKey] = id;
				return true;
			} else {
				return false;
			}
		}
		*/
	/*	private static bool isUnseenId (int chunk, string key, int id)
		{
			var chunkAndKey = chunk + "#" + key;
			if (!keyList.ContainsKey (chunkAndKey)) {
				keyList.Add (chunkAndKey, 1);
				return true;
			}
			if (keyList [chunkAndKey] == id - 1) {
				keyList [chunkAndKey] = id;
				return true;
			} else {
				return false;
			}
		}
*/
		public static bool newEndMapper (int chunk)
		{
			if (!recivedEndList.ContainsKey (chunk)) {
				recivedEndList.Add (chunk, 1);
				return true;
			}
			recivedEndList [chunk]++;
			return false;
		}

		public static bool recivedFromAllEndMapper ()
		{
			log.DebugFormat("Recived end token from {0}/{1} chunks" ,recivedEndList.Count,StatusConfigContainer.totalNumberOfChunks);
			return recivedEndList.Count == StatusConfigContainer.totalNumberOfChunks;
		}

		public static bool recivedAllFromOneEndMapper (int chunk, int numberOfNodes)
		{
			return recivedEndList [chunk] == numberOfNodes;
		}

		public static void runReduce ()
		{
			log.Info ("Running reduce...");
			StatusConfigContainer.Status = StatusType.REDUCE;
			SaveQueue.saveToFile ();
			Thread thread = new Thread (new ThreadStart (ReducerRunner.reduce));
			thread.Start ();
		}

		public static void reduce ()
		{
			Assembly assembly = Assembly.LoadFrom (MapReduceUtils.pathToDll);
			Type reduceClass = assembly.GetTypes().Where(t=>typeof( ApiMaperReducer.ApiReducer).IsAssignableFrom(t)).First();

			if (reduceClass == null) {
				throw new Exception ("Reducer class not found!");
			}

			ApiMaperReducer.ApiReducer reducer = (ApiMaperReducer.ApiReducer)Activator.CreateInstance (reduceClass);

			String filePath = DfsWorkerUtils.getPathToChunk (StatusConfigContainer.fileNameOut, StatusConfigContainer.workerId);
			StreamWriter writer = new StreamWriter (filePath);
			reducer.writer = writer;

			foreach (var key in  keyList) {
				var keyFile = key.Value+ MAPER_RESULT_EXTENSION;
				//var key = keyFileName.Substring (0, keyFileName.Length - MAPER_RESULT_EXTENSION.Length); //check it!!!
				List<String> values = System.IO.File.ReadAllLines (Path.Combine (MapReduceUtils.GetWorkingDirectory (), keyFile)).ToList();

				reducer.reduce (key.Key, values);
			}

			writer.Close ();
			log.Info ("Reducer ended its work!");
			StatusConfigContainer.Status = StatusType.END;
		}
	}

	public static class SaveQueue{
		private static Dictionary<string, List<string>> valuesToSave = new Dictionary<string, List<string>>  ();
		private static int counter = 0;

		public static void clear(){
			valuesToSave = new Dictionary<string, List<string>>  ();
			counter = 0;

		}
		public static void append(string key, string value){
			lock (valuesToSave) {
				if (!valuesToSave.ContainsKey (key)) {
					valuesToSave.Add (key, new List<string> ());
				}
				counter++;
				valuesToSave[key].Add(value);
				if (counter > 100000) {
					saveToFile ();
				}
			}
		}

		public static void saveToFile(){
				foreach(var values in valuesToSave){
					System.IO.File.AppendAllLines  (values.Key, values.Value );
				}
				valuesToSave.Clear();
		}
	
	}
}

