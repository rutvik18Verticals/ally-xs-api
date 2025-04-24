using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The represents card date model.
    /// </summary>
    public class CardDateModel
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
        /// The cause id.
        /// </summary>
        public int? CauseId { get; set; }

        /// <summary>
        /// The poc type.
        /// </summary>
        public short PocType { get; set; }

    }
}
