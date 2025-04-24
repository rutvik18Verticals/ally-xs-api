using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{

    /// <summary>
    /// Represents the class for gas lift analysis response.
    /// </summary>
    public class GLAnalysisResponse
    {

        /// <summary>
        /// The node master data object.
        /// </summary>
        public NodeMasterModel NodeMasterData { get; set; }

        /// <summary>
        /// The GL Analysis Result object. 
        /// </summary>
        public GLAnalysisResultModel AnalysisResultEntity { get; set; }

        /// <summary>
        /// The GL Analysis Valve Status object. 
        /// </summary>
        public IList<GLValveStatusModel> ValveStatusEntities { get; set; }

        /// <summary>
        /// The GL Analysis Result object. 
        /// </summary>
        public IList<GLWellValveModel> WellValveEntities { get; set; }

        /// <summary>
        /// The GL Well Orifice Status Model.
        /// </summary>
        public GLWellOrificeStatusModel WellOrificeStatus { get; set; }

        /// <summary>
        /// The GL Well Orifice Status Model.
        /// </summary>
        public GLWellDetailModel WellDetail { get; set; }

        /// <summary>
        /// The test date.
        /// </summary>
        public DateTime TestDate { get; set; }

        /// <summary>
        /// The gas rate phrase.
        /// </summary>
        public string GasRatePhrase { get; set; }

    }
}
