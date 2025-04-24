using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for card coordinate data.
    /// </summary>
    public class CardCoordinateDataOutput : CoreOutputBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the response id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the response date and time.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the response values.
        /// </summary>
        public IList<CardResponseValuesOutput> Values { get; set; }

        #endregion

    }
}
