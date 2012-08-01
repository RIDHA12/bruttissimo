using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace Bruttissimo.Common.Mvc
{
	public class WindsorControllerFactory : DefaultControllerFactory
	{
		private readonly IKernel kernel;

		public WindsorControllerFactory(IKernel kernel)
		{
			if (kernel == null)
			{
				throw new ArgumentNullException("kernel");
			}
			this.kernel = kernel;
		}

		public override void ReleaseController(IController controller)
		{
			kernel.ReleaseComponent(controller);
		}

		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			if (controllerType == null)
			{
				throw new HttpException((int)HttpStatusCode.NotFound, Resources.Error.ControllerNotFound.FormatWith(requestContext.HttpContext.Request.Path));
			}
			return (IController)kernel.Resolve(controllerType); // this also resolves the IActionInvoker.
		}
	}
}