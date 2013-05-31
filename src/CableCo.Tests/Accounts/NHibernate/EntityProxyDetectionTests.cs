using System;
using System.Reflection;
using CableCo.Accounts;
using NUnit.Framework;

namespace CableCo.Tests.Accounts.NHibernate
{
    public class EntityProxyDetectionTests : DatabaseTests
    {
        [Test]
        public void GetTypeUnproxied_should_reveal_type_being_proxied()
        {
            var supplier1 = new Account("s-1");
            InTransaction(session => session.Save(supplier1));

            var getTypeUnproxiedMethod = typeof(Account).GetMethod("GetTypeUnproxied", BindingFlags.NonPublic | BindingFlags.Instance);
            InTransaction(session =>
            {
                var supplier2 = session.Load<Account>(supplier1.Id);
                var type = supplier2.GetType();

                StringAssert.Contains("Proxy", type.Name);
                Type typeUnproxied = (Type)getTypeUnproxiedMethod.Invoke(supplier2, null);
                Assert.AreEqual(typeof(Account), typeUnproxied);
            });
        }
    }
}