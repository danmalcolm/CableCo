using System;
using Rebus;

namespace CableCo.AccountsService.Handlers
{
    public class DirectDebitApplicationProcess : Saga<DirectDebitApplicationContext>
    {
        public override void ConfigureHowToFindSaga()
        {
            
        }
    }

    public class DirectDebitApplicationContext : ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }
    }
}