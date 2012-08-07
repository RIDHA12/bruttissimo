using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Bruttissimo.Common.Mvc
{
	/// <summary>
	/// Registers all internal component dependencies, such as utility classes.
	/// </summary>
	public class UtilityInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component
					.For<FileSystemHelper>()
					.ImplementedBy<FileSystemHelper>()
					.LifestyleTransient()
			);
			
			container.Register(
				Component
					.For<HashProvider>()
					.ImplementedBy<HashProvider>()
					.LifestyleTransient()
			);

			container.Register(
				Component
					.For<HttpHelper>()
					.ImplementedBy<HttpHelper>()
					.LifestyleTransient()
			);

			container.Register(
				Component
					.For<TextHelper>()
					.ImplementedBy<TextHelper>()
					.LifestyleTransient()
			);
			
			container.Register(
				Component
					.For<ResourceCompressor>()
					.ImplementedBy<ResourceCompressor>()
					.LifestyleTransient()
			);

			container.Register(
				Component
					.For<ExceptionHelper>()
					.ImplementedBy<ExceptionHelper>()
					.LifestyleTransient()
			);
			
			container.Register(
				Component
					.For<RequestSanitizer>()
					.ImplementedBy<RequestSanitizer>()
					.LifestyleTransient()
			);
		}
	}
}