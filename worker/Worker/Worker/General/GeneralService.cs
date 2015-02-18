using System;
using ServiceStack;
using Common;
namespace Worker
{
	public class GeneralService: Service
	{
		public object Any(GetStatus request){
			//SystemLoadChecker systemChecker = new SystemLoadChecker ();
			//return new GetStatusResponse { Result = systemChecker.getAvailableRAM()+systemChecker.getCurrentCpuUsage()};
			return new GetStatusResponse { CPUproc=GeneralServiceUtils.getCPUproc(),MEMproc=GeneralServiceUtils.getMemproc()};
		}
	}
}

