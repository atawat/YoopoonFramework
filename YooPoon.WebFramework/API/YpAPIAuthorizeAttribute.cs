using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac.Integration.WebApi;
using YooPoon.Core.Data;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication.Services;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.API
{
    public class YpAPIAuthorizeAttribute : AuthorizeAttribute, IAutofacAuthorizationFilter
    {
        private string _actionName;
        private string _controllerName;
        public IWorkContext WorkContext { get; set; }

        public IControllerActionService CaService { get; set; }

        private bool IsAllowed { get; set; }

        //private IUser User { get; set; }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            //            var builder = new ContainerBuilder();
            //            var containerManager = new ContainerManager(builder.Build());
            //            WorkContext = containerManager.Resolve<IWorkContext>();
            //User = WorkContext.CurrentUser;
            //用户权限判断
            //获取 controller  名称        
            _controllerName = filterContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            //获取 action 名称      
            _actionName = filterContext.ActionDescriptor.ActionName;


            base.OnAuthorization(filterContext);
        }

        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            //httpContext.Response = httpContext.Request.CreateResponse();
            if (WorkContext.CurrentUser == null)
                return false;
            IsAllowed = DoAuthorized(((UserBase)WorkContext.CurrentUser).UserRoles.ToList(), _controllerName, _actionName);
            return IsAllowed;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse();
            if (WorkContext.CurrentUser == null)
            {
                actionContext.Response.StatusCode = HttpStatusCode.Unauthorized;
            }
            else if (!IsAllowed)
            {
                actionContext.Response.StatusCode = HttpStatusCode.Forbidden;
            }
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