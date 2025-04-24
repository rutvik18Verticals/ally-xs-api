using System;
using Theta.XSPOC.Apex.Kernel.Quantity;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
#pragma warning disable CA1036
    /// <summary>
    /// Supports implementing IValue on sealed classes
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    public abstract class ValueContainerBase<T> : IComparable, IComparable<ValueContainerBase<T>>, IComparable<T>,
        IEquatable<ValueContainerBase<T>>, IEquatable<T>, IValue
        where T : IComparable<T>, IEquatable<T>
    {

        #region Fields

        /// <summary>
        /// The value.
        /// </summary>
#pragma warning disable CA1051
        protected T Value;
#pragma warning restore CA1051

        #endregion

        #region Properties

        /// <summary>
        /// The amount of the value.
        /// </summary>
        public T Amount => Value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new ValueContainerBase with a specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <exception cref="ArgumentNullException">value is null</exception>
        public ValueContainerBase(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        #endregion

        #region Overridden Object Members

        /// <summary>
        /// Indicates whether this instance is equal to a specified object
        /// </summary>
        /// <param name="obj">An object to compare to</param>
        /// <returns>true if this instance is equal to obj, otherwise false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is T)
            {
                return Equals((T)obj);
            }
            else if (obj is ValueContainerBase<T>)
            {
                return Equals((ValueContainerBase<T>)obj);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>The hash code generated</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation of the value of the instance
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Implicitly casts a ValueContainerBase object to the type of its value
        /// </summary>
        /// <param name="valueContainer">The value to cast</param>
        /// <returns>The value contained in valueContainer</returns>
        public static implicit operator T(ValueContainerBase<T> valueContainer)
        {
            if (valueContainer == null)
            {
                return default;
            }

            return valueContainer.Value;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares this instance with a specified object
        ///  and returns a value indicating their relationship to one another
        /// </summary>
        /// <param name="obj">An object to compare to</param>
        /// <returns>A signed integer that indicates the relative order of this instance and obj</returns>
        /// <exception cref="ArgumentException">obj is not of type T or ValueContainerBase</exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is T)
            {
                return CompareTo((T)obj);
            }
            else if (obj is ValueContainerBase<T>)
            {
                return CompareTo((ValueContainerBase<T>)obj);
            }
            else
            {
                throw new ArgumentException($"Object must be of type {typeof(T)} or {GetType()}");
            }
        }

        #endregion

        #region IComparable<ValueContainerBase<T>> Members

        /// <summary>
        /// Compares this instance with a specified value container
        ///  and returns a value indicating their relationship to one another
        /// </summary>
        /// <param name="other">A value container to compare to</param>
        /// <returns>A signed integer that indicates the relative order of this instance and obj</returns>
        public int CompareTo(ValueContainerBase<T> other)
        {
            if (other == null)
            {
                return 1;
            }

            return Value.CompareTo(other.Value);
        }

        #endregion

        #region IComparable<T> Members

        /// <summary>
        /// Compares this instance with a specified value
        ///  and returns a value indicating their relationship to one another
        /// </summary>
        /// <param name="other">A value to compare to</param>
        /// <returns>A signed integer that indicates the relative order of this instance and obj</returns>
        public int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }

        #endregion

        #region IEquatable<ValueContainerBase<T>> Members

        /// <summary>
        /// Indicates whether this instance is equal to a specified value container
        /// </summary>
        /// <param name="other">A value container to compare to</param>
        /// <returns>true if this instance is equal to other, otherwise false</returns>
        public bool Equals(ValueContainerBase<T> other)
        {
            if (other == null)
            {
                return false;
            }

            return Value.Equals(other.Value);
        }

        #endregion

        #region IEquatable<T> Members

        /// <summary>
        /// Indicates whether this instance is equal to a specified value
        /// </summary>
        /// <param name="other">A value to compare to</param>
        /// <returns>true if this instance is equal to other, otherwise false</returns>
        public bool Equals(T other)
        {
            return Value.Equals(other);
        }

        #endregion

    }
#pragma warning restore CA1036
}