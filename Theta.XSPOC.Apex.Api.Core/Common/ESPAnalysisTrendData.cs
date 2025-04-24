using System;
using System.Collections.Generic;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the ESP Analysis Trend Data.
    /// </summary>
    public class ESPAnalysisTrendData : TrendData
    {

        /// <summary>
        /// Gets the type of measurements used in ESP Analysis.
        /// </summary>
        public ESPAnalysisMeasurement Measurement { get; }

        #region Constructor

        /// <summary>
        /// Initializes a new ESPAnalysisTrendData with a specified <seealso cref="ESPAnalysisMeasurement"/>.
        /// </summary>
        /// <param name="measurement">The esp analysis measurement.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="measurement"/> is null.
        /// </exception>
#pragma warning disable CA1062 // Validate arguments of public methods
        public ESPAnalysisTrendData(ESPAnalysisMeasurement measurement) : base(measurement.Key.ToString())
#pragma warning restore CA1062 // Validate arguments of public methods
        {
            Measurement = measurement ?? throw new ArgumentNullException(nameof(measurement));
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
        /// Gets the esp analysis trend data items.
        /// </summary>
        /// <returns>An array of <seealso cref="ESPAnalysisTrendData"/> items.</returns>
        public static ESPAnalysisTrendData[] GetItems()
        {
            IList<ESPAnalysisTrendData> result = new List<ESPAnalysisTrendData>();
            foreach (ESPAnalysisMeasurement measurement in EnhancedEnumBase.GetValues<ESPAnalysisMeasurement>())
            {
                result.Add(new ESPAnalysisTrendData(measurement)
                {
                    Description = measurement.Name,
                });
            }

            return result.OrderBy(a => a.Description).ToArray();
        }

    }
}
