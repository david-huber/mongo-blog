using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Mongo.Documents
{
    public class Subscription
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
