using System.Web.Mvc;

namespace Bruttissimo.Common.Mvc
{
    public static class IViewExtensions
    {
        public static string GetViewPath(this IView view)
        {
            BuildManagerCompiledView buildManagerCompiledView = view as BuildManagerCompiledView;
            if (buildManagerCompiledView == null)
            {
                return null;
            }
            else
            {
                return buildManagerCompiledView.ViewPath;
            }
        }
    }
}
