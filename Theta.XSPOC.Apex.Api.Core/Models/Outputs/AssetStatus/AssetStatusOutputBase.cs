using System.Collections;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs.AssetStatus
{
    /// <summary>
    /// This is the contract base the UI uses to display the common elements in the overlay, communication,
    /// exceptions, alarm, last well test, and status register data for the artificial lift types.
    /// </summary>
    public class AssetStatusOutputBase
    {

        /// <summary>
        /// Gets or sets alarms as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueOutput> Alarms { get; set; }

        /// <summary>
        /// Gets or sets exceptions as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueOutput> Exceptions { get; set; }

        /// <summary>
        /// Gets or sets the image overlay items.
        /// </summary>
        public IList<OverlayStatusDataOutput> ImageOverlayItems { get; set; }

        /// <summary>
        /// Gets or sets last well test information as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueOutput> LastWellTest { get; set; }

        /// <summary>
        /// Gets or sets well status overview as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueOutput> WellStatusOverview { get; set; }

        /// <summary>
        /// Gets or sets the defaults diagram type.
        /// </summary>
        public int DiagramType { get; set; }

        /// <summary>
        /// Gets or sets the gl analysis information data.
        /// </summary>
        public IList<PropertyValueOutput> GLAnalysisData { get; set; }

        /// <summary>
        /// Gets or sets the chemical injection information.
        /// </summary>
        public IList<PropertyValueOutput> ChemicalInjectionInformation { get; set; }

        /// <summary>
        /// Gets or sets the plunger lift data.
        /// </summary>
        public IList<PropertyValueOutput> PlungerLiftData { get; set; }

        /// <summary>
        /// Gets or sets the facility tag data.
        /// </summary>
        public IList<FacilityTagGroupDataOutput> FacilityTagData { get; set; }

        /// <summary>
        /// Gets or sets the PID data.
        /// </summary>
        public PIDDataOutput PIDData { get; set; }

        /// <summary>
        /// Gets or sets the valve control data.
        /// </summary>
        public ValveControlDataOutput ValveControlData { get; set; }

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