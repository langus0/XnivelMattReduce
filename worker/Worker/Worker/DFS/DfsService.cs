using System;
using ServiceStack;

namespace Worker
{
	public class DfsService: Service
	{
		public object Any (Ls request)
		{
			return new LsResponse { Result = DfsUtils.listWorkingDirectory() };
		}

		public object Get (GetChunk request)
		{
			return new GetChunkResponse { Result= DfsUtils.readChunk(request.FileName,request.chunkId) };
		}

		public object Put (SaveChunk request)
		{
			DfsUtils.writeChunk (request.FileName, request.chunkId, request.data);
			return new SaveChunkResponse ();
		}
	}
}

