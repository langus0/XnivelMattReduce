using System;
using System.Collections.Generic;
using ServiceStack.Logging;
using System.IO;

namespace ApiMaperReducer
{
	public abstract class ApiReducer : ApiGlobal
	{
		private static ILog log = LogManager.GetLogger (typeof(ApiMapper));
		public StreamWriter writer;

		public abstract void reduce (string key, List<string> values);

		public void send (string key, string value)
		{
			writer.WriteLine (string.Format ("{0}\t{1}", key, value));	
		}
	}
}

