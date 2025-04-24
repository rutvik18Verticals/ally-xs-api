using System.Collections.Generic;
using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Well Test Trend Data.
    /// </summary>
    public class WellTestTrendData : TrendData
    {

        /// <summary>
        /// Gets or set the unit type.
        /// </summary>
        public int UnitType { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WellTestTrendData" /> class.
        /// </summary>
        /// <param name="key">The unit type.</param>
        public WellTestTrendData(string key)
            : base(key)
        {
            this.UnitType = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WellTestTrendData" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public WellTestTrendData(string name, string description)
            : base(name)
        {
            this.UnitType = 0;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WellTestTrendData" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="unitType">The unitType.</param>
        public WellTestTrendData(string name, string description, int unitType)
            : this(name, description)
        {
            this.UnitType = unitType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WellTestTrendData" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="unitType">The unitType.</param>
        public WellTestTrendData(string name, string description, UnitCategory unitType)
            : this(name, description)
        {
            if (unitType == null)
            {
                throw new ArgumentNullException(nameof(unitType));
            }

            this.UnitType = unitType.Key;
        }

        #endregion

        /// <summary>
        /// Returns the data within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An <see cref="IList{DataPoint}" /> containing the data points found.</returns>
        /// <remarks>The Note property will be set to "Manual" or "Scan" depending on the record source.</remarks>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();

            return data;
        }

    }
}