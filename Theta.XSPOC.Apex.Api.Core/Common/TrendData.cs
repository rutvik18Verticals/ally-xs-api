using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// The trend data base class for different type of trend data. 
    /// </summary>
    public abstract class TrendData : IComparable
    {

        private string _key;
        private string _name;

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key
        {
            get => this.GetType() + "." + this._key + "." + this.NodeId;
            set => this._key = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get => _name == string.Empty ? Description : _name;
            set => this._name = value;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        #region Constructor

        /// <summary>
        /// Initializes a new TrendData with a key.
        /// </summary>
        /// <param name="key">The key.</param>
        public TrendData(string key)
        {
            this.NodeId = string.Empty;
            this._key = string.Empty;
            this._name = string.Empty;
            this.Description = string.Empty;
            this._key = key;
        }

        #endregion

        /// <summary>
        /// Indicates whether trend data values are equal to obj.
        /// </summary>
        /// <param name="obj">The object of type <seealso cref="TrendData"/>.</param>
        /// <returns>true if object is equals to trenddata; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is not TrendData)
            {
                return false;
            }

            TrendData trendData = (TrendData)obj;

            if (trendData == null)
            {
                return false;
            }

            return this.GetType().Equals(trendData.GetType()) &
                this.NodeId.Equals(trendData.NodeId, StringComparison.Ordinal) &
                this._key.Equals(trendData._key, StringComparison.Ordinal);
        }

        /// <summary>
        /// Indicates whether trend data values are equal to obj.
        /// </summary>
        /// <param name="obj">The object of type <seealso cref="TrendData"/>.</param>
        /// <returns>true if object is equals to trenddata; otherwise, false.</returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return obj is TrendData data
                ? this.Description.CompareTo(data.Description)
                : throw new InvalidOperationException(string.Format("Could not compare type {0} to {1}",
                    obj.GetType(), this.GetType()));
        }

        /// <summary>Abstract method to returns the data within a specified date range.</summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An <see cref="IList{DataPoint}" /> containing the data points found.</returns>
        public abstract IList<DataPoint> GetData(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code generated.</returns>
        /// <exception cref="NotImplementedException">.</exception>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #region Overloaded Operators

        /// <summary>
        /// Indicates whether two trend data values are equal.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">Another value.</param>
        /// <returns>true if value1 equals value2; otherwise, false.</returns>
        public static bool operator ==(TrendData left, TrendData right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two trend data values are not equal.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">Another value.</param>
        /// <returns>true if value1 does not equal value2; otherwise, false.</returns>
        public static bool operator !=(TrendData left, TrendData right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Indicates whether a trend data value is less than another.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">Another value.</param>
        /// <returns>true if value1 is less than value2; otherwise, false.</returns>
        public static bool operator <(TrendData left, TrendData right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Indicates whether a trend data value is less or equal to another.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">Another value.</param>
        /// <returns>true if value1 is less than or equal to value2; otherwise, false.</returns>
        public static bool operator <=(TrendData left, TrendData right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Indicates whether a trend data value is greater than another.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">Another value.</param>
        /// <returns>true if value1 is greater than value2; otherwise, false.</returns>
        public static bool operator >(TrendData left, TrendData right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Indicates whether a trend data value is greater or equal to another.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">Another value.</param>
        /// <returns>true if value1 is greater than or equal to value2; otherwise, false.</returns>
        public static bool operator >=(TrendData left, TrendData right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }

        #endregion

    }
}
