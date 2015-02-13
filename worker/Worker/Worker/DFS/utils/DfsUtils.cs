using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Worker
{
	public static class DfsUtils
	{
		const String CHUNKID_SEPARATOR = "-";

		private static String GetWorkingDirectory(){

			return ConfigurationManager.AppSettings["WorkingDirectory"];
		}

		public static Tuple<String,int> splitFileName(String fileName){
			if (fileName.IndexOf (CHUNKID_SEPARATOR) == -1) {
				throw new ArgumentException ("File name does not contain chunk id!");
			}

			int chunkId = int.Parse( fileName.Substring(0,fileName.IndexOf(CHUNKID_SEPARATOR)));
			String name = fileName.Substring(fileName.IndexOf(CHUNKID_SEPARATOR)+1);
			return Tuple.Create (name, chunkId);
		}

		public static List<ChunkHeader> listWorkingDirectory(){
			List<ChunkHeader> list = new List<ChunkHeader>();
			foreach(FileInfo file in new DirectoryInfo(GetWorkingDirectory()).EnumerateFiles())
			{ 
				list.Add( new ChunkHeader (file));
			}
			return list;
		}

		public static Chunk readChunk(String name, int id){
			String filePath = Path.Combine (GetWorkingDirectory (), id.ToString()+CHUNKID_SEPARATOR+name);
			if (!File.Exists (filePath)) {
				throw new ArgumentException ("Chunk does not exists!");
			}
			Chunk chunk = new Chunk();
			chunk.data = File.ReadAllBytes (filePath);
			chunk.header = new ChunkHeader (new FileInfo(filePath));
			return chunk;
		}

		public static void writeChunk(String name, int id, byte[] data){
			String filePath = Path.Combine (GetWorkingDirectory (), id.ToString()+CHUNKID_SEPARATOR+name);
			if (File.Exists (filePath)) {
				throw new ArgumentException ("File already exists!");
			}
			File.WriteAllBytes (filePath, data);
		}

		public static void createWorkingDirectory(){
			string path =GetWorkingDirectory();

			bool exists = System.IO.Directory.Exists(path);

			if(!exists){
				System.IO.Directory.CreateDirectory(path);
			}

		}
	}


}

