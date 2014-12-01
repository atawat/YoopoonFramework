using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using YooPoon.Core.Autofac;
using YooPoon.Core.Data;
using YooPoon.Core.Site;
using YooPoon.Data.EntityFramework;
using YooPoon.WebFramework.MVC;

namespace YooPoon.WebFramework.Dependency
{
    /// <summary>
    /// 依赖注入注册
    /// </summary>
    public class DependencyRegistrar:IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                //HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase)
                //(new FakeHttpContext("~/") as HttpContextBase)
                )
                .As<HttpContextBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerHttpRequest();

            //web helper
            //builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();

            var assemblies = typeFinder.GetAssemblies().ToArray();

            //controllers
            builder.RegisterControllers(assemblies);

            //View
            //builder.RegisterSource(new ViewRegistrationSource());

            //data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();


            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                //dataProvider.InitConnectionFactory();
                ((SqlServerDataProvider)dataProvider).InitDatabase();

                builder.Register<IDbContext>(c => new EfDbContext(dataProviderSettings.DataConnectionString)).InstancePerHttpRequest();
            }
            else
            {
                builder.Register<IDbContext>(c => new EfDbContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerHttpRequest();
            }


            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(assemblies).Where(t=>typeof(IDependency).IsAssignableFrom(t) && t != typeof(IDependency)).AsImplementedInterfaces().InstancePerRequest();

            //注册YpHnadleError
            builder.RegisterType<YpHandleErrorAttribute>().InstancePerRequest();

            //IWorkContext
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerHttpRequest();

            //builder.RegisterFilterProvider();  //TODO:无法通过autofac自带的方法来注册filter
        }

        public int Order { get { return 0; } }
    }
}