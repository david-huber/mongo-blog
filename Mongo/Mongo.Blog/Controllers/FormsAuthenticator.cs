using System.Web.Security;

namespace Mongo.Blog.Controllers
{
    public class FormsAuthenticator : IFormsAuthenticator
    {
        public void SetAuthCookie(string userName, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(userName, rememberMe);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}