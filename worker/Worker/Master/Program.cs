using System;

namespace Master
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			WorkersUtil.loadWorkersFromConfig ();

			var listeningOn = args.Length == 0 ? "http://*:8080/" : args [0];
			var appHost = new MasterHost ().Init ().Start (listeningOn);

			Console.WriteLine ("Master Created at {0}, listening on {1}", 
			                   DateTime.Now, listeningOn);

			Console.ReadKey ();
		}
	}
}
