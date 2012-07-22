using System;
using System.Collections.Generic;
using System.Linq;
using Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Mongo.Repositories
{
    public class PostRepository
    {
        private MongoCollection<Post> _collection;

        public PostRepository(MongoDatabase db)
        {
            _collection = db.GetCollection<Post>("posts");
        }

        public void Insert(Post post)
        {
           _collection.Insert(post);            
        }

        public IQueryable<Post> Find()
        {
            return _collection.AsQueryable<Post>();
        }

        public void AddComment(Post post, Comment comment)
        {
            if (post.Id == null)
            {
                throw new InvalidOperationException("Post has no ID");
            }
            if (post.Comments == null)
            {
                post.Comments = new List<Comment>();
            }
            post.Comments.Add(comment);
            _collection.Save(post);
        }
    }
}
