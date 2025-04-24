using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the PCSF Enron Archive Trend Data.
    /// </summary>
    public class PCSFEnronArchiveTrendData : TrendData
    {

        /// <summary>
        /// Gets or sets the UnitType.
        /// </summary>
        public int UnitType { get; set; }

        /// <summary>
        /// Gets or sets the DatalogNumber.
        /// </summary>
        public int DatalogNumber { get; set; }

        /// <summary>
        /// Gets or sets the FieldNumber.
        /// </summary>
        public int FieldNumber { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <seealso cref="PCSFEnronArchiveTrendData"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public PCSFEnronArchiveTrendData(string key)
            : base(key)
        {
            this.UnitType = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="PCSFEnronArchiveTrendData"/> class.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public PCSFEnronArchiveTrendData(string nodeId, string name, string description)
            : base(name)
        {
            this.UnitType = 0;
            this.Name = name;
            if (!string.IsNullOrEmpty(description))
            {
                this.Description = description;
            }
            else
            {
                this.Description = name;
            }

            this.NodeId = nodeId;
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="PCSFEnronArchiveTrendData"/> class.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="unitType">The unit type.</param>
        public PCSFEnronArchiveTrendData(string nodeId, string name, string description, int unitType)
            : this(nodeId, name, description)
        {
            this.UnitType = unitType;
        }

        #endregion

        /// <summary>Returns the requested list of datapoints.</summary>
        /// <param name="startDate">starting datetime for date range of data to return.</param>
        /// <param name="endDate">ending datetime for date range of data to return.</param>
        /// <returns>List of data points <seealso cref="IList{DataPoint}"/>.</returns>
        /// <remarks>Requires DatalogNumber and DatalogField to be previously setup.</remarks>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();

            return data;
        }

    }
}