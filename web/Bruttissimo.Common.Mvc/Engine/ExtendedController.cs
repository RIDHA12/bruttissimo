using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Bruttissimo.Common.Mvc
{
	/// <summary>
	/// Our implementation of controller base.
	/// </summary>
	public abstract class ExtendedController : StringRenderingController
	{
		/// <summary>
		/// Returns the invalid model state messages, in either Json format for AJAX requests, or as a ViewResult for regular requests.
		/// </summary>
		protected ActionResult InvalidModelState(object model, bool throwOnEmptyModel = true)
		{
			if (throwOnEmptyModel && model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (ModelState.IsValid)
			{
				throw new ArgumentException(Resources.Error.ModelStateIsValid);
			}
			else if (Request.IsAjaxRequest())
			{
				return new ModelStateValidationJsonResult(ModelState);
			}
			else
			{
				return View(model);
			}
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
		/// <para>For regular action methods (which can either be AJAX or not), it's not necessary to use AjaxView, in those cases, use View() instead.</para>
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
		/// <para>For regular action methods (which can either be AJAX or not), it's not necessary to use AjaxView.</para>
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
		/// <para>For regular action methods (which can either be AJAX or not), it's not necessary to use AjaxView.</para>
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
	}
}