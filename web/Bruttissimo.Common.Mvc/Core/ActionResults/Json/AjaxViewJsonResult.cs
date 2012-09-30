using System.Web.Mvc;
using Bruttissimo.Common.Guard;

namespace Bruttissimo.Common.Mvc.Core.ActionResults.Json
{
    /// <summary>
    /// Returns a view expressed as a JSON result.
    /// </summary>
    public sealed class AjaxViewJsonResult : JsonResult
    {
        /// <summary>
        /// Returns a view expressed as a JSON result.
        /// </summary>
        public AjaxViewJsonResult(string title, string html, string script, string container = null)
        {
            Ensure.That(() => html).IsNotNull();

            Data = new
            {
                partial = true,
                title,
                html,
                script,
                container
            };
        }

        /// <summary>
        /// Overload used for targetting a particular container for a fragment of HTML.
        /// </summary>
        public AjaxViewJsonResult(string html, string container = null)
            : this(null, html, null, container)
        {
        }
    }
}
