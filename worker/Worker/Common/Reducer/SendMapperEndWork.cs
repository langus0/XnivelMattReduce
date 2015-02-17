using System;
using ServiceStack;

namespace Worker
{
	[Route("/reducer/mapperEndWork", "PUT")]
	public class SendMapperEndWork
	{
		public string chunk {
			get;
			set;
		}
	}
}

