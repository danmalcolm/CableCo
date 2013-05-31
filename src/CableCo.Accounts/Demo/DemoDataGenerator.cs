using NHibernate;

namespace CableCo.Accounts.Demo
{
    public class DemoDataGenerator
    {
        public void Create(ISession session)
        {
            var product1 = new Product
                {
                    Code = "prn001",
                    Name = "Executive Collection"
                };
            var product2 = new Product
            {
                Code = "sports001",
                Name = "Executive Sports Collection"
            };
            var product3 = new Product
            {
                Code = "sports002",
                Name = "Executive Sports Collection (Full)"
            };
            session.SaveOrUpdate(product1);
            session.SaveOrUpdate(product2);
            session.SaveOrUpdate(product3);

            var account1 = new Account("SWC000001");
            session.SaveOrUpdate(account1);

            var account2 = new Account("SWC000002");
            session.SaveOrUpdate(account2);

            var account3 = new Account("SWC000003");
            session.SaveOrUpdate(account3);
        }
    }
}