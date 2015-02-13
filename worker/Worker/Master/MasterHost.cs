using System;
using ServiceStack;
using Funq;
using ServiceStack.Text;
using ServiceStack.Logging;


namespace Master
{
	public class MasterHost:AppSelfHostBase
	{
		public MasterHost () : base("XnivelMattReduce Master", typeof(DfsMasterService).Assembly)
		{
		}

		public override void Configure (Container container)
		{
			var config = new HostConfig ();
			config.DebugMode = true; //Show StackTraces in service responses during development
			config.WriteErrorsToResponse = true;
			config.ReturnsInnerException = true;
			SetConfig (config);
		}
	}
}



