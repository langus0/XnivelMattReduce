using System;
using ServiceStack;
using ServiceStack.Text;
using Funq;
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
			this.ServiceExceptionHandlers.Add((httpReq, request, exception) => {
				//log your exceptions here

				System.Console.WriteLine(exception.Message);
				System.Console.WriteLine(exception.StackTrace);
					//call default exception handler or prepare your own custom response
					return DtoUtils.CreateErrorResponse(request, exception);
			});



			var config = new HostConfig ();
			config.DebugMode = true; //Show StackTraces in service responses during development
			config.WriteErrorsToResponse = true;
			config.ReturnsInnerException = true;
			SetConfig (config);
		}
	}
}



