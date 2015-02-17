using System;
using ServiceStack;


namespace Worker
{
	[Route("/reducer/sendData", "PUT")]
	public class SendMappedData{
		public string chunk {
			get;
			set;
		}

		public int id { get; set;}
		public string key { get; set;}
		public string value{ get; set;}
	}
}

