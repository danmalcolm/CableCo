using System.Collections.Generic;
using System.Web.Mvc;
using CableCo.Common.Alerts;

namespace CableCo.Accounts.WebApp.Common.Alerts
{
    public static class TempDataDictionaryExtensions
    {
        private const string AlertsKey = "alerts";

        public static List<Alert> GetAlerts(this TempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(AlertsKey))
            {
                tempData[AlertsKey] = new List<Alert>();
            }
            return (List<Alert>)tempData[AlertsKey];
        }

        public static bool HasAlerts(this TempDataDictionary tempData)
        {
            return tempData.ContainsKey(AlertsKey);
        }

        public static void AddAlert(this TempDataDictionary tempData, Alert alert)
        {
            var alerts = tempData.GetAlerts();
            alerts.Add(alert);
        }
    }
}