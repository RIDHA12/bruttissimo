using System;
using System.Web;
using log4net;

namespace Bruttissimo.Common.Mvc
{
    public class LogModule : IHttpModule
    {
        private const string HTTP_BEGIN_REQUEST = "HTTP Begin Request";

        private readonly ILog log = LogManager.GetLogger(typeof(LogModule));

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        public void Dispose()
        {
        }

        protected void BeginRequest(object sender, EventArgs args)
        {
            if (Config.Debug.RequestLog)
            {
                log.Debug(HTTP_BEGIN_REQUEST);
            }
        }
    }
}
