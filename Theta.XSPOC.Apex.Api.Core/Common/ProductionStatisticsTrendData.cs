using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the data for a production statistics trend.
    /// <seealso cref="TrendData" />
    /// </summary>
    public class ProductionStatisticsTrendData : TrendData
    {

        #region Private Members

        private enum LocalePhraseIDs
        {
            DownProductionBOE = 6385, // Down Production BOE    
            LatestProductionBOE = 6383, // Production Latest BOE
            PeakProductionBOE = 6384, // Production Peak BOE   
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionStatisticsTrendData" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public ProductionStatisticsTrendData(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionStatisticsTrendData" /> class.
        /// </summary>
        /// <param name="name">The name of the trend data.</param>
        /// <param name="description">The description of the trend data.</param>
        public ProductionStatisticsTrendData(string name, string description)
            : base(name)
        {
            this.Name = name;
            this.Description = description;
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
        /// Gets the production statistics trend data.
        /// </summary>
        /// <param name="phrases">The locale phrase dictionary.</param>
        /// <returns>An array of <seealso cref="ProductionStatisticsTrendData"/> items.</returns>
        public static ProductionStatisticsTrendData[] GetItems(IDictionary<int, string> phrases)
        {
            if (phrases == null)
            {
                return null;
            }

            return new List<ProductionStatisticsTrendData>()
            {
                new ProductionStatisticsTrendData("DownProductionBOE", phrases[(int)LocalePhraseIDs.DownProductionBOE]),
                new ProductionStatisticsTrendData("LatestProductionBOE", phrases[(int)LocalePhraseIDs.LatestProductionBOE]),
                new ProductionStatisticsTrendData("PeakProductionBOE", phrases[(int)LocalePhraseIDs.PeakProductionBOE]),
            }.ToArray();
        }

    }
}
