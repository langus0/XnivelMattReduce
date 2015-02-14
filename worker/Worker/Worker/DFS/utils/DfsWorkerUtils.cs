using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
			Chunk chunk = new Chunk ();
			chunk.data = System.IO.File.ReadAllBytes (filePath);
			chunk.header = new ChunkHeader (new FileInfo (filePath));
			return chunk;
		}

		public static void writeChunk (String name, int id, byte[] data)
		{
			String filePath = Path.Combine (GetWorkingDirectory (), id.ToString () + DfsUtils.CHUNKID_SEPARATOR + name);
			if (System.IO.File.Exists (filePath)) {
				throw new ArgumentException ("File already exists!");
			}
			System.IO.File.WriteAllBytes (filePath, data);
		}

		public static void createWorkingDirectory ()
		{
			string path = GetWorkingDirectory ();

			bool exists = System.IO.Directory.Exists (path);

			if (!exists) {
				System.IO.Directory.CreateDirectory (path);
			}

		}
	}
}

