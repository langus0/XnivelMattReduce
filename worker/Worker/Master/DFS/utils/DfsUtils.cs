using System;
using System.Collections.Generic;
using System.Configuration;
using ServiceStack;
using Common;

namespace Master
{
	public static class DfsUtils
	{
		public static Tuple<Dictionary<string, FileHeader>, List<string>> listFiles ()
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
			return Tuple.Create (files, workersWithErrors);
		}

		public static File readFileFromDfs(string fileName){
			var lsResult = listFiles();
			var files = lsResult.Item1;
			var workersWithErrors = lsResult.Item2;

			if (!files.ContainsKey (fileName)) {
				throw new Exception ("File does not exists!");
			}
			var fileHeader = files [fileName];
			if (fileHeader.corrupted){
				throw new Exception ("Can't read file: File is corrupted!");
			}

			var file = new File (fileName, fileHeader.chunksCount);
			file.data = new byte[fileHeader.sizeInBytes];
			int idx = 0;

			foreach (var chunk in fileHeader.getChunksSorted()) {
				bool chunkDownloaded = false;
				foreach (var worker in chunk.storedInWorkers) {
					var client = new JsonServiceClient (worker);
					Worker.GetChunkResponse response = null;
					try{
						System.Console.WriteLine("GetChunk {1}-{0} to {2}".FormatWith(chunk.chunkId, fileName, worker));
						response = client.Get (new Worker.GetChunk{chunkId =
						chunk.chunkId, FileName = fileName} );}
					catch (Exception e){
						System.Console.WriteLine(e.Message);
					}
					if (response == null || response.IsErrorResponse()) {
						workersWithErrors.Add (worker);
						continue;
					}

					Array.Copy (response.Result.data, 0, file.data, idx, response.Result.data.Length);
					idx += response.Result.data.Length;
					chunkDownloaded = true;
					break;
				}
				if (!chunkDownloaded) {
					throw new Exception ("Donwloading of chunk {0} of {1} failed!".FormatWith(chunk.chunkId, file.fileName));
				}
			}

			return file;
		
		}
	}
}

