using System;
using System.Collections.Generic;
using ServiceStack;

namespace Worker
{
	[Route("/reducer/sendData", "PUT")]
	public class SendMappedData:IReturn<SendMappedData>
	{
		public int chunk {
			get;
			set;
		}

		//public int id { get; set; }
		public List<KeyValuePair<string,string>> listOfMapperResults{ get; set; }
		//public string key { get; set; }

		//public string value{ get; set; }
	}

	public class SendMappedDataResponse
	{

	}
}

