using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Strings
{
    /// <summary>
    /// Represents a Strings.
    /// </summary>
    public class Route : DocumentBase
    {
        /// <summary>
        /// Gets or sets the StringID
        /// </summary>
        public int StringID { get; set; }

        /// <summary>
        /// Gets or sets the String Name
        /// </summary>
        public string StringName { get; set; }

        /// <summary>
        /// Gets or sets the ContactListID.
        /// </summary>
        public int? ContactListID { get; set; }

        /// <summary>
        /// Gets or sets the ResponderListID.
        /// </summary>
        public int? ResponderListID { get; set; }

        /// <summary>
        /// Gets or sets the legacy Id of the Strngs.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

    }
}
