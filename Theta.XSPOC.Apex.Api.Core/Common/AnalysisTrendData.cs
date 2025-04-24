using System;
using System.Collections.Generic;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Analysis Trend Data.
    /// </summary>
    public class AnalysisTrendData : TrendData
    {

        #region Constructor

        /// <summary>
        /// Initializes a new AnalysisTrendData with a column name.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="columnName"/> is null.
        /// </exception>
        public AnalysisTrendData(string columnName)
            : base(string.Empty)
        {
            this.Name = columnName ?? throw new ArgumentNullException(nameof(columnName));
            this.Key = this.Name;
            this.Description = columnName;
        }

        #endregion

        /// <summary>
        /// Overridden method to get the Data Points in the given date range.
        /// </summary>
        /// <param name="startDate">The start Date.</param>
        /// <param name="endDate">The end Date.</param>
        /// <returns>The <seealso cref="IList{DataPoint}"/>.</returns>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();

            return data;
        }

        /// <summary>
        /// Gets the analysis trend data items.
        /// </summary>
        /// <param name="analysisTrendData">List of analysis trend data items.</param>
        /// <returns>An array of <seealso cref="AnalysisTrendData"/> items object.</returns>
        public static AnalysisTrendData[] GetItems(IList<string> analysisTrendData)
        {
            IList<AnalysisTrendData> analysisTrendDataList = new List<AnalysisTrendData>();
            if (analysisTrendData != null && analysisTrendData.Count > 0)
            {
                foreach (var item in analysisTrendData)
                {
                    analysisTrendDataList.Add(new AnalysisTrendData(item));
                }
            }
            return analysisTrendDataList.ToArray();
        }

    }
}
