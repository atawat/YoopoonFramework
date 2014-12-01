using System.Web.Mvc;
using YooPoon.Core.Logging;

namespace YooPoon.WebFramework.MVC
{
    public class YpHandleErrorAttribute:HandleErrorAttribute
    {
        private readonly ILog _log;

        public YpHandleErrorAttribute(ILog log)
        {
            _log = log;
        }
        public override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            var controllerName = filterContext.RequestContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();
            _log.Debug(exception,string.Format("控制器:{0}中的{1}—Action出错",controllerName,actionName));
        }
    }
}
