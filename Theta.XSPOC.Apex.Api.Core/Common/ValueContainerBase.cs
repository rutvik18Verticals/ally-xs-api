using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
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
        /// Value type T.
        /// </summary>
        protected T Value { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public T Amount => Value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new ValueContainerBase with a specified value.
        /// </summary>
        /// <param name="value">The value</param>
        /// <exception cref="ArgumentNullException">value is null</exception>
        public ValueContainerBase(T value)
        {
            //if (ReferenceEquals(value, null))
            //{
            //    throw new ArgumentNullException("value");
            //}

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
            if (obj is null)
            {
                return false;
            }

            if (obj is T t)
            {
                return Equals(t);
            }
            else if (obj is ValueContainerBase<T> @base)
            {
                return Equals(@base);
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
            if (valueContainer is null)
            {
                return default;
            }

            return valueContainer.Value;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares this instance with a specified object
        /// and returns a value indicating their relationship to one another.
        /// </summary>
        /// <param name="obj">An object to compare to</param>
        /// <returns>A signed integer that indicates the relative order of this instance and obj</returns>
        /// <exception cref="ArgumentException">obj is not of type T or ValueContainerBase</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }

            var v = obj is T;
            if (v)
            {
                return CompareTo((T)obj);
            }
            else if (obj is ValueContainerBase<T> @base)
            {
                return CompareTo(@base);
            }
            else
            {
                throw new ArgumentException(String.Format("Object must be of type {0} or {1}", typeof(T), GetType()));
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
            if (other is null)
            {
                return 1;
            }

            return Value.CompareTo(other.Value);
        }

        #endregion

        #region IComparable<T> Members

        /// <summary>
        /// Compares this instance with a specified value
        /// and returns a value indicating their relationship to one another.
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
            if (other is null)
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

        /// <summary>
        /// Determines whether the values equal each other.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns><c>True</c> if the values equal each other; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ValueContainerBase<T> left, ValueContainerBase<T> right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether the values don't equal each other.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns><c>True</c> if the values don't equal each other; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ValueContainerBase<T> left, ValueContainerBase<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the left values is less than the right value.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns><c>True</c> if the left value is less than the right value; otherwise, <c>false</c>.</returns>
        public static bool operator <(ValueContainerBase<T> left, ValueContainerBase<T> right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether the left values is less than or equal to the right value.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns><c>True</c> if the left value is less than or equal to the right value; otherwise, <c>false</c>.</returns>
        public static bool operator <=(ValueContainerBase<T> left, ValueContainerBase<T> right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the left values is greater than the right value.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns><c>True</c> if the left value is greater than the right value; otherwise, <c>false</c>.</returns>
        public static bool operator >(ValueContainerBase<T> left, ValueContainerBase<T> right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left values is greater than or equal to the right value.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns><c>True</c> if the left value is greater than or equal to the right value; otherwise, <c>false</c>.</returns>
        public static bool operator >=(ValueContainerBase<T> left, ValueContainerBase<T> right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }

        #endregion

    }

    #region Interfaces

    /// <summary>
    /// Represents a general value type for classes to implement.
    /// </summary>
    public interface IValue
    {

    }

    #endregion

}
