using System;
using ServiceStack;

namespace Master
{
	public class DfsMasterService: Service
	{

		public object Any (Ls request)
		{
			return new LsResponse() ;
		}
	}
}



