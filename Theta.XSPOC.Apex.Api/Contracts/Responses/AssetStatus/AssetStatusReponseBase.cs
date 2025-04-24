using System.Collections;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.AssetStatus
{
    /// <summary>
    /// This is the contract base the UI uses to display the common elements in the overlay, communication,
    /// exceptions, alarm, last well test, and status register data for the artificial lift types.
    /// </summary>
    public class AssetStatusReponseBase
    {

        /// <summary>
        /// Gets or sets alarms as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueContract> Alarms { get; set; }

        /// <summary>
        /// Gets or sets exceptions as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueContract> Exceptions { get; set; }

        /// <summary>
        /// Gets or sets the image overlay items.
        /// </summary>
        public IList<OverlayStatusDataContract> ImageOverlayItems { get; set; }

        /// <summary>
        /// Gets or sets last well test information as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueContract> LastWellTest { get; set; }

        /// <summary>
        /// Gets or sets well status overview as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueContract> WellStatusOverview { get; set; }

        /// <summary>
        /// Gets or sets well status overview as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public int DiagramType { get; set; }

    }
}