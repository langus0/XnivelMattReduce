using System;
using System.Collections.Generic;
using System.Configuration;
using ServiceStack;
using ServiceStack.Logging;
using Common;

namespace Master
{
	public static class DfsUtils
	{

		private static ILog log = LogManager.GetLogger (typeof(DfsUtils));

		public static Tuple<Dictionary<string, FileHeader>, List<string>> listFiles ()
		{
			log.Info ("Listing DFS...");
			Dictionary<string,FileHeader> files = new Dictionary<string,FileHeader> ();
			List<string> workersWithErrors = new List <string> ();
			foreach (var worker in WorkersUtil.listOfWorkers) {
				var client = new JsonServiceClient (worker);
				Worker.LsResponse response = null;
				try {
					response = client.Get (new Worker.Ls ());
				} catch (Exception e) {
					log.Error (e);
				}
				if (response == null || response.IsErrorResponse ()) {
					workersWithErrors.Add (worker);
					continue;
				}
				foreach (var chunk in response.Result) {
					if (files.ContainsKey (chunk.fileName)) {
						files [chunk.fileName].addChunk (new ChunksGroup (chunk, worker));
					} else {
						files [chunk.fileName] = new FileHeader (new ChunksGroup (chunk, worker));
					}
				}
			}
			log.Info ("Listing DFS: done ({0} files, problems with {1})".FormatWith(files.Count, workersWithErrors.ToJson()));
			return Tuple.Create (files, workersWithErrors);
		}

		public static File readFileFromDfs (string fileName)
		{
			var lsResult = listFiles ();
			var files = lsResult.Item1;
			var workersWithErrors = lsResult.Item2;

			if (!files.ContainsKey (fileName)) {
				throw new Exception ("File does not exists!");
			}
			var fileHeader = files [fileName];
			if (fileHeader.corrupted) {
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
					try {
						log.Info ("Getting chunk {1}-{0} from {2}".FormatWith (chunk.chunkId, fileName, worker));
						response = client.Get (new Worker.GetChunk { chunkId = chunk.chunkId, FileName = fileName });
					} catch (Exception e) {
						log.Warn (e);
					}
					if (response == null || response.IsErrorResponse ()) {
						workersWithErrors.Add (worker);
						continue;
					}

					Array.Copy (response.Result.data, 0, file.data, idx, response.Result.data.Length);
					idx += response.Result.data.Length;
					chunkDownloaded = true;
					break;
				}
				if (!chunkDownloaded) {
					throw new Exception ("Donwloading of chunk {0} of {1} failed!".FormatWith (chunk.chunkId, file.fileName));
				}
			}

			return file;
		
		}

		public static void deleteFileFromDfs (string fileName)
		{

			List<string> workersWithErrors = new List <string> ();

			foreach (var worker in WorkersUtil.listOfWorkers) {
				var client = new JsonServiceClient (worker);
				System.Net.HttpWebResponse response = null;
				try {
					log.Info ("Deleting chunks of {0} from {1}".FormatWith (fileName, worker));
					response = client.Delete (new Worker.DeleteChunk { chunkId = -1, FileName = fileName });
				} catch (Exception e) {
					log.Warn (e.Message);
				}
				if (response == null || response.IsErrorResponse ()) {
					workersWithErrors.Add (worker);
					continue;
				}
			}
			log.Warn ("During deletion of file {0} having problem with workers: {1}".FormatWith(fileName,workersWithErrors.ToJson ()));
		}
	}
}

