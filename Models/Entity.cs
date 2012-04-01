namespace NHibernateExperiments.Models
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }

        private int? _oldHashCode;

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (other == null)
            {
                return false;
            }

            //to handle the case of comparing two new objects
            var otherIsTransient = Equals(other.Id, default(int));
            var thisIsTransient = Equals(Id, default(int));

            if (otherIsTransient && thisIsTransient)
            {
                return ReferenceEquals(other, this);
            }

            return other.Id.Equals(Id);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            //This is done se we won't change the hash code
            if (_oldHashCode.HasValue)
            {
                return _oldHashCode.Value;
            }
            var thisIsTransient = Equals(Id, default(int));

            //When we are transient, we use the base GetHashCode()
            //and remember it, so an instance can't change its hash code.

            if (thisIsTransient)
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }

            return Id.GetHashCode();
        }

        /// <summary>
        /// Equality operator so we can have == semantics
        /// </summary>
        public static bool operator ==(Entity x, Entity y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Inequality operator so we can have != semantics
        /// </summary>
        public static bool operator !=(Entity x, Entity y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}