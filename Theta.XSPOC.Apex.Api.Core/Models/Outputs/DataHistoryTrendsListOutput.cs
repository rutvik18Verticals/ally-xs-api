using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the data history trend items output response.
    /// </summary>
    public class DataHistoryTrendsListOutput
    {

        /// <summary>
        /// Gets or sets the list esp analysis trend data items.
        /// </summary>
        public ESPAnalysisTrendData[] ESPAnalysisTrendDataItems { get; set; }

        /// <summary>
        /// Gets or sets the list of gl analysis trend data items.
        /// </summary>
        public GLAnalysisTrendData[] GLAnalysisTrendDataItems { get; set; }

        /// <summary>
        /// Gets or sets the list of operational score trend data items.
        /// </summary>
        public OperationalScoreTrendData[] OperationalScoreTrendDataItems { get; set; }

        /// <summary>
        /// Gets or sets the list of production statistics trend data items.
        /// </summary>
        public ProductionStatisticsTrendData[] ProductionStatisticsTrendDataItems { get; set; }

        /// <summary>
        /// Gets or sets the list of meter column data items.
        /// </summary>
        public IList<MeterColumnItemModel> MeterColumnDataItems { get; set; }

        /// <summary>
        /// Gets or sets the list of failure component trend data.
        /// </summary>
        public IList<ComponentItemModel> FailureComponentTrendData { get; set; }

        /// <summary>
        /// Gets or sets the list of failure sub component trend data.
        /// </summary>
        public IList<ComponentItemModel> FailureSubComponentTrendData { get; set; }

        /// <summary>
        /// Gets or sets the list of plunger lift trend data items.
        /// </summary>
        public PlungerLiftTrendData[] PlungerLiftTrendDataItems { get; set; }

        /// <summary>
        /// Gets or sets the list of pcsf datalog configuration.
        /// </summary>
        public IDictionary<int, PCSFDatalogConfiguration> PCSFDatalogConfigurationData { get; set; }

        /// <summary>
        /// Gets or sets the list of events trend data items.
        /// </summary>
        public IList<EventTrendItem> EventsTrendDataItems { get; set; }

        /// <summary>
        /// Gets or sets the rod stress trend data items.
        /// </summary>
        public IList<RodStressTrendItemModel> RodStressTrendItems { get; set; }

    }
}
