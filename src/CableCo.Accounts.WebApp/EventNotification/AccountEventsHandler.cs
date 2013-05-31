using System.Linq;
using CableCo.Accounts.Events;
using CableCo.Common.Alerts;
using Rebus;

namespace CableCo.Accounts.WebApp.EventNotification
{
    public class AccountEventsHandler 
        : IHandleMessages<AccountCreated>,
        IHandleMessages<SubscriptionsChanged>,
        IHandleMessages<SubscriptionProvisioned>
    {
        private readonly AlertStore store;

        public AccountEventsHandler(AlertStore store)
        {
            this.store = store;
        }

        private void AddAlert(string message)
        {
            store.Add(Alert.Create(message, AlertType.Information));
        }

        public void Handle(AccountCreated @event)
        {
            string message = string.Format("Account {0} has been created", @event.AccountCode);
            AddAlert(message);
        }

        public void Handle(SubscriptionsChanged @event)
        {
            string message = string.Format("Account {0} has changed subscriptions {1}", 
                @event.AccountCode, string.Join(",", @event.Subscriptions.Select(s => s.ProductCode)));
            AddAlert(message);
        }

        public void Handle(SubscriptionProvisioned @event)
        {
            string message = string.Format("Account {0}'s service {1} has been provisioned",
                @event.AccountCode, @event.ProductCode);
            AddAlert(message);
        }
    }
}