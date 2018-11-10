using System.Linq;
using System.Threading.Tasks;
using CableCo.Accounts.Events;
using CableCo.Common.Alerts;
using Rebus.Handlers;

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

        public async Task Handle(AccountCreated @event)
        {
            string message = string.Format("Account {0} has been created", @event.AccountCode);
            AddAlert(message);
        }

        public async Task Handle(SubscriptionsChanged @event)
        {
            string message = string.Format("Account {0} has changed subscriptions {1}", 
                @event.AccountCode, string.Join(",", @event.Subscriptions.Select(s => s.ProductCode)));
            AddAlert(message);
        }

        public async Task Handle(SubscriptionProvisioned @event)
        {
            string message = string.Format("Account {0}'s service {1} has been provisioned",
                @event.AccountCode, @event.ProductCode);
            AddAlert(message);
        }
    }
}