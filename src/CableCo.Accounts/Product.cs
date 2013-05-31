namespace CableCo.Accounts
{
    public class Product : Entity
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}