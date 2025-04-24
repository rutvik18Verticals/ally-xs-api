using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the LoadViewRowOutput.
    /// </summary>
    public class LoadViewRowResult
    {

        /// <summary>
        /// Gets and sets the GroupStatusColumns.
        /// </summary>
        public IList<GroupStatusColumns> GroupStatusColumns { get; set; }

        /// <summary>
        /// Gets and sets the ColumnOverrides.
        /// </summary>
        public IList<ColumnOverride> ColumnOverrides { get; set; } = new List<ColumnOverride>();

        /// <summary>
        /// Gets and sets the Rows.
        /// </summary>
        public IList<RowModel> Rows { get; set; }

    }
}
