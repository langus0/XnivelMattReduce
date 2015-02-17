using System;
using System.Collections.Generic;

namespace Common
{
	public class TaskAssigment
	{
		public string workerIP{ get; set; }

		public List<int> listOfChunksToProcess{ get; set; }

		public int workerId{ get; set; }
	}
}

