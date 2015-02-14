using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using ServiceStack;
using Common;

namespace Master
{
	public static class DfsUtils
	{
		public static Tuple<List<FileHeader>, List<string>> listFiles ()
		{
			Dictionary<string,FileHeader> files = new Dictionary<string,FileHeader> ();
			List<string> workersWithErrors = new List <string>();
			foreach (var worker in WorkersUtil.listOfWorkers) {
				var client = new JsonServiceClient (worker);
				Worker.LsResponse response = null;
				try{
					 response = client.Get (new Worker.Ls ());}
				catch (Exception e){

				}
				if (response == null || response.IsErrorResponse()) {
					workersWithErrors.Add (worker);
					continue;
				}
				foreach (var chunk in response.Result) {
					if (files.ContainsKey (chunk.fileName)) {
						files [chunk.fileName].addChunk (new ChunksGroup(chunk, worker));
					} else {
						files [chunk.fileName] = new FileHeader (new ChunksGroup(chunk, worker));
					}
				}
			}
			return Tuple.Create (new List<FileHeader>(files.Values.ToArray()), workersWithErrors);
		}
	}
}

