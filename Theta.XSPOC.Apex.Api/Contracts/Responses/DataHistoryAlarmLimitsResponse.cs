using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// The response for the data history alarm limit.
    /// </summary>
    public class DataHistoryAlarmLimitsResponse : ResponseBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the response values.
        /// </summary>
        public IList<DataHistoryAlarmLimitsValues> Values { get; set; }

        #endregion

    }
}
