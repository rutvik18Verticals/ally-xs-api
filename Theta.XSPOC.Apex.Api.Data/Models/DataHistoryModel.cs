using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class to represent the model for Data History Item.
    /// </summary>
    public class DataHistoryModel
    {

        /// <summary>
        /// Gets or sets the node master data.
        /// </summary>
        public NodeMasterModel NodeMasterData { get; set; }

        /// <summary>
        /// Gets or sets the group trend data.
        /// </summary>
        public IList<GroupTrendDataModel> GroupTrendData { get; set; }

        /// <summary>
        /// Gets or sets the group trend data.
        /// </summary>
        public IList<MeasurementTrendItemModel> MeasurementTrendData { get; set; }

        /// <summary>
        /// Gets or sets the controller trend data.
        /// </summary>
        public IList<ControllerTrendItemModel> ControllerTrendData { get; set; }

        /// <summary>
        /// Gets or sets the analysis trend data.
        /// </summary>
        public IList<string> AnalysisTrendData { get; set; }

        /// <summary>
        /// Gets or sets the rod stress trend data.
        /// </summary>
        public IList<RodStressTrendItemModel> RodStressTrendData { get; set; }

        /// <summary>
        /// Gets or sets the well test trend data.
        /// </summary>
        public IList<RodStressTrendItemModel> WellTestTrendData { get; set; }

        /// <summary>
        /// Gets or sets the failure component trend data.
        /// </summary>
        public IList<ComponentItemModel> FailureComponentTrendData { get; set; }

        /// <summary>
        /// Gets or sets the failure sub component trend data.
        /// </summary>
        public IList<ComponentItemModel> FailureSubComponentTrendData { get; set; }

        /// <summary>
        /// Gets or sets the meter trend data.
        /// </summary>
        public IList<MeterColumnItemModel> MeterTrendData { get; set; }

        /// <summary>
        /// Gets or sets the pcsf datalog configuration trend data.
        /// </summary>
        public IList<PCSFDatalogConfigurationItemModel> PCSFDatalogConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the pcsf datalog configuration trend data.
        /// </summary>
        public IList<EventTrendItem> EventsTrendData { get; set; }

        /// <summary>
        /// Get or set the system parameter value determining gas lift included in test
        /// </summary>
        public string GLIncludeInjGasInTest { get; set; }

    }
}
