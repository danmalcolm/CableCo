using System.Linq;
using CableCo.Accounts.Commands;
using CableCo.Accounts.Events;
using CableCo.Common.Alerts;
using Rebus;

namespace CableCo.Accounts.WebApp.EventNotification
{
    public class CommandAlertHandler 
        : IHandleMessages<CommandAlert>
    {
        private readonly AlertStore store;

        public CommandAlertHandler(AlertStore store)
        {
            this.store = store;
        }

        public void Handle(CommandAlert alert)
        {
            store.Add(Alert.Create(alert.Message, alert.Type));
        }
    }
}