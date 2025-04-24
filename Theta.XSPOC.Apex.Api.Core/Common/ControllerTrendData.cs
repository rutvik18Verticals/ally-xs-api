using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Controller Trend Data.
    /// </summary>
    public class ControllerTrendData : TrendData
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public bool IsFacilityTag { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public int UnitType { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the register.
        /// </summary>
        public string Register => this.Tag ?? this.Address.ToString();

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <seealso cref="ControllerTrendData"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public ControllerTrendData(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="ControllerTrendData"/> class with <seealso cref="ControllerTrendItemModel"/>.
        /// </summary>
        /// <param name="record">The <seealso cref="ControllerTrendItemModel"/>object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="record"/> is null.
        /// </exception>
        public ControllerTrendData(ControllerTrendItemModel record)
            : base(string.Empty)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.IsFacilityTag = record.FacilityTag == 1;
            this.Name = record.Name;
            this.Description = record.Description;
            this.Address = record.Address;
            this.UnitType = record.UnitType;
            this.Tag = record.Tag ?? null;
            this.Key = this.Address.ToString();
        }

        #endregion

        /// <summary>Returns the data within a specified date range.</summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An <see cref="IList{DataPoint}" /> containing the data points found.</returns>
        /// <remarks>The Note property will be set to "Manual" or "Scan" depending on the record source.</remarks>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();

            return data;
        }

        /// <summary>
        /// Gets the controller trend data items.
        /// </summary>
        /// <param name="controllerTrendData">List of <seealso cref="IList{ControllerTrendItemModel}"/> model object.</param>
        /// <returns>An array of <seealso cref="ControllerTrendData"/> items.</returns>
        public static ControllerTrendData[] GetItems(IList<ControllerTrendItemModel> controllerTrendData)
        {
            IList<ControllerTrendData> controllerTrendDataList = new List<ControllerTrendData>();

            if (controllerTrendData != null && controllerTrendData.Count > 0)
            {
                foreach (var item in controllerTrendData)
                {
                    controllerTrendDataList.Add(new ControllerTrendData(item));
                }
            }

            return controllerTrendDataList.ToArray();
        }

    }
}
