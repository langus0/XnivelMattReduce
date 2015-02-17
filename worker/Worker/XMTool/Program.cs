using System;
using ServiceStack;
using System.Configuration;
using System.IO;
using System.Linq;

namespace XMTool
{
	class MainClass
	{
		const String FORMAT = "{0,-15}	{1,-8}	{2,-6}	{3,-10}	{4, -2}";
		const String FORMAT_DETAILS = ">>>\t {0,-8}	{1,-8}	{2,-20}";

		public static void showHelp ()
		{
			System.Console.WriteLine ("XMTool options:");
			System.Console.WriteLine (" ls - list DFS");
			System.Console.WriteLine (" ll - list DFS with details");
			System.Console.WriteLine (" register - register user\t");
			System.Console.WriteLine (" rm FILENAME - remove file from DFS");
			System.Console.WriteLine (" put FILENAME NUM_CHUNKS NUM_REPLICAS - save file to DFS");
			System.Console.WriteLine (" cat FILENAME - read file from DFS");
			System.Console.WriteLine (" run FILENAME_DLL FILE_IN FILE_OUT - run MR task");
		}

		public static JsonServiceClient getClient ()
		{
			var client = new JsonServiceClient (ConfigurationManager.AppSettings ["MasterIP"]) {
				UserName = ConfigurationManager.AppSettings ["UserName"],
				Password = ConfigurationManager.AppSettings ["Password"]
			};
			client.AlwaysSendBasicAuthHeader = true;    
			return client;
		}

		public static void ls (bool detail)
		{
			Master.Ls request = new Master.Ls ();
			Master.LsResponse response = getClient ().Get (request);
			if (response.IsErrorResponse ()) {
				System.Console.WriteLine ("Something went wrong...");
				return;
			}
			System.Console.WriteLine ("Inactive workers: {0}\n\n".FormatWith (response.inactiveWorkers.ToJson ()));


			System.Console.WriteLine (FORMAT.FormatWith ("Name", "bytes", "chunks", "corrupted", "accessibleReplicas"));
			foreach (var file in response.Result) {
				System.Console.WriteLine (FORMAT.FormatWith (file.fileName, file.sizeInBytes, file.chunksCount, file.corrupted, file.accessibleReplicas));
				if (detail) {
					System.Console.WriteLine (FORMAT_DETAILS.FormatWith ("chunkId", "bytes", "accessibleIn"));
					foreach (var chunk in file.chunks) {
						System.Console.WriteLine (FORMAT_DETAILS.FormatWith (chunk.chunkId, chunk.sizeInBytes, chunk.storedInWorkers.ToJson ()));
					}
				}
			}

		}

		public static void put (string path, int chunks, int replicas)
		{
			if (!System.IO.File.Exists (path))
				throw new ArgumentException ("File does not exists in local filesystem");

			Common.File fileToSend = new Common.File (Path.GetFileName (path), chunks);
			fileToSend.data = System.IO.File.ReadAllLines (path).ToList ();

			Master.SaveFile request = new Master.SaveFile { numOfReplicas = replicas, file = fileToSend };
			var response = getClient ().Put (request);

			if (response.IsErrorResponse ()) {
				System.Console.WriteLine ("Something went wrong...");
				return;
			}
		

		}

		public static void rm (string name)
		{
			Master.DeleteFile request = new Master.DeleteFile { FileName=name };
			var response = getClient ().Delete (request);

			if (response.IsErrorResponse ()) {
				System.Console.WriteLine ("Something went wrong...");
				return;
			}
		}

		public static void register ()
		{
			var client = new JsonServiceClient (ConfigurationManager.AppSettings ["MasterIP"]);
			Register request = new Register {UserName = ConfigurationManager.AppSettings ["UserName"],
				Password =ConfigurationManager.AppSettings ["Password"]
			};
			var response = client.Post (request);
			if (response.IsErrorResponse ()) {
				System.Console.WriteLine ("Error during registration...");
			} else {
				System.Console.WriteLine ("Registration ok");
			}

		}

		public static void run (string pathToDll, string fileIn, string fileOut)
		{
			if (!System.IO.File.Exists (pathToDll))
				throw new ArgumentException ("File with DLL does not exists in local filesystem");
			byte[] dllBytes = System.IO.File.ReadAllBytes (pathToDll);

			Master.RunMR request = new Master.RunMR { fileNameIn = fileIn, fileNameOut = fileOut, fileWithDll = dllBytes };
			getClient ().Put (request);	
		}

		public static void cat (string name)
		{
			Master.GetFile request = new Master.GetFile { FileName=name };
			Master.GetFileResponse response =  getClient ().Get (request);	
			foreach (var line in response.Result.data) {
				System.Console.WriteLine (line);
			}
		}

		public static void Main (string[] args)
		{
			if (args.Length < 1) {
				showHelp ();
				return;
			}
			if (args [0].EqualsIgnoreCase ("register")) {
				register ();
				return;
			}
			if (args [0].EqualsIgnoreCase ("ls")) {
				ls (false);
				return;
			}
			if (args [0].EqualsIgnoreCase ("ll")) {
				ls (true);
				return;
			}
			if (args [0].EqualsIgnoreCase ("put")) {
				put (args [1], Int32.Parse (args [2]), Int32.Parse (args [3]));
				return;
			}
			if (args [0].EqualsIgnoreCase ("rm")) {
				rm (args [1]);
				return;
			}
			if (args [0].EqualsIgnoreCase ("run")) {
				run (args [1], args [2], args [3]);
				return;
			}
			if (args [0].EqualsIgnoreCase ("cat")) {
				cat (args [1]);
				return;
			}
			System.Console.WriteLine ("Command not found...");
			showHelp ();
		}
	}
}
