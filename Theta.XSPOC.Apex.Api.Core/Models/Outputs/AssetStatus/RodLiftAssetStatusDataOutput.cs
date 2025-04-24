using System.Collections;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs.AssetStatus
{
    /// <summary>
    /// This is the contract the UI uses to display the overlay, communication, exceptions, alarm, last well test,
    /// and status register data for the rod lift artificial lift type.
    /// </summary>
    public class RodLiftAssetStatusDataOutput : AssetStatusOutputBase
    {

        /// <summary>
        /// Gets or sets the rod string as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueOutput> RodStrings { get; set; }

        /// <summary>
        /// Gets or sets status registers information as an <seealso cref="IList"/> of overlay status.
        /// </summary>
        public IList<PropertyValueOutput> StatusRegisters { get; set; }

    }
}