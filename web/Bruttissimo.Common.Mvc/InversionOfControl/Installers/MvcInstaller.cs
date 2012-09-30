using Bruttissimo.Common.Guard;
using Bruttissimo.Common.InversionOfControl.Installers;
using Bruttissimo.Common.Mvc.InversionOfControl.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Bruttissimo.Common.Mvc.InversionOfControl.Installers
{
    /// <summary>
    /// Registers core MVC-specific dependencies.
    /// </summary>
    public sealed class MvcInstaller : IWindsorInstaller
    {
        private readonly MvcInstallerParameters parameters;

        /// <summary>
        /// Installs all required components and dependencies for the Mvc infrastructure package.
        /// </summary>
        public MvcInstaller(MvcInstallerParameters parameters)
        {
            Ensure.That(() => parameters).IsNotNull();

            this.parameters = parameters;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(
                new CommonInstaller(parameters.JobAssembly, parameters.MapperAssemblies),
                new AspNetInstaller(),
                new HttpModuleInstaller(),
                new MvcComponentInstaller(),
                new MvcControllerInstaller(parameters),
                new MvcModelBinderInstaller(parameters.ModelAssembly),
                new MvcModelValidatorInstaller(parameters.ModelAssembly),
                new MvcUtilityInstaller(),
                new MvcViewInstaller(parameters.ViewAssembly),
                new SquishItInstaller(),
                new SignalRInstaller(parameters.HubAssembly)
            );
        }
    }
}
