using System.Web.Mvc;
using Autofac;
using YooPoon.Core.Autofac;
using YooPoon.Core.Site;

namespace YooPoon.WebFramework.MVC
{
    public abstract class YpWebViewPage<TModel> : WebViewPage<TModel>
    {
        private  IWorkContext _workContext { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            var builder = new ContainerBuilder();
            var containerManager = new ContainerManager(builder.Build());
            _workContext = containerManager.Resolve<IWorkContext>();
        }

        public IWorkContext WorkContext { get { return _workContext; }}
    }
}