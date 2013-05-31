using System.Linq;
using System.Web.Mvc;
using CableCo.Accounts.Commands;
using CableCo.Accounts.WebApp.Controllers.Accounts;
using CableCo.Common.Logging;
using Microsoft.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using Rebus;
using log4net;

namespace CableCo.Accounts.WebApp.Controllers.AccountSubscriptions
{
    public class AccountSubscriptionsController : ControllerBase
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();

        private readonly IBus bus;
        private readonly ISession session;

        public AccountSubscriptionsController(IBus bus, ISession session)
        {
            this.bus = bus;
            this.session = session;
        }
        
        [HttpPost]
        public ActionResult Index(AddSubscriptionsRequest request)
        {
            var account = session.Query<Account>().SingleOrDefault(x => x.Code == request.Code);
            if (account == null)
                return HttpNotFound();
            var command = new ChangeSubscriptions
            {
                AccountCode = account.Code,
                ProductCodes = request.ProductCodes
            };

            Log.InfoFormat("Sending message to add subscriptions to account: {0}", request.Code);
            bus.Send(command);
            AlertSuccess("Requesting subscription setup. Please wait...");
            return this.RedirectToAction((AccountsController c) => c.Index());
        }
    }
}