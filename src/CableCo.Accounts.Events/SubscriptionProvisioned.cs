namespace CableCo.Accounts.Events
{
    public class SubscriptionProvisioned : IDomainEvent
    {
        public string AccountCode { get; set; } 

        public string ProductCode { get; set; } 
    }
}