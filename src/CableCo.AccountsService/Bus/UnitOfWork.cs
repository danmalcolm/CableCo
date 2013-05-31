using CableCo.Common.Logging;
using Castle.Windsor;
using NHibernate;
using Rebus;
using Rebus.Bus;
using log4net;

namespace CableCo.AccountsService.Bus
{
    public class NullUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
            
        }

        public void Commit()
        {
            
        }

        public void Abort()
        {
            
        }
    }
    public class UnitOfWork : IUnitOfWork
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
            context.Items["session"] = session;
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

        public void Abort()
        {
            Log.DebugFormat("Aborting unit of work");
            session.Transaction.Rollback();
        }
    }
}