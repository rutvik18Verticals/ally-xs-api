using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security
{
    /// <summary>
    /// This class defines the role MongoDB document.
    /// </summary>
    public class Role : DocumentBase
    {

        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public IList<string> Permissions { get; set; }

    }
}