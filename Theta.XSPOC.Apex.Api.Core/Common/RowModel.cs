using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the RowModel.
    /// </summary>
    public class RowModel
    {

        /// <summary>
        /// Get and set the Columns.
        /// </summary>
        public IList<RowColumnModel> Columns { get; set; }

        /// <summary>
        /// Get and set the Common.
        /// </summary>
        public IDictionary<string, object> Common { get; set; } = new Dictionary<string, object>();

    }
}
