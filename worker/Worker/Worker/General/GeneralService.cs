using System;
using ServiceStack;

namespace Worker
{
	public class GeneralService: Service
	{
		public object Any(GetStatus request){
			SystemLoadChecker systemChecker = new SystemLoadChecker ();
			return new GetStatusResponse { Result = systemChecker.getAvailableRAM()+systemChecker.getCurrentCpuUsage()};
		}
	}
}

