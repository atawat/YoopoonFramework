using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using YooPoon.Core.Autofac;

namespace YooPoon.WebFramework.Dependency
{
    public class YpWebApiDependencyResolver:IDependencyResolver
    {
        private readonly ContainerManager _containerManager;

        public YpWebApiDependencyResolver(InitializeContainer initializeContainer)
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

        public IDependencyScope BeginScope()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}