using System;
using System.Web.Mvc;
using Bruttissimo.Common.Mvc.Core.ActionResults;

namespace Bruttissimo.Common.Mvc.Core.Attributes
{
    /// <summary>
    /// AuthorizeAttribute implementation with a slight tweak that makes authenticated
    /// but unauthorized requests to return Not Found results, enhancing security.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExtendedAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            bool authenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
            if (!authenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new NotFoundResult();
            }
        }
    }
}
