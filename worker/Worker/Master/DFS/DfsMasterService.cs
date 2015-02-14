using System;
using System.Collections.Generic;
using ServiceStack;
using Common;
namespace Master
{
	public class DfsMasterService: Service
	{

		public object Any (Ls request)
		{
			var result = DfsUtils.listFiles ();
			return new LsResponse{Result = result.Item1, inactiveWorkers = result.Item2} ;
		}
	}
}



