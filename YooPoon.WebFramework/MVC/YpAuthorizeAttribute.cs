using System.Linq;
using System.Web;
using System.Web.Mvc;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.MVC
{
    public class YpAuthorizeAttribute : AuthorizeAttribute
    {
        public IWorkContext WorkContext { get; set; }

        private bool IsAllowed { get; set; }

        private UserBase User { get; set; }

        //public YpAuthorizeAttribute()
        //{

        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
//            var builder = new ContainerBuilder();
//            var containerManager = new ContainerManager(builder.Build());
//            WorkContext = containerManager.Resolve<IWorkContext>();
            User = WorkContext.CurrentUser as UserBase;
            //用户权限判断
            //获取 controller  名称        
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            //获取 action 名称      
            var actionName = filterContext.RouteData.Values["action"].ToString();
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
                //filterContext.HttpContext.Response.StatusCode = 403;
                IsAllowed = false;
            }
            else
            {
                IsAllowed = true;
            }
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //var user = WorkContext.CurrentUser as UserBase;
            if (User == null)
            {
                httpContext.Response.StatusCode = 401;
                return false;
            }
            if (!IsAllowed)
            {
                httpContext.Response.StatusCode = 403;
                return false;
            }
            return true;
        }
    }
}