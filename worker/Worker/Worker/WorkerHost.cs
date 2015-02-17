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
	public class WorkerHost : AppHostHttpListenerBase
	{

		public WorkerHost () : base("XnivelMattReduce Worker",new[] {  typeof(DfsWorkerService).Assembly,typeof(Reducer).Assembly})
		{
		}

		
		public override ServiceStackHost Start (string urlBase)
		{
			return base.Start (urlBase);
		}
		public override void Configure (Container container)
		{

			LogManager.LogFactory = new NLogFactory ();

			GlobalRequestFilters.Add ((req, resp, requestDto) => {
				ILog log = LogManager.GetLogger (GetType ());
				log.Info (string.Format (
					"REQ {0}: {1} {2} {3} {4} {5}\n",
					DateTimeOffset.Now.Ticks,req.Verb,
					req.OperationName, req.RemoteIp, req.RawUrl, req.UserAgent));
			});
			GlobalResponseFilters.Add ((req, resp, requestDto) => {
				ILog log = LogManager.GetLogger (GetType ());
				log.Info (string.Format (
					"RES {0}: {1} {2}\n",
					DateTimeOffset.Now.Ticks, resp.StatusCode, resp.ContentType));
			});
			this.ServiceExceptionHandlers.Add((httpReq, request, exception) => {
				ILog log = LogManager.GetLogger (GetType ());
				log.Error(exception);
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

