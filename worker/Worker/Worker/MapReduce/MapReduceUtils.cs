using System;
using System.IO;
using System.Configuration;

namespace Worker
{
	public class MapReduceUtils
	{
		const string USERDLL_NAME = "MapReduce.dll";

		public static string  GetWorkingDirectory ()
		{
			return Path.Combine (ConfigurationManager.AppSettings ["WorkingDirectory"], "workspace/");
		}

		public static void  clearWorkingDirectory ()
		{
			string path = GetWorkingDirectory ();

			if (System.IO.Directory.Exists (path)) {
				System.IO.Directory.Delete (path, true);
			}
			System.IO.Directory.CreateDirectory (path);
		}

		public static void saveDll (byte[] data)
		{
			String filePath = Path.Combine (GetWorkingDirectory (), USERDLL_NAME);
			if (System.IO.File.Exists (filePath)) {
				throw new ArgumentException ("File already exists!");
			}
			System.IO.File.WriteAllBytes (filePath, data);
		}
	}
}

