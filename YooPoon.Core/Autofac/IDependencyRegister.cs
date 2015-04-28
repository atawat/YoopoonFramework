using Autofac;

namespace YooPoon.Core.Autofac
{
    public interface IDependencyRegister
    {
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);

        int Order { get; }
    }
}
