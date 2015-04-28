using System;
using System.Collections.Generic;
using System.Linq;

namespace YooPoon.Core.Autofac
{
    /// <summary>
    /// Configures the inversion of control container with services used by Nop.
    /// </summary>
    public class ContainerConfigurer
    {
        public virtual void Configure(ContainerManager containerManager)
        {
            //other dependencies
            //containerManager.AddComponentInstance<NopConfig>(configuration, "nop.configuration");
            //containerManager.AddComponentInstance<IEngine>(engine, "nop.engine");
            containerManager.AddComponentInstance<ContainerConfigurer>(this, "YooPoon.ContainerConfigurer");

            //type finder
            containerManager.AddComponent<ITypeFinder, WebAppTypeFinder>("YooPoon.TypeFinder");

            //register dependencies provided by other assemblies
            var typeFinder = containerManager.Resolve<ITypeFinder>();
            containerManager.UpdateContainer(x =>
            {
                var drTypes = typeFinder.FindClassesOfType<IDependencyRegister>();
                var drInstances = new List<IDependencyRegister>();
                foreach (var drType in drTypes)
                    drInstances.Add((IDependencyRegister)Activator.CreateInstance(drType));
                //sort
                drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
                foreach (var dependencyRegistrar in drInstances)
                    dependencyRegistrar.Register(x, typeFinder);
            });
        }
    }
}
