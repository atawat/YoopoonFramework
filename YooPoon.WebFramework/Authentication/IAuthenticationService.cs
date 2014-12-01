using YooPoon.Core.Autofac;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.Authentication
{
    public interface IAuthenticationService:IDependency
    {
        void SignIn(UserBase customer, bool createPersistentCookie);
        void SignOut();
        UserBase GetAuthenticatedUser(); 
    }
}