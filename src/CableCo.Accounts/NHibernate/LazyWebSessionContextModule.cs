using System;
using System.Web;
using NHibernate;

namespace CableCo.Accounts.NHibernate
{
    /// <summary>
    /// Coordinates NHibernate session and transaction lifecycle during each web request
    /// </summary>
    public class LazyWebSessionContextModule : IHttpModule
    {
        /// <summary>
        /// The ConfigurationStore used to store NHibernate Configuration and
        /// SessionFactory within the currently executing web application. Needs
        /// to be provided during application startup.
        /// </summary>
        public static ConfigurationStore ConfigurationStore { get; set; }

        public void Init(HttpApplication httpApplication)
        {
            httpApplication.BeginRequest += BeginRequest;
            httpApplication.EndRequest += EndRequest;
            httpApplication.Error += Error;
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            LazyWebSessionContext.Bind(() =>
            {
                var session = ConfigurationStore.SessionFactory.OpenSession();
                session.BeginTransaction();
                return session;
            });
        }

        private void EndRequest(object sender, EventArgs e)
        {
            Unbind(t => t.Commit());
        }

        private void Error(object sender, EventArgs e)
        {
            Unbind(t => t.Rollback());
        }

        private void Unbind(Action<ITransaction> action)
        {
            var session = LazyWebSessionContext.Unbind();
            if (session != null)
            {
                var transaction = session.Transaction;
                if (transaction != null && transaction.IsActive)
                {
                    action(transaction);
                }
                session.Dispose();
            }
        }

        public void Dispose() {}
    }
}