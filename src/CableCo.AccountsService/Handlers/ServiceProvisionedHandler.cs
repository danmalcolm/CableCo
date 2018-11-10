using System.Linq;
using System.Threading.Tasks;
using CableCo.Accounts;
using CableCo.Provisioning.Events;
using NHibernate;
using NHibernate.Linq;
using Rebus.Handlers;

namespace CableCo.AccountsService.Handlers
{
    public class ServiceProvisionedHandler : IHandleMessages<ProductProvisioned>
    {
        private readonly ISession session;

        public ServiceProvisionedHandler(ISession session)
        {
            this.session = session;
        }

        public async Task Handle(ProductProvisioned @event)
        {
            var account = session.Query<Account>().SingleOrDefault(x => x.Code == @event.AccountCode);
            account.ActivateSubscription(@event.ProductCode);
        }
    }
}