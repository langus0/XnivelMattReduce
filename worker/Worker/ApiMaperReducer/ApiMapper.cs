using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Logging;
//using Common;


namespace ApiMaperReducer
{
	public abstract class ApiMapper : ApiGlobal
	{
		public int chunk { private get; set; }
		private static ILog log = LogManager.GetLogger (typeof(ApiMapper));
		private Dictionary<string, int> keyList = new Dictionary<string, int> ();
		private int IdFromKey(string key)
		{
			if (!keyList.ContainsKey (key)) {
				keyList.Add (key, 1);
			} else {
				keyList [key] += 1;
			}
			return keyList [key];

		}
		public void endWork()
		{
			foreach (var worker in listOfNodes) {
				var client = new JsonServiceClient (worker);

				Worker.SendMapperEndWork sendData = new Worker.SendMapperEndWork ();
				sendData.chunk = chunk;

				try {
					log.DebugFormat("Sending endwork to {0}", worker);
					var response = client.Put (sendData);
				} catch (Exception e) {
					log.Error (e);
				}
			}
		}
		public abstract void map (string line);

		public void send(string key,string value)
		{
			Common.HashUtils HashUtil = new Common.HashUtils ();
			String hashedKey = HashUtil.makeHashKey (key);
			string[] ListOfAddresses = HashUtil.makeListOfAddressesFromKey (hashedKey, listOfNodes);
			foreach (var worker in ListOfAddresses) {
				var client = new JsonServiceClient (worker);

				Worker.SendMappedData sendData = new Worker.SendMappedData ();
				sendData.id = IdFromKey (key);
				sendData.chunk = chunk;
				sendData.key = key;
				sendData.value = value;

				try {
					//var response = client.Put (new Worker.SendMappedData ());
					var response = client.Put (sendData);
				} catch (Exception e) {
					log.Error (e);
				}
			}
	//		log.Info ("Listing DFS: done ({0} files, problems with {1})".FormatWith(files.Count, workersWithErrors.ToJson()));
		}
	}
}

