using YooPoon.Core.Autofac;
using YooPoon.Framework.User.Entity;

namespace YooPoon.Framework.Authentication
{
    public interface IAuthenticationService:IDependency
    {
        void SignIn(UserBase customer, bool createPersistentCookie);
        void SignOut();
        UserBase GetAuthenticatedUser(); 
    }
}