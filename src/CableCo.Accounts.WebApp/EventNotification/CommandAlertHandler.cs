using System.Threading.Tasks;
using CableCo.Accounts.Commands;
using CableCo.Common.Alerts;
using Rebus.Handlers;

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

        public async Task Handle(CommandAlert alert)
        {
            store.Add(Alert.Create(alert.Message, alert.Type));
        }
    }
}