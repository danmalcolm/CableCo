using CableCo.Common.Logging;
using Castle.Windsor;
using log4net;
using NHibernate;
using Rebus.Pipeline;

namespace CableCo.Common.Rebus
{
    public class UnitOfWork 
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();
        private readonly IWindsorContainer container;
        private readonly ISession session;

        public UnitOfWork(IWindsorContainer container, IMessageContext context)
        {
            Log.DebugFormat("Starting unit of work");
            this.container = container;
            var factory = container.Resolve<ISessionFactory>();
            session = factory.OpenSession();
            session.BeginTransaction();
            context.TransactionContext.Items["session"] = session;
        }

        public void Dispose()
        {
            Log.DebugFormat("Disposing unit of work");
            session.Dispose();
        }

        public void Commit()
        {
            Log.DebugFormat("Committing unit of work");
            session.Transaction.Commit();
        }

        public void Rollback()
        {
            Log.DebugFormat("Aborting unit of work");
            session.Transaction.Rollback();
        }
    }
}