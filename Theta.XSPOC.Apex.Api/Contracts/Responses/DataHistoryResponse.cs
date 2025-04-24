using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a trend data response that needs to be send out.
    /// </summary>
    public class DataHistoryResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<DataHistoryListItem> Values { get; set; }

    }
}