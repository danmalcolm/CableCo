using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CableCo.Accounts.Commands;
using CableCo.Common.Logging;
using NHibernate;
using NHibernate.Linq;
using Rebus;
using log4net;

namespace CableCo.Accounts.WebApp.Controllers.Accounts
{
    public class AccountsController : ControllerBase
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();

        private readonly IBus bus;
        private readonly ISession session;

        public AccountsController(IBus bus, ISession session)
        {
            this.bus = bus;
            this.session = session;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new ListModel
                {
                    Accounts = session.Query<Account>().OrderBy(x => x.Code).ToList()
                };
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(string code)
        {
            var account = session.Query<Account>().SingleOrDefault(x => x.Code == code);
            if (account == null)
                return HttpNotFound();
            var subscribedProductCodes = account.Subscriptions.Select(x => x.ProductCode).ToList();
            var availableProducts = session.Query<Product>().Where(p => !subscribedProductCodes.Contains(p.Code)).ToList();
            
            var model = new DetailsModel
            {
                Account = account,
                AvailableProducts = availableProducts
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CreateAccountRequest request)
        {
            Log.InfoFormat("Sending message to create account: {0}", request.Code);
            bus.Send(new CreateAccount { AccountCode = request.Code, AccountName = request.Name });
            AlertSuccess("Requesting account creation. Please wait...");
            return this.RedirectToAction("Index");
        }
    }
}