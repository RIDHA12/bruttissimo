using System;
using System.Web.Mvc;
using Bruttissimo.Common.Guard;
using Bruttissimo.Common.Interface;
using Bruttissimo.Common.InversionOfControl;
using Bruttissimo.Common.Mvc.Core.ActionResults;
using Bruttissimo.Common.Mvc.Core.ActionResults.Json;
using Bruttissimo.Common.Mvc.Core.Attributes;
using Bruttissimo.Common.Mvc.Interface;
using Bruttissimo.Common.Resources;

namespace Bruttissimo.Common.Mvc.Core.Controllers
{
    /// <summary>
    /// Our implementation of controller base.
    /// </summary>
    public abstract class ExtendedController : StringRenderingController
    {
		private readonly Lazy<IUrlHelper> urlHelperLazy;
		private readonly Lazy<IMapper> mapperLazy;

	    protected IUrlHelper urlHelper
	    {
		    get { return urlHelperLazy.Value; }
	    }

		protected IMapper mapper
		{
			get { return mapperLazy.Value; }
		}

		protected ExtendedController()
		{
			urlHelperLazy = IoC.Container.Resolve<Lazy<IUrlHelper>>();
			mapperLazy = IoC.Container.Resolve<Lazy<IMapper>>();
		}

	    /// <summary>
        /// Returns the invalid model state messages, in either Json format for AJAX requests, or as a ViewResult for regular requests.
        /// </summary>
        protected ActionResult InvalidModelState(object model, bool throwOnEmptyModel = true)
        {
            if (throwOnEmptyModel)
            {
                Ensure.That(() => model).IsNotNull();
            }
            Ensure.That(() => ModelState.IsValid).WithExtraMessage(Error.ModelStateIsValid).IsFalse();

            if (Request.IsAjaxRequest())
            {
                return new ModelStateValidationJsonResult(ModelState);
            }
	        return View(model);
        }

        /// <summary>
        /// Returns the default not found result, with an optional message and exception.
        /// </summary>
        protected NotFoundResult NotFound(string message = null, Exception exception = null)
        {
            return new NotFoundResult(message, exception);
        }

        /// <summary>
        /// Returns an AjaxViewJsonResult with the corresponding HTML and JavaScript for the provided view.
        /// <para>This method is intended to be used in action methods marked with <see cref="AjaxOnlyAttribute" />.</para>
        /// <para>For regular action methods (which can either be AJAX or not), it's not necessary to use AjaxView. Just invoke View instead.</para>
        /// <para>Keep in mind in AJAX scenarios you can still return View and target a container using ViewBag.AjaxViewContainer</para>
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="container">The client-side container where to place the AJAX view result.</param>
        protected AjaxViewJsonResult AjaxView(object model, string container = null)
        {
            AjaxViewJsonResult result = AjaxView(null, null, model, container);
            return result;
        }

        /// <summary>
        /// Returns an AjaxViewJsonResult with the corresponding HTML and JavaScript for the provided view.
        /// <para>This method is intended to be used in action methods marked with <see cref="AjaxOnlyAttribute" />.</para>
        /// <para>For regular action methods (which can either be AJAX or not), it's not necessary to use AjaxView. Just invoke View instead.</para>
        /// <para>Keep in mind in AJAX scenarios you can still return View and target a container using ViewBag.AjaxViewContainer</para>
        /// </summary>
        /// <param name="viewName">The name of the view to return.</param>
        /// <param name="model">The model.</param>
        /// <param name="container">The client-side container where to place the AJAX view result.</param>
        protected AjaxViewJsonResult AjaxView(string viewName, object model, string container = null)
        {
            AjaxViewJsonResult result = AjaxView(viewName, null, model, container);
            return result;
        }

        /// <summary>
        /// Returns an AjaxViewJsonResult with the corresponding HTML and JavaScript for the provided view.
        /// <para>This method is intended to be used in action methods marked with <see cref="AjaxOnlyAttribute" />.</para>
        /// <para>For regular action methods (which can either be AJAX or not), it's not necessary to use AjaxView. Just invoke View instead.</para>
        /// <para>Keep in mind in AJAX scenarios you can still return View and target a container using ViewBag.AjaxViewContainer</para>
        /// </summary>
        /// <param name="viewName">The name of the view to return.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="model">The model.</param>
        /// <param name="container">The client-side container where to place the AJAX view result.</param>
        protected AjaxViewJsonResult AjaxView(string viewName, string controller, object model, string container = null)
        {
            SeparationOfConcernsResult view = PartialViewSeparationOfConcerns(viewName, controller, model);
            string html = view.Html;
            string script = view.JavaScript;
            return new AjaxViewJsonResult(null, html, script, container);
        }

        /// <summary>
        /// Returns a PartialViewResult if the executing context is a child action, and a ViewResult otherwise.
        /// </summary>
        protected ActionResult FlexibleView(string viewName, object model)
        {
            if (ControllerContext.IsChildAction)
            {
                return PartialView(viewName, model);
            }
            else
            {
                return View(viewName, model);
            }
        }
    }
}
