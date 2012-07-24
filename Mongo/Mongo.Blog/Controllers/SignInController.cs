using System.Web.Mvc;
using System.Web.Security;
using Mongo.Blog.Models;
using Mongo.Documents;
using Mongo.Repositories;
using MongoDB.Driver;

namespace Mongo.Blog.Controllers
{
    public class SignInController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFormsAuthenticator _formsAuthenticator;

        public SignInController()
        {
            _subscriptionRepository = new SubscriptionRepository(MongoServer.Create().GetDatabase("blog"));
            _formsAuthenticator = new FormsAuthenticator(); 
        }

        public SignInController(ISubscriptionRepository subscriptionRepository, IFormsAuthenticator formsAuthenticator)
        {
            _subscriptionRepository = subscriptionRepository;
            _formsAuthenticator = formsAuthenticator;
        }

        public ActionResult Index(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SignIn signIn, string returnUrl)
        {
            var subscription = _subscriptionRepository.GetSubscription(signIn.UserName, signIn.Password);
            if (subscription != null)
            {
                _formsAuthenticator.SetAuthCookie(subscription.UserName, false);

                if (returnUrl != null && Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Post");
            }
            return View(signIn);
        }

        public ActionResult LogOff()
        {
            _formsAuthenticator.SignOut();
            return RedirectToAction("Index", "Post");
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(Registration model)
        {
            if (ModelState.IsValid)
            {
                _subscriptionRepository.AddSubscription(new Subscription()
                                                            {
                                                                Email = model.Email,
                                                                UserName = model.UserName,
                                                                Password = model.Password
                                                            });

                FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                return RedirectToAction("Index", "Post");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}