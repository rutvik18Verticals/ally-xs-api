using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represent the class for rod lift analysis data.
    /// </summary>
    public class RodLiftAnalysisResponse
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
        /// The well details object.
        /// </summary>
        public WellDetailsModel WellDetails { get; set; }

        /// <summary>
        /// The card data object.
        /// </summary>
        public CardDataModel CardData { get; set; }

        /// <summary>
        /// The well test data.
        /// </summary>
        public WellTestModel WellTestData { get; set; }

        /// <summary>
        /// The node master data object.
        /// </summary>
        public NodeMasterModel NodeMasterData { get; set; }

        /// <summary>
        /// The current raw scan data object.
        /// </summary>
        public IList<CurrentRawScanDataModel> CurrentRawScanData { get; set; }

        /// <summary>
        /// The XDiagResults object
        /// </summary>
        public XDiagResultsModel XDiagResults { get; set; }

        /// <summary>
        /// The pumping unit manufacturer. 
        /// </summary>
        public string PumpingUnitManufacturer { get; set; }

        /// <summary>
        /// The pumping unit API designation
        /// </summary>
        public string PumpingUnitAPIDesignation { get; set; }

        /// <summary>
        /// The system parameter value
        /// </summary>
        public string SystemParameters { get; set; }

    }
}
