using System;
using ServiceStack;
using Funq;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.Text;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.Redis;

namespace Worker
{
	public class WorkerHost : AppSelfHostBase
	{

		public WorkerHost () : base("XnivelMattReduce Worker", typeof(DfsWorkerService).Assembly)
		{
		}

		
		public override ServiceStackHost Start (string urlBase)
		{
			return base.Start (urlBase);
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

