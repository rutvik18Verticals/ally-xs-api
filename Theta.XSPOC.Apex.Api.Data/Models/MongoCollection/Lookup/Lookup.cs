using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// The lookup document for the lookup collection.
    /// </summary>
    public class Lookup : DocumentBase
    {

        /// <summary>
        /// Gets or sets the legacy primary keys.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

        /// <summary>
        /// Gets or sets the lookup type.
        /// </summary>
        public string LookupType { get; set; }

        /// <summary>
        /// Gets or sets the lookup document that is based on lookup type.
        /// </summary>
        public LookupBase LookupDocument { get; set; }

    }
}
