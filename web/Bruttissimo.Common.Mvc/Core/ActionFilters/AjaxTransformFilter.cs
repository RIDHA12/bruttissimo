using System;
using System.Web.Mvc;
using Bruttissimo.Common.Guard;
using Bruttissimo.Common.Mvc.Core.ActionResults.Json;
using Bruttissimo.Common.Mvc.Core.Controllers;
using Bruttissimo.Common.Resources;

namespace Bruttissimo.Common.Mvc.Core.ActionFilters
{
    public class AjaxTransformFilter : IActionFilter
    {
        private readonly string defaultTitle;

        public AjaxTransformFilter(string defaultTitle)
        {
            Ensure.That(() => defaultTitle).IsNotNull();
            this.defaultTitle = defaultTitle;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null || !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return; // this filter only cares about AJAX requests that successfully produced an ActionResult.
            }
            var controller = filterContext.Controller as StringRenderingController;
            if (controller == null)
            {
                throw new InvalidOperationException(Error.AjaxTransformAttributeDecoration);
            }

            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null) // view result AJAX transformation
            {
                string title = controller.ViewBag.Title ?? defaultTitle;
                string container = controller.ViewBag.AjaxViewContainer;
                SeparationOfConcernsResult view = controller.PartialViewSeparationOfConcerns(viewResult.ViewName, null, viewResult.Model);
                string html = view.Html;
                string script = view.JavaScript;

                AjaxViewJsonResult ajaxView = new AjaxViewJsonResult(title, html, script, container);
                filterContext.Result = ajaxView;
            }

            var redirectResult = filterContext.Result as RedirectResult;
            if (redirectResult != null)
            {
                filterContext.Result = new JsonRedirectResult(redirectResult);
            }
        }
    }
}
