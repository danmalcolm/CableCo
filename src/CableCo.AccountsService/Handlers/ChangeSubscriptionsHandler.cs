using System.Linq;
using System.Threading.Tasks;
using CableCo.Accounts;
using CableCo.Accounts.Commands;
using CableCo.Common.Logging;
using NHibernate;
using NHibernate.Linq;
using log4net;
using Rebus.Bus;
using Rebus.Handlers;

namespace CableCo.AccountsService.Handlers
{
    public class ChangeSubscriptionsHandler : IHandleMessages<ChangeSubscriptions>
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();
        private readonly IBus bus;
        private readonly ISession session;

        public ChangeSubscriptionsHandler(IBus bus, ISession session)
        {
            this.bus = bus;
            this.session = session;
        }

        public async Task Handle(ChangeSubscriptions message)
        {
            Log.InfoFormat("Handling ChangeSubscriptions: {0}", message.AccountCode);

            var account = session.Query<Account>().SingleOrDefault(x => x.Code == message.AccountCode);
            var productCodes = account.Subscriptions.Select(x => x.ProductCode).ToList();
            productCodes.AddRange(message.ProductCodes);
            var products = session.Query<Product>().Where(x => productCodes.Contains(x.Code)).ToList();
            account.ChangeSubscriptions(products);
            session.SaveOrUpdate(account);
        }
    }
}