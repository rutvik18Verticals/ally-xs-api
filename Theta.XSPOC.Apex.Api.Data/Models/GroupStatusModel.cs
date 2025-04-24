using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the ResponseValues.
    /// </summary>
    public class GroupStatusModel
    {

        /// <summary>
        /// Get and sets the columns.
        /// </summary>
        public IList<GroupStatusColumnModel> Columns { get; set; }

        /// <summary>
        /// Get and sets the Rows.
        /// </summary>
        public IList<IList<GroupStatusRowModel>> Rows { get; set; }

    }
}
