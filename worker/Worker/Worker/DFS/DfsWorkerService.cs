using System;
using ServiceStack;

namespace Worker
{
	public class DfsWorkerService: Service
	{
		public object Any (Ls request)
		{
			return new LsResponse { Result = DfsWorkerUtils.listWorkingDirectory() };
		}

		public object Get (GetChunk request)
		{
			return new GetChunkResponse { Result = DfsWorkerUtils.readChunk(request.FileName,request.chunkId) };
		}

		public object Put (SaveChunk request)
		{
			DfsWorkerUtils.writeChunk (request.FileName, request.chunkId, request.data);
			return new SaveChunkResponse ();
		}
	}
}

