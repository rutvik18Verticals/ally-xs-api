using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Base class for common post processing data.
    /// </summary>
    public class PostProcessingBase
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the communication status.
        /// </summary>
        public string CommunicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the data that should be saved to saved parameters. The dictionary key is
        /// the address and the value is the value to save.
        /// </summary>
        public IList<IDictionary<int, float>> DataToSave { get; set; }

    }
}
