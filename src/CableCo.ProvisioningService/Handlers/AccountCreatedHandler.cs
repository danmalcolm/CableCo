using CableCo.Accounts.Events;
using CableCo.Common.Logging;
using Rebus;
using log4net;

namespace CableCo.ProvisioningService.Handlers
{
    public class AccountCreatedHandler : IHandleMessages<AccountCreated>
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();

        public void Handle(AccountCreated @event)
        {
            
        }
    }
}