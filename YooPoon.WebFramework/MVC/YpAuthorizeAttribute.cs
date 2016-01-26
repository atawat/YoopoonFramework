using System.Linq;
using System.Web;
using System.Web.Mvc;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.MVC
{
    public class YpAuthorizeAttribute : AuthorizeAttribute
    {
        private string _controllerName;
        private string _actionName;
        public IWorkContext WorkContext { get; set; }

        //public YpAuthorizeAttribute()
        //{

        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //var builder = new ContainerBuilder();
            //var containerManager = new ContainerManager(builder.Build());
            //WorkContext = containerManager.Resolve<IWorkContext>();
            //User = WorkContext.CurrentUser as UserBase;
            //获取 controller  名称        
            _controllerName = filterContext.RouteData.Values["controller"].ToString();
            //获取 action 名称      
            _actionName = filterContext.RouteData.Values["action"].ToString();
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = WorkContext.CurrentUser as UserBase;
            if (user == null)
            {
                httpContext.Response.StatusCode = 401;
                return false;
            }
            //用户权限判断
            var isAllowed = user.UserRoles.ToList()
                .Exists(
                    ur =>
                        ur.Role.RoleName == "superAdmin" ||
                        ur.Role.RolePermissions.ToList()
                            .Exists(
                                rp =>
                                    rp.IsAllowed && rp.ControllerAction.ActionName == _actionName &&
                                    rp.ControllerAction.ControllerName == _controllerName));

            if (!isAllowed)
            {
                httpContext.Response.StatusCode = 403;
                return false;
            }
            return true;
        }
    }
}