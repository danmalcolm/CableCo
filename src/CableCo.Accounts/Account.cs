using System;
using System.Collections.Generic;
using System.Linq;
using CableCo.Accounts.Events;
using CableCo.Common.Utility;

namespace CableCo.Accounts
{
    public class Account : Entity
    {
        protected Account()
        {
            Subscriptions = new List<Subscription>();
        }

        public Account(string code)
        {
            Code = code;
            DomainEvents.Raise(new AccountCreated { AccountCode = code });
        }

        public virtual string Code { get; set; }

        public virtual void ChangeSubscriptions(IEnumerable<Product> products)
        {
            var subscriptions = products.Select(p => new Subscription(this, p.Code));
            Subscriptions.Clear();
            subscriptions.Each(Subscriptions.Add);
            DomainEvents.Raise(new SubscriptionsChanged
            {
                AccountCode = Code,
                Subscriptions = subscriptions.Select(s => new SubscriptionInfo { ProductCode = s.ProductCode }).ToList()
            });
        }

        public virtual IList<Subscription> Subscriptions { get; protected set; } 

        public virtual void ActivateSubscription(string productCode)
        {
            var subscription = Subscriptions.SingleOrDefault(s => s.ProductCode == productCode);
            if(subscription == null)
                throw new ArgumentException("Not subscribed to " + productCode, "productCode");

            subscription.MarkProvisioned();
        }
    }
}