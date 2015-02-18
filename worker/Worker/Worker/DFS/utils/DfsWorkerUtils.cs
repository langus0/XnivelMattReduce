using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Common;

namespace Worker
{
	public static class DfsWorkerUtils
	{
	
		private static String GetWorkingDirectory ()
		{

			return ConfigurationManager.AppSettings ["WorkingDirectory"];
		}

		public static List<ChunkHeader> listWorkingDirectory ()
		{
			List<ChunkHeader> list = new List<ChunkHeader> ();
			foreach (FileInfo file in new DirectoryInfo(GetWorkingDirectory()).EnumerateFiles()) { 
				list.Add (new ChunkHeader (file));
			}
			return list;
		}

		public static Chunk readChunk (String name, int id)
		{
			String filePath = Path.Combine (GetWorkingDirectory (), id.ToString () + DfsUtils.CHUNKID_SEPARATOR + name);
			if (!System.IO.File.Exists (filePath)) {
				throw new ArgumentException ("Chunk does not exists!");
			}

			var data = System.IO.File.ReadAllLines (filePath).ToList();
			return new Chunk (new FileInfo (filePath), data);
		}

		public static void writeChunk(Chunk chunk){
			writeChunk (chunk.fileName, chunk.chunkId, chunk.data);
		}


		public static void writeChunk (String name, int id, List<string> data)
		{
			String filePath = Path.Combine (GetWorkingDirectory (), id.ToString () + DfsUtils.CHUNKID_SEPARATOR + name);
			if (System.IO.File.Exists (filePath)) {
				throw new ArgumentException ("File already exists!");
			}
			System.IO.File.WriteAllLines (filePath, data);
		}

		public static void deleteChunk (String name, int id)
		{
			String filePath = Path.Combine (GetWorkingDirectory (), id.ToString () + DfsUtils.CHUNKID_SEPARATOR + name);
			if (!System.IO.File.Exists (filePath)) {
				throw new ArgumentException ("Chunk not exists!");
			}
			System.IO.File.Delete (filePath);
		}

		public static void deleteAllChunksOfFile (String name)
		{
			var fileChunks = (from chunk in DfsWorkerUtils.listWorkingDirectory ()
			                  where chunk.fileName == name
			                  select chunk).ToList ();
			if (fileChunks.Count == 0) {
				throw new ArgumentException ("No chunk of file exists on this worker!"); 
			}
			foreach (var chunks in fileChunks) {
				DfsWorkerUtils.deleteChunk (name, chunks.chunkId);
			}
		}

		public static void createWorkingDirectory ()
		{
			string path = GetWorkingDirectory ();

			if (!System.IO.Directory.Exists (path)) {
				System.IO.Directory.CreateDirectory (path);
			}

		}

		public static string getPathToChunk(string name, int id){
			return Path.Combine (GetWorkingDirectory (), DfsUtils.joinToFileName (name, id));
		}
	}
}

