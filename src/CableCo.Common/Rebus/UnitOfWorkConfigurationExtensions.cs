using Castle.Windsor;
using Rebus.Config;
using Rebus.Pipeline;

namespace CableCo.Common.Rebus
{
    public static class UnitOfWorkConfigurationExtensions
    {
        public static RebusConfigurer SetupUnitOfWork(this RebusConfigurer configurer, IWindsorContainer container)
        {
            return configurer.Options(x => x.EnableUnitOfWork(c => Create(c, container), Commit, Rollback, Dispose));
        }

        private static UnitOfWork Create(IMessageContext context, IWindsorContainer container)
        {
            var unitOfWork = new UnitOfWork(container, context);

            // stash current unit of work in the transaction context's items
            context.TransactionContext.Items["uow"] = unitOfWork;

            return unitOfWork;
        }

        private static void Commit(IMessageContext context, UnitOfWork uow) => uow.Commit();

        private static void Rollback(IMessageContext context, UnitOfWork uow) => uow.Rollback();

        private static void Dispose(IMessageContext context, UnitOfWork uow) => uow.Dispose();
    }
}