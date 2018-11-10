using System.Threading.Tasks;
using CableCo.Accounts.Events;
using CableCo.Common.Logging;
using log4net;
using Rebus.Handlers;

namespace CableCo.ProvisioningService.Handlers
{
    public class AccountCreatedHandler : IHandleMessages<AccountCreated>
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();

        public async Task Handle(AccountCreated @event)
        {
            
        }
    }
}