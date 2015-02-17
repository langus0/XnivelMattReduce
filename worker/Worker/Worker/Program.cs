using System;

namespace Worker
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			Console.WriteLine ("Remember IP must be IDENTICAL as in master config!");
			DfsWorkerUtils.createWorkingDirectory ();
			var listeningOn = args.Length == 0 ? "http://*:1337/" : args [0];
			if (!listeningOn.Contains("http")){
				Console.WriteLine ("Start IP with http(s)");
			}
			if (!listeningOn.EndsWith("/")){
				Console.WriteLine ("End IP with /");
			}
			var appHost = new WorkerHost ().Init ().Start (listeningOn);
			StatusConfigContainer.init (listeningOn);
			Console.WriteLine ("WorkerHost Created at {0}, listening on {1}", 
			                   DateTime.Now, listeningOn);
			Console.ReadKey ();
			
		}
	}
}
