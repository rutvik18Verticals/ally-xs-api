using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the enabled status data for a well.
    /// </summary>
    public class AppplicationSettingsOutput : CoreOutputBase
    {
        /// <summary>
        /// Application Settings key and value pairs
        /// </summary>
        public Dictionary<string, string> ApplicationSettings { get; set; }

    }
}
