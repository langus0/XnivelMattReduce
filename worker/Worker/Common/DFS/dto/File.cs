using System;
using System.Collections.Generic;

namespace Common
{
	public class File
	{
		public String fileName{ get; set; }

		public int chunksCount{ get; set; }

		public byte[] data{ get; set; }

		public File(string fileName, int chunksCount){
			this.fileName = fileName;
			this.chunksCount=chunksCount;
		}
	}
}

