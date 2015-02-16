using System;
using ServiceStack;


namespace Reducer
{
	[Route("/reducer/sendData", "PUT")]
	public class SendData{
		public string key { get; set;}
		public string value{ get; set;}
	}
}

