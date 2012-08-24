using System;
using System.Web;

namespace Bruttissimo.Common.Mvc
{
    public class ErrorHandlerModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.Error += Error;
        }

        public void Dispose()
        {
        }

        protected void Error(object sender, EventArgs args)
        {
            HttpApplication application = (HttpApplication)sender;
            ExceptionHelper exceptionHelper = IoC.Container.Resolve<ExceptionHelper>();
            HttpApplicationErrorHander errorHandler = new HttpApplicationErrorHander(application, exceptionHelper);
            errorHandler.HandleApplicationError();
        }
    }
}