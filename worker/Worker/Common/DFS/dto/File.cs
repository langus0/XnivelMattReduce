using System;
using System.Collections.Generic;

namespace Common
{
	public class File
	{
		public String fileName{ get; set; }

		public int chunksCount{ get; set; }

		public List<string> data{ get; set; }

		public File(string fileName, int chunksCount){
			this.fileName = fileName;
			this.chunksCount=chunksCount;
			this.data = new List<string>();
		}
	}
}

