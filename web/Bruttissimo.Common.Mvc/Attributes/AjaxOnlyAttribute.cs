using System;
using System.Reflection;
using System.Web.Mvc;

namespace Bruttissimo.Common.Mvc
{
	/// <summary>
	/// This method handles only AJAX requests. Non AJAX requests are ignored.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
	{
		/// <summary>
		/// Determines whether the action method selection is valid for the specified controller context.
		/// </summary>
		/// <returns>
		/// true if the action method selection is valid for the specified controller context; otherwise, false.
		/// </returns>
		/// <param name="controllerContext">The controller context.</param><param name="methodInfo">Information about the action method.</param>
		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			return controllerContext.HttpContext.Request.IsAjaxRequest();
		}
	}
}