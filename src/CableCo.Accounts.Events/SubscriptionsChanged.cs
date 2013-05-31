using System.Collections.Generic;

namespace CableCo.Accounts.Events
{
    public class SubscriptionsChanged : IDomainEvent
    {
        public string AccountCode { get; set; }

        public List<SubscriptionInfo> Subscriptions { get; set; }
    }
}