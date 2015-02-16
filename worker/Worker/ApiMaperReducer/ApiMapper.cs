using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Logging;


namespace ApiMaperReducer
{
	public abstract class ApiMapper : ApiGlobal
	{
		private static ILog log = LogManager.GetLogger (typeof(ApiMapper));

		public void send(string key,string value)
		{
			Common.HashUtils HashUtil = new Common.HashUtils ();
			String hashedKey = HashUtil.makeHashKey (key);
			List<string> ListOfAddresses = HashUtil.makeListOfAddressesFromKey (hashedKey, listOfNodes);
			foreach (var worker in ListOfAddresses) {
				var client = new JsonServiceClient (worker);


				//DOPOPRAWY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

				Reducer.SendData response = null;
				try {
		//			response = client.Put (new Reducer.SendData ());
				} catch (Exception e) {
					log.Error (e);
				}
			}
	//		log.Info ("Listing DFS: done ({0} files, problems with {1})".FormatWith(files.Count, workersWithErrors.ToJson()));
		}
	}
}

