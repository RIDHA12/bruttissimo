﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Bruttissimo.Common.Resources;
using log4net;

namespace Bruttissimo.Common.Mvc
{
    public class HttpApplicationErrorHander
    {
        private readonly ILog log = LogManager.GetLogger(typeof(HttpApplicationErrorHander));
        private readonly HttpApplication application;
        private readonly ExceptionHelper helper;

        public HttpApplicationErrorHander(HttpApplication application, ExceptionHelper helper)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }
            if (helper == null)
            {
                throw new ArgumentNullException("helper");
            }
            this.application = application;
            this.helper = helper;
        }

        public void HandleApplicationError()
        {
            HttpContextWrapper context = new HttpContextWrapper(HttpContext.Current);
            using (ErrorController controller = ErrorController.Instance(context))
            {
                Exception exception = application.Server.GetLastError();
                if (exception == null) // prevent bizarre scenario when handling requests to *.cshtml physical files.
                {
                    return;
                }
                HttpResponseBase response = context.Response;
                response.Clear();
                response.Status = Constants.HttpServerError;
                response.TrySkipIisCustomErrors = true;

                LogApplicationException(response, exception);
                try
                {
                    WriteViewResponse(exception, controller);
                }
                catch (Exception exceptionRenderingViewResult) // now we're in trouble. lets be as graceful as possible.
                {
                    Exception concatenated = helper.ConcatExceptions(exception, exceptionRenderingViewResult);
                    WriteGracefulResponse(concatenated, controller);
                }
                finally
                {
                    application.Server.ClearError();
                }
            }
        }

        private void LogApplicationException(HttpResponseBase response, Exception exception)
        {
            if (exception.IsHttpNotFound())
            {
                log.Debug(Error.WebResourceNotFound, exception);
                response.Status = Constants.HttpNotFound;
                return;
            }
            log.Error(Error.UnhandledException, exception);
        }

        private bool WriteJsonResponse(HttpRequestBase request, HttpResponseBase response, string message)
        {
            if (request.IsAjaxRequest())
            {
                response.Status = Constants.HttpSuccess;
                response.ContentType = Constants.JsonContentType;
                response.Write(message);
                return true;
            }
            return false;
        }

        private void WriteViewResponse(Exception exception, StringRenderingController controller)
        {
            if (!WriteJsonResponse(controller.Request, controller.Response, User.UnhandledExceptionJson))
            {
                HttpResponseBase response = controller.Response;
                ErrorViewModel model = helper.GetErrorViewModel(controller.RouteData, exception);
                string result = controller.ViewString(Constants.ErrorViewName, model);
                response.ContentType = Constants.HtmlContentType;
                response.Write(result);
            }
        }

        private void WriteGracefulResponse(Exception exception, ErrorController controller)
        {
            try
            {
                // write an HTML response from an embedded resource in the assembly.
                WriteHtmlResponse(exception, controller);
            }
            catch (Exception exceptionRenderingHtml) // we seem to be having a very rough day, lets just call it a day.
            {
                Exception concatenated = helper.ConcatExceptions(exception, exceptionRenderingHtml);
                WritePlainTextResponse(controller.Response, concatenated); // write a plain text response.
            }
        }

        private void WriteHtmlResponse(Exception exceptionWritingView, Controller controller)
        {
            log.Fatal(Error.FatalException, exceptionWritingView);
            
            HttpResponseBase response = controller.Response;
            ErrorViewModel model = helper.GetErrorViewModel(controller.RouteData, exceptionWritingView);
            string html = GetHtmlResponse(model);

            response.Clear();
            response.ContentType = Constants.HtmlContentType;
            response.Write(html);
        }

        private void WritePlainTextResponse(HttpResponseBase response, Exception exceptionWritingHtml)
        {
            log.Fatal(Error.FatalException, exceptionWritingHtml);

            response.Clear();
            response.ContentType = Constants.PlainTextContentType;
            response.Write(User.FatalException);
        }

        private string GetHtmlResponse(ErrorViewModel model)
        {
            string html = GetEmbeddedHtmlTemplate(Constants.UnrecoverableViewName);
            string htmlForException = model.DisplayException ? GetHtmlException(model.Exception) : string.Empty;
            html = html.Replace(Unrecoverable.ModelTitle, HttpUtility.HtmlEncode(User.FatalException));
            html = html.Replace(Unrecoverable.ModelRefresh, HttpUtility.HtmlEncode(User.Refresh));
            html = html.Replace(Unrecoverable.ModelMessage, HttpUtility.HtmlEncode(model.Message));
            html = html.Replace(Unrecoverable.ModelException, htmlForException);
            return html;
        }

        public string GetHtmlException(Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }
            object sqlData = exception.Data["SQL"];
            string sqlHtml = string.Empty;

            if (sqlData != null)
            {
                string sql = sqlData.ToString();
                sqlHtml = Unrecoverable.Sql.FormatWith(HttpUtility.HtmlEncode(sql));
            }

            StringBuilder stackTrace = new StringBuilder();
            string stackTraceHtml = string.Empty;

            if (exception.StackTrace != null)
            {
                string[] lines = exception.StackTrace.SplitOnNewLines();
                foreach (string line in lines)
                {
                    stackTrace.AppendFormat(Unrecoverable.StackTraceLine, HttpUtility.HtmlEncode(line.Trim()));
                }
                stackTraceHtml = Unrecoverable.StackTrace.FormatWith(stackTrace);
            }

            string innerException = GetHtmlException(exception.InnerException);
            if (!innerException.NullOrBlank())
            {
                innerException = Unrecoverable.InnerException.FormatWith(innerException);
            }
            string html = GetEmbeddedHtmlTemplate(Constants.UnrecoverableExceptionViewName);
            html = html.Replace(Unrecoverable.ModelMessage, HttpUtility.HtmlEncode(exception.Message));
            html = html.Replace(Unrecoverable.ModelSql, sqlHtml);
            html = html.Replace(Unrecoverable.ModelStackTrace, stackTraceHtml);
            html = html.Replace(Unrecoverable.ModelInnerException, innerException);
            return html;
        }

        private string GetEmbeddedHtmlTemplate(string viewName)
        {
            Type type = typeof(HttpApplicationErrorHander);
            Assembly assembly = type.Assembly;
            Stream stream = assembly.GetManifestResourceStream(type, viewName);
            string html = stream.ReadFully(); // we don't use RazorEngine here, to avoid any complications in case what's faulty is RazorEngine itself.
            return html;
        }
    }
}
