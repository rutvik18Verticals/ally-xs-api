using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security
{
    /// <summary>
    /// This class defines the preference MongoDB sub document.
    /// </summary>
    public class Preference
    {

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the property item.
        /// </summary>
        public IList<Property> PropertyItem { get; set; }

    }
}