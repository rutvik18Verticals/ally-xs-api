using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents an object with a unique identifier.
    /// </summary>
    public abstract class IdentityBase
    {

        #region Public Properties

        /// <summary>
        /// Gets the ID.
        /// </summary>
        public object Id { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been assigned an ID.
        /// </summary>
        public bool IsNew => Id == null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new IdentityBase with the default ID.
        /// </summary>
        public IdentityBase()
        {
        }

        /// <summary>
        /// Initializes a new IdentityBase with a specified ID.
        /// </summary>
        /// <param name="id">The ID</param>
        public IdentityBase(object id)
        {
            Id = id;
        }

        #endregion

        #region Static Methods

        private static bool AreEqual(IdentityBase identity1, IdentityBase identity2)
        {
            //both are either the same reference or null
            if (ReferenceEquals(identity1, identity2))
            {
                return true;
            }

            //one is null, but the other is not
            if (identity1 is null || identity2 is null)
            {
                return false;
            }

            //the ID has not been set on at least one of them, so don't use it for comparison
            //we've already established that the references are not equal
            if (identity1.IsNew || identity2.IsNew)
            {
                return false;
            }

            return identity1.GetType().Equals(identity2.GetType()) && identity1.Id.Equals(identity2.Id);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the ID to a specified value.
        /// </summary>
        /// <param name="id">The ID</param>
        /// <exception cref="ArgumentNullException">id is null.</exception>
        public void SetId(object id)
        {
            if (id == null)
            {
                const string PARAM_NAME = nameof(id);
                throw new ArgumentNullException(PARAM_NAME);
            }

            Id = id;
        }

        /// <summary>
        /// Resets the ID to the default value.
        /// </summary>
        public void ResetId()
        {
            Id = null;
        }

        #endregion

        #region Overridden Object Members

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>true if this instance is equal to obj; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is IdentityBase identity)
            {
                return AreEqual(this, identity);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code generated.</returns>
        public override int GetHashCode()
        {
            //if the ID has not been set, don't use it to generate a hash code
            if (IsNew)
            {
                return base.GetHashCode();
            }
            else
            {
                //use the type and ID to generate a hash code
                return MathUtility.GenerateHashCode(GetType(), Id);
            }
        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Indicates whether two identities are equal.
        /// </summary>
        /// <param name="identity1">An identity</param>
        /// <param name="identity2">Another identity</param>
        /// <returns>true if identity1 equals identity2; otherwise, false.</returns>
        public static bool operator ==(IdentityBase identity1, IdentityBase identity2)
        {
            return AreEqual(identity1, identity2);
        }

        /// <summary>
        /// Indicates whether two identities are not equal.
        /// </summary>
        /// <param name="identity1">An identity</param>
        /// <param name="identity2">Another identity</param>
        /// <returns>true if identity1 does not equal identity2; otherwise, false.</returns>
        public static bool operator !=(IdentityBase identity1, IdentityBase identity2)
        {
            return !AreEqual(identity1, identity2);
        }

        #endregion

    }
}
