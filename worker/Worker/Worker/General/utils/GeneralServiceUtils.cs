using System;
using System.Diagnostics;

namespace Worker
{
	public class GeneralServiceUtils
	{
		public static string getMemproc()
		{
			Process pMEM = new Process();
			pMEM.StartInfo.UseShellExecute = false;
			pMEM.StartInfo.RedirectStandardOutput = true;
			pMEM.StartInfo.FileName = "/bin/bash";
			pMEM.StartInfo.Arguments = "-c \"top -bn1 | awk '/Mem:/ { mem =  $5 / $3 * 100 \\\"%\\\" }; END   { print mem }' \"";

			pMEM.Start();
			string MEMproc = pMEM.StandardOutput.ReadToEnd();
			pMEM.WaitForExit();
			return MEMproc.Trim();
		}
		public static string getCPUproc(){
			Process pCPU = new Process();
			pCPU.StartInfo.UseShellExecute = false;
			pCPU.StartInfo.RedirectStandardOutput = true;
			pCPU.StartInfo.FileName = "/bin/bash";
			pCPU.StartInfo.Arguments = "-c \"ps aux  | awk 'BEGIN { sum = 0 }  { sum += $3 }; END { print sum \\\"%\\\" }' \"";

			pCPU.Start();
			string CPUproc = pCPU.StandardOutput.ReadToEnd();
			pCPU.WaitForExit();
			return CPUproc.Trim();
		}
	}
}

