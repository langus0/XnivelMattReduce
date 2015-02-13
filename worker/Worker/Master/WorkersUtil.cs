using System;
using System.Collections.Generic;
using System.Configuration;
namespace Master
{
	public class WorkersUtil
	{
		public static List<string> listOfWorkers {get;set;}

		public static void loadWorkersFromConfig(){
			String listFromConfig = ConfigurationManager.AppSettings ["Workers"];
			listOfWorkers = new List<string> ();
			listOfWorkers.AddRange(listFromConfig.Split (';'));

		}



	}
}


