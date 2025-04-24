using System;
using System.Collections.Generic;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the GL Analysis Trend Data.
    /// </summary>
    public class GLAnalysisTrendData : TrendData
    {

        /// <summary>
        /// Gets the type of measurements used in GL Analysis.
        /// </summary>
        public GLAnalysisMeasurement Measurement { get; }

        #region Constructor

        /// <summary>
        /// Initializes a new GLAnalysisTrendData with a specified <seealso cref="GLAnalysisMeasurement"/>.
        /// </summary>
        /// <param name="measurement">The gl analysis measurement.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="measurement"/> is null
        /// </exception>
#pragma warning disable CA1062 // Validate arguments of public methods
        public GLAnalysisTrendData(GLAnalysisMeasurement measurement) : base(measurement.Key.ToString())
#pragma warning restore CA1062 // Validate arguments of public methods
        {
            this.Measurement = measurement ?? throw new ArgumentNullException(nameof(measurement));
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
        /// Gets the gl analysis trend data items.
        /// </summary>
        /// <returns>An array of <seealso cref="GLAnalysisTrendData"/> items.</returns>
        public static GLAnalysisTrendData[] GetItems()
        {
            IList<GLAnalysisTrendData> result = new List<GLAnalysisTrendData>();
            IList<GLAnalysisMeasurement> trendMeasurements = GetTrendMeasurements();

            foreach (GLAnalysisMeasurement measurement in (IEnumerable<GLAnalysisMeasurement>)trendMeasurements)
            {
                result.Add(new GLAnalysisTrendData(measurement)
                {
                    Description = measurement.Name,
                });
            }

            return result.OrderBy(a => a.Description).ToArray();
        }

        /// <summary>
        /// Get the lists of trend measurements.
        /// </summary>
        /// <returns>List of <seealso cref="IList{GLAnalysisMeasurement}"/>.</returns>
        public static IList<GLAnalysisMeasurement> GetTrendMeasurements()
        {
            var trendMeasurements = new List<GLAnalysisMeasurement>();
            trendMeasurements.Add(GLAnalysisMeasurement.ProductivityIndex);
            trendMeasurements.Add(GLAnalysisMeasurement.RateAtMaxOil);
            trendMeasurements.Add(GLAnalysisMeasurement.RateAtMaxLiquid);
            trendMeasurements.Add(GLAnalysisMeasurement.FlowingBHP);
            trendMeasurements.Add(GLAnalysisMeasurement.FormationGOR);
            trendMeasurements.Add(GLAnalysisMeasurement.TotalGLR);
            trendMeasurements.Add(GLAnalysisMeasurement.InjectedGLR);
            trendMeasurements.Add(GLAnalysisMeasurement.InjectedGasRate);
            trendMeasurements.Add(GLAnalysisMeasurement.MaxLiquidRate);
            trendMeasurements.Add(GLAnalysisMeasurement.InjectionRateForMaxLiquidRate);
            trendMeasurements.Add(GLAnalysisMeasurement.GLRForMaxLiquidRate);
            trendMeasurements.Add(GLAnalysisMeasurement.OptimumLiquidRate);
            trendMeasurements.Add(GLAnalysisMeasurement.InjectionRateForOptimumLiquidRate);
            trendMeasurements.Add(GLAnalysisMeasurement.GLRForOptimumLiquidRate);
            trendMeasurements.Add(GLAnalysisMeasurement.MinimumFlowingBHP);
            trendMeasurements.Add(GLAnalysisMeasurement.ValveCriticalVelocity);
            trendMeasurements.Add(GLAnalysisMeasurement.TubingCriticalVelocity);
            trendMeasurements.Add(GLAnalysisMeasurement.FirstInjectingValveDepth);

            return trendMeasurements;
        }
    }
}
