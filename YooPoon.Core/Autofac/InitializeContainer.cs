using Autofac;

namespace YooPoon.Core.Autofac
{
    public class InitializeContainer
    {
        private readonly ContainerBuilder _builder;
        private ContainerManager _containerManager;

        public InitializeContainer()
        {
            _builder = new ContainerBuilder();
        }

        public void Initializing()
        {
            var containerManager = new ContainerManager(_builder.Build());
            _containerManager = containerManager;
            var configurer = new ContainerConfigurer();
            configurer.Configure(containerManager);
        }

        public ContainerBuilder Builder { get { return _builder; } }

        public ContainerManager ContainerManager { get { return _containerManager; } }
    }
}