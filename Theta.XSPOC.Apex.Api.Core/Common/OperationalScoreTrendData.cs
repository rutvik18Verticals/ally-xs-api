using System;
using System.Collections.Generic;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the data for an operational score trend.
    /// <seealso cref="TrendData" />
    /// </summary>
    public class OperationalScoreTrendData : TrendData
    {

        #region Private Members

        private enum LocalePhraseIDs
        {
            OperationalScore = 6340, // Operational Score    
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationalScoreTrendData" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public OperationalScoreTrendData(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationalScoreTrendData" /> class.
        /// </summary>
        /// <param name="name">The name of the trend data.</param>
        /// <param name="description">The description of the trend data.</param>
        public OperationalScoreTrendData(string name, string description)
            : base(name)
        {
            Name = name;
            Description = description;
        }

        /// <summary>Returns the data within a specified date range.</summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An <see cref="IList{DataPoint}" /> containing the data points found.</returns>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();

            return data;
        }

        /// <summary>
        /// Gets the operational score trend data items.
        /// </summary>
        /// <param name="phrases">The locale phrase dictionary.</param>
        /// <returns>An Array of <seealso cref="OperationalScoreTrendData"/> items.</returns>
        public static OperationalScoreTrendData[] GetItems(IDictionary<int, string> phrases)
        {
            if (phrases == null)
            {
                return null;
            }

            return new List<OperationalScoreTrendData>
                {
                    new OperationalScoreTrendData("OperationalScore", phrases[(int)LocalePhraseIDs.OperationalScore])
                }.OrderBy(a => a.Description).ToArray();
        }
    }
}
