using System.Web.Mvc;
using Mongo.Blog.Controllers;
using Mongo.Blog.Models;
using Mongo.Documents;
using Moq;
using NUnit.Framework;
using Mongo.Repositories;

namespace Mongo.Tests.Unit
{
    [TestFixture]
    public class SignInControllerTests
    {
        private SignInController _controller;
        private Mock<ISubscriptionRepository> _repo;
        private Mock<IFormsAuthenticator> _formsAuthenticator;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<ISubscriptionRepository>();
            _formsAuthenticator = new Mock<IFormsAuthenticator>();
            _controller = new SignInController(_repo.Object, _formsAuthenticator.Object);
        }

        [Test]
        public void IndexReturnsViewOfSignIn()
        {

            var viewResult = _controller.Index(null) as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.EqualTo(""));
        }

        [Test]
        public void PostIndexReturnsNullReturnsIndexView()
        {
            _repo.Setup(r => r.GetSubscription("User", "Password")).Returns(() => null);
            var signIn = new SignIn
                             {
                                 UserName = "User",
                                 Password = "Password"
                             };
            var result = _controller.Index(signIn, null) as ViewResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetModelFromViewResult<SignIn>(), Is.EqualTo(signIn));
            Assert.That(result.ViewName, Is.EqualTo(""));
        }

        [Test]
        public void PostIndexUserWithGoodCredentialsRedirectsToPostIndex()
        {
            _repo.Setup(r => r.GetSubscription("User", "Password"))
                .Returns(() => new Subscription
                                {
                                    UserName = "User"
                                });
            var signIn = new SignIn
            {
                UserName = "User",
                Password = "Password"
            };
            var result = _controller.Index(signIn, null) as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Post"));
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            _formsAuthenticator.Verify(f => f.SetAuthCookie("User", false));
            
        }

    }
}
