using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Measurement Trend Data.
    /// </summary>
    public class MeasurementTrendData : TrendData
    {

        private readonly IList<int> _addresses = new List<int>();

        /// <summary>
        /// Gets or sets the ParamStandardType.
        /// </summary>
        public int ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public int? UnitType { get; set; }

        /// <summary>
        /// Gets or sets the address of the parameter with the highest priority.
        /// </summary>
        public int PriorityAddress { get; set; }

        /// <summary>
        /// Gets or sets the description of the parameter with the highest priority.
        /// </summary>
        public string PriorityDescription { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <seealso cref="MeasurementTrendData"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public MeasurementTrendData(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="MeasurementTrendData"/> class with <seealso cref="MeasurementTrendDataModel"/>.
        /// </summary>
        /// <param name="record">The <seealso cref="MeasurementTrendDataModel"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="record"/> is null.
        /// </exception>
        public MeasurementTrendData(MeasurementTrendItemModel record) : base(record != null
            ? record.ParamStandardType.ToString()
            : string.Empty)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.Description = record.Name;
            this.Name = this.Description;
            this.ParamStandardType = record.ParamStandardType != null ? (int)record.ParamStandardType : 0;
            this.PriorityAddress = record.Address != null ? (int)record.Address : 0;

            this.UnitType = record.UnitTypeID;

            if (string.IsNullOrEmpty(record.Description))
            {
                PriorityDescription = record.Description;
                if (string.IsNullOrEmpty(Name))
                {
                    Description = PriorityDescription;
                    Name = PriorityDescription;
                }
            }
        }

        #endregion

        /// <summary>Returns the address for a specified point.</summary>
        /// <param name="index">The index of the point.</param>
        /// <returns>The address for the specified point if found; otherwise, null.</returns>
        public int? GetAddressForPoint(int index)
        {
            int? addressForPoint = new int?();
            if (this._addresses.Any() && index >= 0 && index < this._addresses.Count)
            {
                addressForPoint = new int?(this._addresses[index]);
            }

            return addressForPoint;
        }

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

        /// <summary>
        /// Gets the analysis trend data items.
        /// </summary>
        /// <param name="measurementTrendData">List of <seealso cref="IList{MeasurementTrendItemModel}"/> model object.</param>
        /// <returns>An array of <seealso cref="MeasurementTrendData"/> items.</returns>
        public static MeasurementTrendData[] GetItems(IList<MeasurementTrendItemModel> measurementTrendData)
        {
            var measurementTrendDataList = new List<MeasurementTrendData>();
            if (measurementTrendData != null && measurementTrendData.Count > 0)
            {
                foreach (var item in measurementTrendData)
                {
                    measurementTrendDataList.Add(new MeasurementTrendData(item));
                }
            }

            return measurementTrendDataList.ToArray();
        }
    }
}
