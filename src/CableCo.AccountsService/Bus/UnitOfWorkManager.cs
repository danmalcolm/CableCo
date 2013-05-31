using CableCo.Common.Logging;
using Castle.Windsor;
using Rebus;
using Rebus.Bus;
using log4net;

namespace CableCo.AccountsService.Bus
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();
        private readonly IWindsorContainer container;

        public UnitOfWorkManager(IWindsorContainer container)
        {
            this.container = container;
        }

        public IUnitOfWork Create()
        {
            var context = MessageContext.GetCurrent();
            if (IsInternalMessage(context))
            {
                return new NullUnitOfWork();
            }
            else
            {
                Log.DebugFormat("Creating unit of work");
                return new UnitOfWork(container, MessageContext.GetCurrent());    
            }
        }

        private bool IsInternalMessage(IMessageContext context)
        {
            return false;
        }
    }
}