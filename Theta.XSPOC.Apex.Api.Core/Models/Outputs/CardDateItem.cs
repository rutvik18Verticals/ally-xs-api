using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents a card date item.
    /// </summary>
    public class CardDateItem
    {

        /// <summary>
        /// The date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The card type id.
        /// </summary>
        public string CardTypeId { get; set; }

        /// <summary>
        /// The card type name.
        /// </summary>
        public string CardTypeName { get; set; }

    }
}
