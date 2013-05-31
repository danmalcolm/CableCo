using System;

namespace CableCo.Accounts
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.Empty;
        }

        public virtual Guid Id { get; protected set; }

        public virtual int Version { get; protected set; }

        #region Equals / hashcode

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            // Other is null or not an Entity
            if (ReferenceEquals(null, other))
                return false;

            // Same object reference
            if (ReferenceEquals(this, other))
                return true;

            // Other must be same type
            if (GetTypeUnproxied() != other.GetTypeUnproxied())
                return false;

            // Different unsaved entities can't be equal
            if (Equals(Guid.Empty, this.Id) && Equals(Guid.Empty, other.Id))
                return false;

            return Equals(this.Id, other.Id);
        }

        public override int GetHashCode()
        {
            if (Id == Guid.Empty)
            {
                // New entity that has not been assigned an id, use base
                // implementation
                return base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    return (Id.GetHashCode() * 397) ^ GetTypeUnproxied().GetHashCode();
                }
            }
        }

        /// <summary>
        /// Gets the current type, or the type being proxied if this is
        /// an NHibernate generated proxy. NHibernate proxy classes shadow
        /// the GetType() method which returns the type being proxied:
        /// https://groups.google.com/forum/?fromgroups=#!topic/sharp-architecture/3dBfm67eAjo
        ///
        /// Calling this.GetTypeUnproxied() is not necessary, as a proxy
        /// instance would use its own GetType implementation and return
        /// the correct type. However, all calls to GetType()
        /// are directed through this method to support unit tests
        /// </summary>
        /// <returns></returns>
        protected virtual Type GetTypeUnproxied()
        {
            return GetType();
        }

        #endregion
    }
}