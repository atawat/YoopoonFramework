using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using YooPoon.Core.Autofac;
using YooPoon.Core.Data;
using YooPoon.Core.Site;
using YooPoon.Data.EntityFramework;
using YooPoon.WebFramework.API;
using YooPoon.WebFramework.MVC;

namespace YooPoon.WebFramework.Dependency
{
    /// <summary>
    /// 依赖注入注册
    /// </summary>
    public class DependencyRegister:IDependencyRegister
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
                .InstancePerRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerRequest();

            //web helper
            //builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerRequest();

            var assemblies = typeFinder.GetAssemblies().ToArray();

            //controllers
            builder.RegisterControllers(assemblies);
            //ApiControllers
            builder.RegisterApiControllers(assemblies);

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

                builder.Register<IDbContext>(c => new EfDbContext(dataProviderSettings.DataConnectionString)).InstancePerRequest();
            }
            else
            {
                builder.Register<IDbContext>(c => new EfDbContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerRequest();
            }


            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof (IDependency).IsAssignableFrom(t) && t != typeof (IDependency))
                .AsImplementedInterfaces()
                .InstancePerRequest();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof (ISingletonDependency).IsAssignableFrom(t) && t != typeof (ISingletonDependency))
                .AsImplementedInterfaces()
                .SingleInstance();

            //IWorkContext
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerRequest();

            //filter
            builder.RegisterFilterProvider(); //todo：使用YpDependencyResolver时此方法会报空，用官方自带的不会
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            
            //注册YpHnadleError
            //全局注册
            builder.RegisterType<YpHandleErrorAttribute>().AsExceptionFilterFor<Controller>().SingleInstance();
            builder.RegisterType<YpAPIHandleErrorAttribute>().AsWebApiExceptionFilterFor<ApiController>().SingleInstance();
            //单个注册
            //builder.RegisterType<YpHandleErrorAttribute>().SingleInstance();
            //builder.RegisterType<YpAPIHandleErrorAttribute>().SingleInstance();

            //注册YpAuthorizeAttribute
            //全局注册
            builder.RegisterType<YpAPIAuthorizeAttribute>()
                .AsWebApiAuthorizationFilterFor<ApiController>()
                .PropertiesAutowired()
                .InstancePerRequest();
            builder.RegisterType<YpAuthorizeAttribute>()
                .AsAuthorizationFilterFor<Controller>()
                .PropertiesAutowired()
                .InstancePerRequest();
            //单个注册
            //builder.RegisterType<YpAPIAuthorizeAttribute>().PropertiesAutowired().InstancePerRequest();
            //builder.RegisterType<YpAuthorizeAttribute>().PropertiesAutowired().InstancePerRequest();
        }

        public int Order { get { return 0; } }
    }
}