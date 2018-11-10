using System;
using Rebus.Sagas;

namespace CableCo.AccountsService.Handlers
{
    public class DirectDebitApplicationProcess : Saga<DirectDebitApplicationContext>
    {
       
        protected override void CorrelateMessages(ICorrelationConfig<DirectDebitApplicationContext> config)
        {
            throw new NotImplementedException();
        }
    }

    public class DirectDebitApplicationContext : ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }
    }
}