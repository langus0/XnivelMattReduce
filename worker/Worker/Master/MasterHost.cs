using System;
using ServiceStack;
using ServiceStack.Text;
using Funq;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;

namespace Master
{
	public class MasterHost:AppHostHttpListenerBase
	{
		public MasterHost () : base("XnivelMattReduce Master", typeof(DfsMasterService).Assembly)
		{
		}

		public override void Configure (Container container)
		{

			LogManager.LogFactory = new NLogFactory ();

			GlobalRequestFilters.Add ((req, resp, requestDto) => {
				ILog log = LogManager.GetLogger (GetType ());
				log.Info (string.Format (
					"REQ {0}: {1} {2} {3} {4} {5} \n",
					DateTimeOffset.Now.Ticks, req.Verb,
					req.OperationName, req.RemoteIp, req.RawUrl, req.UserAgent));
			});
			GlobalResponseFilters.Add ((req, resp, requestDto) => {
				ILog log = LogManager.GetLogger (GetType ());
				log.Info (string.Format (
					"RES {0}: {1} {2}\n",
					DateTimeOffset.Now.Ticks, resp.StatusCode, resp.ContentType));
			});

			this.ServiceExceptionHandlers.Add ((httpReq, request, exception) => {
				ILog log = LogManager.GetLogger (GetType ());
				log.Error(exception);
				//call default exception handler or prepare your own custom response
				return DtoUtils.CreateErrorResponse (request, exception);
			});

			/*
			Plugins.Add (new AuthFeature (() => new AuthUserSession (),
			                              new IAuthProvider[] {new BasicAuthProvider ()})
			             );
			Plugins.Add (new RegistrationFeature ());
			//Plugins.Add(new RequestLogsFeature());

			container.Register<ICacheClient>(new MemoryCacheClient());
			var userRep = new InMemoryAuthRepository();
			container.Register<IUserAuthRepository>(userRep);
			*/
			var config = new HostConfig ();
			config.DebugMode = true; //Show StackTraces in service responses during development
			config.WriteErrorsToResponse = true;
			config.ReturnsInnerException = true;
			SetConfig (config);
		}
	}
}



