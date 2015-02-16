using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ServiceStack;
namespace Master
{
	public class WorkersUtil
	{
		public static List<String> listOfWorkers {get;set;}

		public static void loadWorkersFromConfig(){
			String listFromConfig = ConfigurationManager.AppSettings ["Workers"];
			listOfWorkers = new List<String> ();
			listOfWorkers.AddRange(listFromConfig.Split (';'));

		}

		public static List<string> getActiveWorkers(){
			var lsResult = DfsUtils.listFiles ();
			return WorkersUtil.listOfWorkers.Except (lsResult.Item2).ToList ();

		}
		public static List<JsonServiceClient> getClientsToActiveWorkers ()
		{
			return (from worker in getActiveWorkers()
			        select new JsonServiceClient (worker)).ToList ();
			//for (int i = 0;; i++) {
			//	yield return listOfWorkers [i % listOfWorkers.Count];
			//}
		}
	}
}


