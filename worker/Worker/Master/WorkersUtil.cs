using System;
using System.Collections.Generic;
using System.Configuration;
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
	}
}


