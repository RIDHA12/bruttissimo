using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Bruttissimo.Domain.Logic
{
	public class EmailTemplateService : TemplateService, IEmailTemplateService
	{
		public EmailTemplateService(TemplateServiceConfiguration configuration)
			: base(configuration)
		{
		}

		public EmailTemplateService()
		{
		}
	}
}