using System;
using System.IO;
using System.Configuration;
using ServiceStack.Logging;

namespace Worker
{
	public class MapReduceUtils
	{
		const string USERDLL_NAME = "MapReduce.dll";
		private static ILog log = LogManager.GetLogger (typeof(MapReduceUtils));

		public static string  GetWorkingDirectory ()
		{
			return Path.Combine (ConfigurationManager.AppSettings ["WorkingDirectory"], "workspace/");
		}

		public static void  clearWorkingDirectory ()
		{
			string path = GetWorkingDirectory ();
			log.InfoFormat ("Clearing working direcotry {0}", path);

			if (System.IO.Directory.Exists (path)) {
				System.IO.Directory.Delete (path, true);
			}
			System.IO.Directory.CreateDirectory (path);
		}

		public static void saveDll (byte[] data)
		{
			String filePath = Path.Combine (GetWorkingDirectory (), USERDLL_NAME);
			log.InfoFormat ("Saving dll file {0}", filePath);

			if (System.IO.File.Exists (filePath)) {
				throw new ArgumentException ("File already exists!");
			}
			System.IO.File.WriteAllBytes (filePath, data);
		}
	}
}

