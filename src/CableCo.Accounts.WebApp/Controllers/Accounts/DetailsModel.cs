using System.Collections.Generic;

namespace CableCo.Accounts.WebApp.Controllers.Accounts
{
    public class DetailsModel
    {
        public Account Account { get; set; }

        public List<Product> AvailableProducts { get; set; } 
    }
}