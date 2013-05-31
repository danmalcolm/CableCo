using System.Threading;

namespace CableCo.Common.Logging
{
    public class UserNameLogProperty
    {
        public override string ToString()
        {
            return Thread.CurrentPrincipal.Identity.Name;
        }
    }
}