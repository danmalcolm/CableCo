using System.Collections.Generic;
using System.Linq;
using CableCo.Accounts;
using NUnit.Framework;

namespace CableCo.Tests.Accounts.NHibernate
{
    public class AccountPersistenceTests : DatabaseTests
    {
        [Test]
        public void can_persist_and_retrieve_product()
        {
            var original = new Account("acc-1");
            original.ChangeSubscriptions(new List<Product>
                    {
                        new Product { Code = "sports-001" }, new Product { Code = "films-001" }
                    });
            InTransaction(session => session.Save(original));

            Account retrieved = null;
            InTransaction(session =>
            {
                retrieved = session.Get<Account>(original.Id);
                Assert.IsNotNull(retrieved);
                Assert.AreEqual(original.Code, retrieved.Code);
                CollectionAssert.AreEquivalent(original.Subscriptions.Select(x => x.ProductCode).ToArray(), retrieved.Subscriptions.Select(x => x.ProductCode).ToArray());
            });
        }
    }
}