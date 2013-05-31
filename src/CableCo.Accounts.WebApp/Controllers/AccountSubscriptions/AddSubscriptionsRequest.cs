using System.Collections.Generic;

namespace CableCo.Accounts.WebApp.Controllers.AccountSubscriptions
{
    public class AddSubscriptionsRequest
    {
        public AddSubscriptionsRequest()
        {
            ProductCodes = new List<string>();
        }

        public string Code { get; set; }

        public List<string> ProductCodes { get; set; }
    }
}