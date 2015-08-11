using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac.Integration.WebApi;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.API
{
    public class YpAPIAuthorizeAttribute : AuthorizeAttribute, IAutofacAuthorizationFilter
    {
        public IWorkContext WorkContext { get; set; }

        private bool IsAllowed { get; set; }

        private UserBase User { get; set; }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            //            var builder = new ContainerBuilder();
            //            var containerManager = new ContainerManager(builder.Build());
            //            WorkContext = containerManager.Resolve<IWorkContext>();
            User = WorkContext.CurrentUser as UserBase;
            //用户权限判断
            //获取 controller  名称        
            var controllerName = filterContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            //获取 action 名称      
            var actionName = filterContext.ActionDescriptor.ActionName;
            if (User != null &&
                !User.UserRoles.ToList()
                    .Exists(
                        ur =>
                            ur.Role.RoleName == "superAdmin" ||
                            ur.Role.RolePermissions.ToList()
                                .Exists(
                                    rp =>
                                        rp.IsAllowed && rp.ControllerAction.ActionName == actionName &&
                                        rp.ControllerAction.ControllerName == controllerName)))
            {
                //filterContext.Response.StatusCode = HttpStatusCode.Forbidden;
                IsAllowed = false;
            }
            else
            {
                IsAllowed = true;
            }
            base.OnAuthorization(filterContext);
        }

        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            //httpContext.Response = httpContext.Request.CreateResponse();
            if (User == null)
                return false;
            if (!IsAllowed)
                return false;
            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse();
            if (User == null)
            {
                actionContext.Response.StatusCode = HttpStatusCode.Unauthorized;
            }
            else if (!IsAllowed)
            {
                actionContext.Response.StatusCode = HttpStatusCode.Forbidden;
            }
        }
    }
}