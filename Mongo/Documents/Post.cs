using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Mongo.Documents
{
    public class Post
    {

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime Posted { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
