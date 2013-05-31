using System;

namespace CableCo.Accounts.Events
{
    /// <summary>
    /// A mechanism used by objects in the Accounts domain to raise events relating to
    /// operations executed
    /// </summary>
    public class DomainEvents
    {
        private static Action<IDomainEvent> handleEvent = @event => { };

        /// <summary>
        /// Configures the way in which events raised by domain objects are handled
        /// </summary>
        /// <param name="handle"></param>
        public static void Configure(Action<IDomainEvent> handle)
        {
            DomainEvents.handleEvent = handle;
        }
        
        public static void Raise(IDomainEvent @event)
        {
            handleEvent(@event);
        }
    }
}