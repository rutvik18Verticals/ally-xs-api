using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the class for esp analysis response.
    /// </summary>
    public class ESPAnalysisResponse
    {

        /// <summary>
        /// The cause id.
        /// </summary>
        public int? CauseId { get; set; }

        /// <summary>
        /// The poc type.
        /// </summary>
        public short PocType { get; set; }

        /// <summary>
        /// The card type.
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// The node master data object.
        /// </summary>
        public NodeMasterModel NodeMasterData { get; set; }

        /// <summary>
        /// The ESP Analysis Result object. 
        /// </summary>
        public ESPAnalysisResultModel AnalysisResultEntity { get; set; }

        /// <summary>
        /// The ESP Analysis Result object. 
        /// </summary>
        public IList<ESPWellPumpModel> WellPumpEntities { get; set; }

        /// <summary>
        /// Gets or sets the test date.
        /// </summary>
        public DateTime TestDate { get; set; }

    }
}
