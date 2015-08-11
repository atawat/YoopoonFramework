using System.Net;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using YooPoon.Core.Logging;

namespace YooPoon.WebFramework.API
{
    public class YpAPIHandleErrorAttribute : ExceptionFilterAttribute,IAutofacExceptionFilter
    {
        private readonly ILog _log;

        public YpAPIHandleErrorAttribute(ILog log)
        {
            _log = log;
        }
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            var exception = filterContext.Exception;
            var controllerName = filterContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionContext.ActionDescriptor.ActionName;
            _log.Debug(exception, string.Format("控制器:{0}中的{1}—Action出错", controllerName, actionName));
            //filterContext.Response.StatusCode = HttpStatusCode.InternalServerError;
            base.OnException(filterContext);
        }
    }
}