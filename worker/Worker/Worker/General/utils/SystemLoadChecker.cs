using System;
using System.Diagnostics;

namespace Worker
{

	//TEN KOD NIE DZIAŁA!!!!!!!!!!!!! ;(
	public class SystemLoadChecker
	{   
		PerformanceCounter cpuCounter;
		PerformanceCounter ramCounter;
		PerformanceCounter myCpuCounter;
		PerformanceCounter myRamCounter;

		public SystemLoadChecker ()
		{

			cpuCounter = new PerformanceCounter ("Processor", "% Processor Time", "_Total",true); 
			ramCounter = new PerformanceCounter ("Memory", "Available MBytes");
			myCpuCounter = new PerformanceCounter ("Process", "% Processor Time", Process.GetCurrentProcess ().ProcessName);
			myRamCounter = new PerformanceCounter ("Process", "Working Set", Process.GetCurrentProcess ().ProcessName);
		}

		public string getCurrentCpuUsage ()
		{
			return cpuCounter.NextValue () + "%";
		}

		public string getAvailableRAM ()
		{
			return ramCounter.NextValue () + "MB";
		}
	}
}

