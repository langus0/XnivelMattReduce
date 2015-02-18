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
		private SendQueue queue;
	/*	private Dictionary<string, int> keyList = new Dictionary<string, int> ();
		private int IdFromKey(string key)
		{
			if (!keyList.ContainsKey (key)) {
				keyList.Add (key, 1);
			} else {
				keyList [key] += 1;
			}
			return keyList [key];

		}*/
		public void endWork()
		{
			queue.send ();
			foreach (var worker in listOfNodes) {
				var client = new JsonServiceClient (worker);

				Worker.SendMapperEndWork sendData = new Worker.SendMapperEndWork ();
				sendData.chunk = chunk;

				try {
					log.DebugFormat("Sending endwork to {0}", worker);
					var response = client.Put (sendData);
				} catch (Exception e) {
					log.Error ("Error in sending endWork",e);
				}
			}
		}
		public abstract void map (string line);

		/*public void send(string key,string value)
		{
			Common.HashUtils HashUtil = new Common.HashUtils ();
			String hashedKey = HashUtil.makeHashKey (key);
			string[] ListOfAddresses = HashUtil.makeListOfAddressesFromKey (hashedKey, listOfNodes);
			foreach (var worker in ListOfAddresses) {
				var client = new JsonServiceClient (worker);

				Worker.SendMappedData sendData = new Worker.SendMappedData ();
				//sendData.id = IdFromKey (key);
				sendData.chunk = chunk;
				var toSend = new List<KeyValuePair<string, string>> ();
				toSend.Add (new KeyValuePair<string,string> (key, value));
				sendData.listOfMapperResults = toSend;
//				sendData.key = key;
//				sendData.value = value;

				try {
					//var response = client.Put (new Worker.SendMappedData ());
					var response = client.Put (sendData);
				} catch (Exception e) {
					log.ErrorFormat ("Error sending <{0}, {1}> to {2}",key, value, worker );
					log.Error (e);
				}
			}
	//		log.Info ("Listing DFS: done ({0} files, problems with {1})".FormatWith(files.Count, workersWithErrors.ToJson()));
		}*/

		public void send(string key,string value)
		{
			queue.append (key,value);
		}

		public void init(int chunk)
		{
			queue = new SendQueue (listOfNodes, chunk);
		}
	}

	public class SendQueue{
		private static ILog log = LogManager.GetLogger (typeof(SendQueue));
		private  Dictionary<int, Worker.SendMappedData> valuesToSend= new Dictionary<int, Worker.SendMappedData>  ();
		private  int counter = 0;
		private  int maxReducers {get{
				return listOfNodes.Length;
			}}
		private  int chunk = 0;
		private string[] listOfNodes;
		public SendQueue(string[] reducers, int chunk){
			this.listOfNodes = reducers;
			this.chunk = chunk;
			initValuesToSend ();
		}

		public void initValuesToSend (){
			valuesToSend =  new Dictionary<int, Worker.SendMappedData>  ();
			for (int i =0; i<maxReducers; i++) {
				valuesToSend [i] = new Worker.SendMappedData ();
				valuesToSend [i].chunk = chunk;
				valuesToSend [i].listOfMapperResults = new List<KeyValuePair<string, string>> ();
			}
		}

		public void append(string key, string value){
			var i = Common.HashUtils.getReducerIDFromKey (key, listOfNodes);
			valuesToSend [i].listOfMapperResults.Add (new KeyValuePair<string, string>(key,value));
				counter++;

			if (counter > 5000) {
				send ();
				counter = 0;
			}

		}

		public void send(){
			log.Info ("Saving map results to workers");
			foreach(var values in valuesToSend){
				if (values.Value.listOfMapperResults.Count > 0) {
					var client = new JsonServiceClient (listOfNodes[values.Key]);
					try {
						//var response = client.Put (new Worker.SendMappedData ());
						var response = client.Put (values.Value);
					} catch (Exception e) {
						log.ErrorFormat ("Error sending map result to {0}", listOfNodes[values.Key] );
						log.Error (e);
					}
				}
			}
			valuesToSend.Clear();
			initValuesToSend ();
		}

	}

}

