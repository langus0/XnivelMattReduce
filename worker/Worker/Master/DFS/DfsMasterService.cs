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
			return new LsResponse{Result = new List<FileHeader>(result.Item1.Values.ToArray()), inactiveWorkers = result.Item2} ;
		}

		public object Get (GetFile request)
		{
			var result = DfsUtils.readFileFromDfs (request.FileName);
			return new GetFileResponse { Result = result };

		}
		public object Delete(DeleteFile request){
			DfsUtils.deleteFileFromDfs (request.FileName);
			return true;
		}
	}
}



