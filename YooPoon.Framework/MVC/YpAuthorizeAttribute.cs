using System.Linq;
using System.Web.Mvc;
using Autofac;
using YooPoon.Core.Autofac;
using YooPoon.Core.Site;
using YooPoon.Framework.User.Entity;

namespace YooPoon.Framework.MVC
{
    public class YpAuthorizeAttribute : AuthorizeAttribute
    {
        public IWorkContext WorkContext { get; set; }

        private bool IsAllowed { get; set; }

        public YpAuthorizeAttribute()
        {
            var builder = new ContainerBuilder();
            var containerManager = new ContainerManager(builder.Build());
            WorkContext = containerManager.Resolve<IWorkContext>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = WorkContext.CurrentUser as UserBase;
            //用户权限判断
            //获取 controller  名称        
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            //获取 action 名称      
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (user != null &&
                !user.UserRoles.ToList()
                    .Exists(
                        ur =>
                            ur.Role.RolePermissions.ToList()
                                .Exists(
                                    rp =>
                                        rp.IsAllowed && rp.ControllerAction.ActionName == actionName &&
                                        rp.ControllerAction.ControllerName == controllerName)))
            {
                filterContext.HttpContext.Response.StatusCode = 403;
                IsAllowed = false;
                //Todo:添加跳转页
            }
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var user = WorkContext.CurrentUser as UserBase;
            if (user == null)
                return false;
            if (!IsAllowed)
                return false;
            return true;
        }
    }
}