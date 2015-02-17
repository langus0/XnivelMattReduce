using System;
using ServiceStack;

namespace Worker
{
	[Route("/reducer/mapperEndWork", "PUT")]
	public class SendMapperEndWork : IReturn<SendMapperEndWorkResponse>
	{
		public string chunk {
			get;
			set;
		}
	}
	public class SendMapperEndWorkResponse{

	}
}

