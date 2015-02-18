using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Reflection;
using Common;

namespace Worker
{
	public class ReducerUtils
	{
		private static Dictionary<string, int> keyList = new Dictionary<string, int> ();
		public static bool correctid(string chunk_and_key,int id)
		{
			if (!keyList.ContainsKey (chunk_and_key)) {
				keyList.Add (chunk_and_key, 1);
				return true;
			}
			if (keyList [chunk_and_key] == id - 1) {
				keyList [chunk_and_key] = id;
				return true;
			} else {
				return false;
			}
		}


		private static Dictionary<string, int> recivedEndList = new Dictionary<string, int> ();
		public static bool newEndMapper(string chunk)
		{
			if (!recivedEndList.ContainsKey (chunk)) {
				recivedEndList.Add (chunk, 1);
				return true;
			}
			recivedEndList [chunk]++;
			return false;
		}
		public static bool recivedFromAllEndMapper(int numberOfNodes)
		{
			return keyList.Count==numberOfNodes;
		}
		public static bool recivedAllFromOneEndMapper(string chunk,int numberOfNodes)
		{
			return recivedEndList [chunk] == numberOfNodes;
		}


		public static Thread Tmapper;

		public static void mapperFunction(){
			String dllPath = Path.Combine (MapReduceUtils.GetWorkingDirectory (), MapReduceUtils.USERDLL_NAME);

			Assembly assembly = Assembly.LoadFrom(dllPath);
			AppDomain.CurrentDomain.Load(assembly.GetName());
			Type t = assembly.GetType("Reducer");

			var propperMapper = Activator.CreateInstance(t);
			var methodSetIP = t.GetMethod("setListOfNodes");
			methodSetIP.Invoke (propperMapper,new object[]{StatusConfigContainer.listOfNodes});


			//			string[] files = System.IO.Directory.GetFiles("/home/xniv/", "*.txt");

			/*
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

			}*/
			//wyslij koniec;
			var methodEndWork = t.GetMethod("endWork");
			methodEndWork.Invoke (propperMapper,null);
			StatusConfigContainer.Status = StatusType.WAITING_FOR_REDUCE;
		}

		public static void startMapperWork(){
			Tmapper = new Thread (new ThreadStart (mapperFunction));
			Tmapper.Start ();
			StatusConfigContainer.Status = StatusType.REDUCE;
		}
	}
}

