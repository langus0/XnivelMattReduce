using System;
using ServiceStack;
using System.IO;


namespace Worker
{
	public class Reducer: Service
	{

		public object Put (SendMappedData request)
		{
			if (ReducerUtils.correctid (request.chunk + request.key, request.id)) {
				File.AppendAllText(request.key, request.value + Environment.NewLine);
			}
			return true;
		}
		public object Put (SendMapperEndWork request)
		{
			return true;
		}
	}
}

