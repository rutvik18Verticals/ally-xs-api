using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class to represent the model for Data History Item.
    /// </summary>
    public class DataHistoryItemModel
    {

        /// <summary>
        /// Gets or sets the failure component trend data.
        /// </summary>
        public NodeMasterModel NodeMasterData { get; set; }

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
        /// Get or set the system parameter value determining gas lift included in test.
        /// </summary>
        public string GLIncludeInjGasInTest { get; set; }

    }
}
