using CableCo.Accounts.Events;

namespace CableCo.Accounts
{
    public class Subscription : Entity
    {
        protected Subscription() {}

        public Subscription(Account account, string productCode)
        {
            Account = account;
            ProductCode = productCode;
            Status = SubscriptionStatus.AwaitingActivation;
        }

        public virtual Account Account { get; protected set; }
        public virtual string ProductCode { get; protected set; }
        public virtual SubscriptionStatus Status { get; protected set; }

        public virtual void MarkProvisioned()
        {
            if (Status != SubscriptionStatus.Provisioned)
            {
                Status = SubscriptionStatus.Provisioned;
                DomainEvents.Raise(new SubscriptionProvisioned { AccountCode = Account.Code, ProductCode = ProductCode });
            }
        }
    }
}