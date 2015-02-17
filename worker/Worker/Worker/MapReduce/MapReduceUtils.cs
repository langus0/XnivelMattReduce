using System;
using System.IO;
using System.Configuration;
using ServiceStack.Logging;

namespace Worker
{
	public static class MapReduceUtils
	{
		public const string USERDLL_NAME = "MapReduce.dll";
		private static ILog log = LogManager.GetLogger (typeof(MapReduceUtils));

		public static string  GetWorkingDirectory ()
		{
			return Path.Combine (ConfigurationManager.AppSettings ["WorkingDirectory"], "workspace/");
		}

		public static string pathToDll{ get {
				return Path.Combine (GetWorkingDirectory (), USERDLL_NAME);

			} }

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
			log.InfoFormat ("Saving dll file {0}", pathToDll);

			if (System.IO.File.Exists (pathToDll)) {
				throw new ArgumentException ("File already exists!");
			}
			System.IO.File.WriteAllBytes (pathToDll, data);
		}
	}
}

