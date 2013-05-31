using System.Collections.Generic;
using System.Linq;
using CableCo.Common.Alerts;

namespace CableCo.Accounts.WebApp.EventNotification
{
    /// <summary>
    /// Stores an application-wide list of alerts
    /// </summary>
    public class AlertStore
    {
        private readonly List<Alert> alerts = new List<Alert>();

        public void Add(Alert alert)
        {
             alerts.Add(alert);
        }

        public IEnumerable<Alert> Alerts
        {
            get 
            { 
                var items = alerts.ToList();
                items.Reverse();
                return items;
            }
        }




    }
}