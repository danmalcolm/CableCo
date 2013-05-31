using System;
using System.Web;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace CableCo.Accounts.NHibernate
{
    /// <summary>
    /// An implementation of ICurrentSessionContext that makes an NHibernate session 
    /// available for the current HttpContext. Initialisation of the session is
    /// deferred until the session is accessed. LazyWebSessionContextModule is responsible 
    /// for starting session and coordinating rollbacks / commits
    /// Based on an example at http://joseoncode.com/2011/03/03/effective-nhibernate-session-management-for-web-apps/
    /// without support for multiple session factories
    /// </summary>
    public class LazyWebSessionContext : ICurrentSessionContext
    {
        private const string LazySessionKey = "LazyWebSessionContext.CurrentSession";

        // Required constructor signature
        public LazyWebSessionContext(ISessionFactoryImplementor factory) {}

        public ISession CurrentSession()
        {
            var lazy = HttpContext.Current.Items[LazySessionKey] as Lazy<ISession>;
            return lazy == null ? null : lazy.Value;
        }

        /// <summary>
        /// Initialises session for the current web request. The session is started
        /// on-demand (when CurrentSession method is called), so the startSession
        /// parameter might not be called if a session isn't required during the
        /// current request
        /// </summary>
        /// <param name="startSession">Function to initialize the current session</param>
        public static void Bind(Func<ISession> startSession)
        {
            var lazy = new Lazy<ISession>(startSession);
            HttpContext.Current.Items[LazySessionKey] = lazy;
        }

        /// <summary>
        /// Unbinds the session from the current HttpContext. Also returns the session
        /// if it has been started.
        /// </summary>
        /// <returns>The active session for the current web request, if it has been started</returns>
        public static ISession Unbind()
        {
            var lazy = HttpContext.Current.Items[LazySessionKey] as Lazy<ISession>;
            HttpContext.Current.Items.Remove(LazySessionKey);
            if (lazy == null || !lazy.IsValueCreated) return null;
            return lazy.Value;
        }
    }
}