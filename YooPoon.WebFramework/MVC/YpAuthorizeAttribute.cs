using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication.Services;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.MVC
{
    public class YpAuthorizeAttribute : AuthorizeAttribute
    {
        private string _controllerName;
        private string _actionName;
        public IWorkContext WorkContext { get; set; }
        public IControllerActionService CaService { get; set; }
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
            if (WorkContext.CurrentUser == null)
            {
                httpContext.Response.StatusCode = 401;
                return false;
            }
            //用户权限判断
            var isAllowed = DoAuthorized(((UserBase)WorkContext.CurrentUser).UserRoles.ToList(), _controllerName, _actionName);

            if (isAllowed)
                return true;
            httpContext.Response.StatusCode = 403;
            return false;
        }

        private bool DoAuthorized(List<UserRole> userRoles, string controllerName, string actionName)
        {
            if (userRoles == null || userRoles.Count == 0)
                return false;
            if (userRoles.Exists(ur => ur.Role.RoleName == "superAdmin"))
                return true;
            var controllerAction = CaService.GetControllerActionByName(controllerName, actionName);
            if (controllerAction == null)
                return false;
            //            var rolePermission = userRoles.SelectMany(c => c.Role.RolePermissions).Select(c=>new{c.ControllerAction.Id,c.IsAllowed}).ToList();
            //            rolePermission = rolePermission.Where(p=>p.Id == controllerAction.Id).ToList();
            //            if (rolePermission.Exists(p=> p.IsAllowed))
            //                return true;
            var isAllowed = false;
            foreach (var role in userRoles)
            {
                if (role.Role.RolePermissions.Any(permission => permission.IsAllowed && permission.ControllerAction.Id == controllerAction.Id))
                {
                    isAllowed = true;
                }
                if (isAllowed)
                    break;
            }
            return isAllowed;
        }
    }
}