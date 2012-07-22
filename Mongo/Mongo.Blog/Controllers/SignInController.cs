using System.Web.Mvc;
using Mongo.Blog.Models;
using Mongo.Repositories;

namespace Mongo.Blog.Controllers
{
    public class SignInController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;       

        public SignInController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public ActionResult Index()
        {
            return View(new SignIn());
        }

        public ActionResult Authenticate(SignIn signIn)
        {
            var subscription = _subscriptionRepository.GetSubscription(signIn.UserName, signIn.Password);
            if (subscription != null)
            {
                return RedirectToAction("Index", "Post");
            }
            return View("Index", signIn);
        }
    }
}