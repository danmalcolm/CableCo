using System.Linq;
using CableCo.Accounts;
using CableCo.Accounts.Commands;
using CableCo.Common.Logging;
using NHibernate;
using NHibernate.Linq;
using Rebus;
using log4net;

namespace CableCo.AccountsService.Handlers
{
    public class CreateAccountHandler : IHandleMessages<CreateAccount>
    {
        private static readonly ILog Log = LogUtility.ForCurrentType();
        private readonly IBus bus;
        private readonly ISession session;

        public CreateAccountHandler(IBus bus, ISession session)
        {
            this.bus = bus;
            this.session = session;
        }

        public void Handle(CreateAccount message)
        {
            Log.InfoFormat("Handling CreateAccount: {0}", message.AccountCode);

            if (session.Query<Account>().Any(x => x.Code == message.AccountCode))
            {
                AlertThatCommandInvalid(message);
                return;
            }
            var account = new Account(message.AccountCode);
            session.SaveOrUpdate(account);
            AlertThatCommandSucceeded(message);
        }

        private void AlertThatCommandSucceeded(CreateAccount message)
        {
            bus.Reply(CommandAlert.Success(string.Format("I've sorted that for you chief. Account {0} has been created", message.AccountCode)));
        }

        private void AlertThatCommandInvalid(CreateAccount message)
        {
            bus.Reply(CommandAlert.Invalid(string.Format("An account with the code '{0}' already exists", message.AccountCode)));
        }
    }
}