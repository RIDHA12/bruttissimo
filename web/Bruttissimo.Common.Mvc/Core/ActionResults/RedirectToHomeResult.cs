using System.Web.Mvc;
using System.Web.Routing;
using Bruttissimo.Common.Guard;

namespace Bruttissimo.Common.Mvc
{
    public sealed class RedirectToHomeResult : RedirectToRouteResult
    {
        public RedirectToHomeResult(string action = "Index", string controller = "Home")
            : base(CreateRouteValueDictionary(action, controller))
        {
        }

        private static RouteValueDictionary CreateRouteValueDictionary(string action, string controller)
        {
            Ensure.That(action, "action").IsNotNull();
            Ensure.That(controller, "controller").IsNotNull();

            RouteValueDictionary dictionary = new RouteValueDictionary
            {
                {"action", action},
                {"controller", controller}
            };
            return dictionary;
        }
    }
}
