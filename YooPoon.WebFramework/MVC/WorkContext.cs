using System;
using System.Web;
using YooPoon.Core.Data;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication;

namespace YooPoon.WebFramework.MVC
{
    public class WorkContext : IWorkContext
    {
        private IUser _cacheduUser;
        private readonly IAuthenticationService _authenticationService;
        private readonly HttpContextBase _httpContext;
        private const string UserCookieName = "Yp.User";  //TODO:修改为依据setting配置的cookie名称

        public WorkContext(IAuthenticationService authenticationService,HttpContextBase httpContext)
        {
            _authenticationService = authenticationService;
            _httpContext = httpContext;
        }

        public IUser CurrentUser
        {
            get
            {
                if (_cacheduUser != null)
                    return _cacheduUser;
                var user = _authenticationService.GetAuthenticatedUser();
                _cacheduUser = user;
                if(user!=null)
                SetUserCookie(user.UserName);
                return _cacheduUser;
            }
            set
            {
                SetUserCookie(value.UserName);
                _cacheduUser = value;
            }
        }

        protected virtual void SetUserCookie(string userName)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(UserCookieName)
                {
                    HttpOnly = true,
                    Value = HttpUtility.UrlEncode(userName)
                };
                ;
                if (string.IsNullOrEmpty(userName))
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    const int cookieExpires = 24 * 365; //TODO:改为可配置的
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(UserCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }
    }
}