using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mongo.Documents;
using Mongo.Repositories;
using MongoDB.Driver;

namespace Mongo.Blog.Controllers
{
    public class PostController : Controller
    {
        private PostRepository _postRepository
            ;

        public PostController()
        {
            _postRepository = new PostRepository(MongoServer.Create().GetDatabase("blog"));
        }

        public ActionResult List()
        {
            var posts = _postRepository.Find();
            return View(posts.OrderByDescending(x => x.Posted).ToList());
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string id)
        {
            var post = (from p in _postRepository.Find() where p.Id == id select p).FirstOrDefault();
            return View(post);
        }

        public ActionResult AddComment(Comment comment, string postId)
        {
            var post = (from p in _postRepository.Find() where p.Id == postId select p).FirstOrDefault();
            if (post != null)
            {
                _postRepository.AddComment(post, comment);                
            }           

            return View("Detail", post);
        }



        public ActionResult Add(Post post)
        {
            // must validate the post 
            if (!String.IsNullOrEmpty(post.Title) && !String.IsNullOrEmpty(post.Content))
            {       // save the post 

                post.Posted = DateTime.Now;

                _postRepository.Insert(post);

                // list the post after save 
                return RedirectToAction("List");
            }

            return View();
        }

    }
}