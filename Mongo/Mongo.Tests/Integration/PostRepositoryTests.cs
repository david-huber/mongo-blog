using System;
using System.Collections.Generic;
using System.Linq;
using Mongo.Documents;
using Mongo.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;

namespace Mongo.Tests.Integration
{
    [TestFixture, Category("Integration")]
    public class PostRepositoryTests : MongoTests
    {
        private MongoDatabase _db;

        [SetUp]
        public void SetUp()
        {
            var mongo = MongoServer.Create();
            _db = mongo.GetDatabase("test");
        }

        [Test]
        public void InsertAddsAPost()
        {
            var repository = new PostRepository(_db);            
            repository.Insert(new Post
                                  {
                                      Title = "The Loneliest Title"
                                  });
            var posts = (from p in _db.GetCollection<Post>("posts").AsQueryable<Post>()
                        where p.Title == "The Loneliest Title"
                        select p).ToArray();
            Assert.That(posts, Has.Exactly(1).Not.Null);
            Assert.That(posts, Has.Exactly(0).Null);
            Assert.That(posts[0].Title, Is.EqualTo("The Loneliest Title"));
            
        }

        [Test]
        public void InsertNullThrowsArgumentNullException()
        {
            var repository = new PostRepository(_db);            
            Assert.That(() => repository.Insert(null), Throws.InstanceOf<ArgumentNullException>());            
        }

        [Test]
        public void FindReturnsAllInsertedNodes()
        {
            var posts = new List<Post>
                            {
                                new Post {Title = "First"},
                                new Post {Title = "Second"},
                                new Post {Title = "Third"}
                            };
            _db.GetCollection<Post>("posts").InsertBatch(posts);
            var repository = new PostRepository(_db);
            var result = repository.Find().ToArray();
            Assert.That(posts, Has.Exactly(3).Not.Null);
            Assert.That(posts, Has.Exactly(0).Null);
            Assert.That(posts, Has.Exactly(1).Matches<Post>(p => p.Title == "First"));
            Assert.That(posts, Has.Exactly(1).Matches<Post>(p => p.Title == "Second"));
            Assert.That(posts, Has.Exactly(1).Matches<Post>(p => p.Title == "Third"));
        }

        [Test]
        public void AddCommentAddsComment()
        {
            var post = new Post
                           {
                               Title = "The Not Lonely Post"
                           };
            _db.GetCollection<Post>("posts").Insert(post);
            var repository = new PostRepository(_db);
            repository.AddComment(post, new Comment
                                            {
                                                Content = "Here I am!"
                                            });
            var posts = (from p in _db.GetCollection<Post>("posts").AsQueryable<Post>()
                          where p.Title == "The Not Lonely Post"
                          select p).ToArray();
            Assert.That(posts, Has.Exactly(1).Not.Null);
            Assert.That(posts, Has.Exactly(0).Null);
            var comments = posts[0].Comments;
            Assert.That(comments, Is.Not.Null);            
            Assert.That(comments, Has.Exactly(1).Not.Null);
            Assert.That(comments, Has.Exactly(0).Null);
            Assert.That(comments[0].Content, Is.EqualTo("Here I am!"));

        }

        [Test]
        public void AddCommentToUnsavedPostThrowsInvalidOperationException()
        {
            var post = new Post
            {
                Title = "The Not Lonely Post"
            };
            var repository = new PostRepository(_db);
            Assert.That(() => repository.AddComment(post, new Comment
                                            {
                                                Content = "Here I am!"
                                            }), Throws.InvalidOperationException);
        }

    }
}
