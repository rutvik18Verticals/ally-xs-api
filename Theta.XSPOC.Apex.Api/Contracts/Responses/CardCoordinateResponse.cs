using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// The response for the card coordinate.
    /// </summary>
    public class CardCoordinateResponse : ResponseBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the response values.
        /// </summary>
        public IList<CardResponseValues> Values { get; set; } = new List<CardResponseValues>();

        #endregion

    }
}
