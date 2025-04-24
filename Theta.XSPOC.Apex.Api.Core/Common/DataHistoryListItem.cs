using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the data history trends response.
    /// </summary>
    public class DataHistoryListItem
    {

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the TypeId.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="DataHistoryListItem"/> items.
        /// </summary>
        public IList<DataHistoryListItem> Items { get; set; } = new List<DataHistoryListItem>();

    }
}
