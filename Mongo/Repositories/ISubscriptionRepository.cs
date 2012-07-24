using System.Linq;
using Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Mongo.Repositories
{
    public interface ISubscriptionRepository
    {
        Subscription GetSubscription(string user, string password);
        void AddSubscription(Subscription subscription);
    }

    public class SubscriptionRepository : ISubscriptionRepository
    {
        private MongoCollection<Subscription> _collection;

        public SubscriptionRepository(MongoDatabase db)
        {
            _collection = db.GetCollection<Subscription>("subscriptions");            
        }

        public Subscription GetSubscription(string userName, string password)
        {
            return _collection.AsQueryable<Subscription>()
                .FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }

        public void AddSubscription(Subscription subscription)
        {
            _collection.Insert(subscription);
        }
    }
}
