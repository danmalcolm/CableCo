using System.Collections.Generic;

namespace CableCo.Accounts.Commands
{
    public class ChangeSubscriptions
    {
        public string AccountCode { get; set; }

        public List<string> ProductCodes { get; set; }
    }
}