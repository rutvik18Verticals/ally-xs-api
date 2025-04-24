using System;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Describes a card date value that needs to be send out.
    /// </summary>
    public class CardDateValue
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
