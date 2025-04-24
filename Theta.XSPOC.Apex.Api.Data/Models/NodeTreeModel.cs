namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class representing groups or assets from the node tree.
    /// </summary>
    public class NodeTreeModel
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        public string Node { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Gets or sets the number of descendants.
        /// </summary>
        public int NumDescendants { get; set; }

    }
}
