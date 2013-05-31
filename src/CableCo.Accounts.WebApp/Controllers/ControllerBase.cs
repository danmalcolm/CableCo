using System.Web.Mvc;
using CableCo.Accounts.WebApp.Common.Alerts;
using CableCo.Common.Alerts;

namespace CableCo.Accounts.WebApp.Controllers
{
    public class ControllerBase : Controller
    {
        public void AlertWarning(string message)
        {
            AddAlert(message, AlertType.Warning);
        }

        public void AlertSuccess(string message)
        {
            AddAlert(message, AlertType.Success);
        }

        public void Information(string message)
        {
            AddAlert(message, AlertType.Information);
        }

        public void AlertError(string message)
        {
            AddAlert(message, AlertType.Error);
        }

        private void AddAlert(string message, AlertType type)
        {
            var alert = Alert.Create(message, type);
            TempData.AddAlert(alert);
        }
    }
}