using CableCo.Common.Windsor;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace CableCo.AccountsService.Windsor
{
    public class ContainerInitialiser
    {
        /// <summary>
        /// Creates Windsor Container and installs components via all installers within application projects
        /// </summary>
        /// <returns></returns>
        public WindsorContainer Create()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.InDirectory(ApplicationAssemblyHelper.AssemblyFilter));
            return container;
        }
    }
}