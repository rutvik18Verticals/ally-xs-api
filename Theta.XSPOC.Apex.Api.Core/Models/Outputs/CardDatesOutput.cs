using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for card dates.
    /// </summary>
    public class CardDatesOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public IList<CardDateItem> Values { get; set; } = new List<CardDateItem>();

    }
}
