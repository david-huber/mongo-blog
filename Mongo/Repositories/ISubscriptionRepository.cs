using Mongo.Documents;

namespace Mongo.Repositories
{
    public interface ISubscriptionRepository
    {
        Subscription GetSubscription(string user, string password);
    }
}
