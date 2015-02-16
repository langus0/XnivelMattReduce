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
			DfsWorkerUtils.writeChunk (request.chunk);
			return true;
		}

		public object Delete (DeleteChunk request)
		{
			if (request.chunkId < -1) {
				throw new Exception ("Chunk ID can not be negative! (Eventually use -1 to delete all chunks)");
			}

			if (request.chunkId == -1) { //delete all chunks of file
				DfsWorkerUtils.deleteAllChunksOfFile (request.FileName);
			} else {
				DfsWorkerUtils.deleteChunk (request.FileName, request.chunkId);
			}
			return true;
		}
	}
}

