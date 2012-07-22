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

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<ISubscriptionRepository>();
            _controller = new SignInController(_repo.Object);
        }

        [Test]
        public void IndexReturnsViewOfSignIn()
        {
            
            var model = _controller.Index().GetModelFromViewResult<SignIn>();                
            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void AuthenticateReturnsNullReturnsIndexView()
        {
            _repo.Setup(r => r.GetSubscription("User", "Password")).Returns(() => null);
            var signIn = new SignIn
                             {
                                 UserName = "User",
                                 Password = "Password"
                             };
            var result = _controller.Authenticate(signIn) as ViewResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetModelFromViewResult<SignIn>(), Is.EqualTo(signIn));
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void AuthenticatedUserWithGoodCredentialsRedirectsToIndex()
        {
            _repo.Setup(r => r.GetSubscription("User", "Password")).Returns(() => new Subscription());
            var signIn = new SignIn
            {
                UserName = "User",
                Password = "Password"
            };
            var result = _controller.Authenticate(signIn) as RedirectToRouteResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Post"));
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            
        }

    }
}
