using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CableCo.Common.Alerts;

namespace CableCo.Accounts.WebApp.EventNotification
{
    public class AlertsController : ApiController
    {
        private readonly AlertStore store;

        public AlertsController(AlertStore store)
        {
            this.store = store;
        }

        // GET api/<controller>
        public IEnumerable<Alert> Get()
        {
            return store.Alerts.Take(20);
        }
    }
}