namespace CableCo.Accounts.Events
{
    public class AccountCreated : IDomainEvent
    {
        public string AccountCode { get; set; }     
    }
}