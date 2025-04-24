using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.AssetStatus
{
    /// <summary>
    /// This is the contract the UI uses to display the overlay, communication, exceptions, alarm, last well test,
    /// and status register data for the rod lift artificial lift type.
    /// </summary>
    public class AssetStatusDataValue : AssetStatusReponseBase
    {

        /// <summary>
        /// Gets or sets the rod string as an <seealso cref="IList{PropertyValueContract}"/> of overlay status.
        /// </summary>
        public IList<PropertyValueContract> RodStrings { get; set; }

        /// <summary>
        /// Gets or sets status registers information as an <seealso cref="IList{PropertyValueContract}"/> of overlay status.
        /// </summary>
        public IList<PropertyValueContract> StatusRegisters { get; set; }

        /// <summary>
        /// Gets or sets the gl analysis information data.
        /// </summary>
        public IList<PropertyValueContract> GLAnalysisData { get; set; }

        /// <summary>
        /// Gets or sets the chemical injection information.
        /// </summary>
        public IList<PropertyValueContract> ChemicalInjectionInformation { get; set; }

        /// <summary>
        /// Gets or sets the plunger lift data.
        /// </summary>
        public IList<PropertyValueContract> PlungerLiftData { get; set; }

        /// <summary>
        /// Gets or sets the facility tag data.
        /// </summary>
        public IList<FacilityTagGroupDataOutput> FacilityTagData { get; set; }

        /// <summary>
        /// Gets or sets the PID tag data.
        /// </summary>
        public PIDDataOutput PIDData { get; set; }

        /// <summary>
        /// Gets or sets the valve control data.
        /// </summary>
        public ValveControlDataResponse ValveControlData { get; set; }

        /// <summary>
        /// Gets or sets the refresh interval in seconds.
        /// </summary>
        public int RefreshInterval { get; set; }

        /// <summary>
        /// Gets or sets the smarten live url.
        /// </summary>
        public string SmartenLiveUrl { get; set; }

    }
}
