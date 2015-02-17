using System;

namespace Worker
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			DfsWorkerUtils.createWorkingDirectory ();
			var listeningOn = args.Length == 0 ? "http://*:1337/" : args [0];
			var appHost = new WorkerHost ().Init ().Start (listeningOn);
			StatusConfigContainer.init (listeningOn);
			Console.WriteLine ("WorkerHost Created at {0}, listening on {1}", 
			                   DateTime.Now, listeningOn);
			Console.ReadKey ();
			
		}
	}
}
