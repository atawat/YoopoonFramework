using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YooPoon.Core.Autofac;

namespace YooPoon.WebFramework.Dependency
{
    public class YpDependencyResolver : IDependencyResolver
    {
        private readonly ContainerManager _containerManager;

        public YpDependencyResolver(InitializeContainer initializeContainer)
        {
            _containerManager = initializeContainer.ContainerManager;
        }

        public object GetService(Type serviceType)
        {
            return _containerManager.ResolveOptional(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return (IEnumerable<object>)_containerManager.Resolve(type);
        }
    }
}