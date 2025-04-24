namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a model for the rod within group status.
    /// </summary>
    public class RodForGroupStatusModel
    {

        /// <summary>
        /// Gets or sets the node Id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the rod number.
        /// </summary>
        public short RodNum { get; set; }

        /// <summary>
        /// Gets or sets the name of the rod grade.
        /// </summary>
        public string Name { get; set; }

    }
}
