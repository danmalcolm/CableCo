using System.Linq;
using CableCo.Accounts;
using CableCo.Accounts.Commands;
using CableCo.Provisioning.Events;
using NHibernate;
using NHibernate.Linq;
using Rebus;

namespace CableCo.AccountsService.Handlers
{
    public class ServiceProvisionedHandler : IHandleMessages<ProductProvisioned>
    {
        private readonly ISession session;

        public ServiceProvisionedHandler(ISession session)
        {
            this.session = session;
        }

        public void Handle(ProductProvisioned @event)
        {
            var account = session.Query<Account>().SingleOrDefault(x => x.Code == @event.AccountCode);
            account.ActivateSubscription(@event.ProductCode);
        }
    }
}