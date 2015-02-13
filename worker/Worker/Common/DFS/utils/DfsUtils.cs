using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Common
{
	public static class DfsUtils
	{
		public const String CHUNKID_SEPARATOR = "-";

		public static Tuple<String,int> splitFileName (String fileName)
		{
			if (fileName.IndexOf (CHUNKID_SEPARATOR) == -1) {
				throw new ArgumentException ("File name does not contain chunk id!");
			}

			int chunkId = int.Parse (fileName.Substring (0, fileName.IndexOf (CHUNKID_SEPARATOR)));
			String name = fileName.Substring (fileName.IndexOf (CHUNKID_SEPARATOR) + 1);
			return Tuple.Create (name, chunkId);
		}
	}
}

