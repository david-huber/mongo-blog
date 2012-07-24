namespace Mongo.Blog.Controllers
{
    public interface IFormsAuthenticator
    {
        void SetAuthCookie(string userName, bool rememberMe);
        void SignOut();
    }
}