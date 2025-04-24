using System;
using Theta.XSPOC.Apex.Kernel.Utilities;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represent the data points response contract.
    /// </summary>
    /// <typeparam name="TX">The type of the x coordinate value.</typeparam>
    /// <typeparam name="TY">The type of the y coordinate value.</typeparam>
    public class DataPoint<TX, TY>
    {

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public TX X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        public TY Y { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        public string Note { get; set; }

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        public DataPoint()
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public DataPoint(TX x, TY y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="note">The note.</param>
        public DataPoint(TX x, TY y, string note)
            : this(x, y)
        {
            this.Note = note;
        }

        #endregion

        /// <summary>
        /// Function to check the x and y coordinates of the data point are same.
        /// </summary>
        /// <param name="obj">The <seealso cref="DataPoint"/>.</param>
        /// <returns>Bool value whether x and y coordinates of the data point are same.</returns>
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is DataPoint<TX, TY> dataPoint)
            {
                flag = Equals(X, dataPoint.X) &&
                    Equals(Y, dataPoint.Y);
            }

            return flag;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return MathUtility.GenerateHashCode(X, Y);
        }
    }

    /// <summary>
    /// Represent the data points response contract.
    /// </summary>
    public class DataPoint : DataPoint<DateTime, Decimal>
    {

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        public DataPoint()
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public DataPoint(DateTime x, Decimal y)
            : base(x, y)
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="note">The note.</param>
        public DataPoint(DateTime x, Decimal y, string note)
            : base(x, y, note)
        {
        }
    }

}
