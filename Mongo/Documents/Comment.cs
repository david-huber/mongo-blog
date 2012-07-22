using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Mongo.Documents
{
    public class Comment
    {

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime Posted { get; set; }
        public string Content { get; set; }

    }
}
