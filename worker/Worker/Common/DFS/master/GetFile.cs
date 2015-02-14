using ServiceStack;
using Common;

namespace Master
{
	[Route("/dfs/getFile/{FileName}", "GET")]
	public class GetFile: IReturn<GetFileResponse>
	{
		public string FileName{ get; set; }
	}

	public class GetFileResponse
	{
		public File Result { get; set; }
	}
}

