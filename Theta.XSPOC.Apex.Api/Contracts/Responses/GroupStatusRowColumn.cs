namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Represents a row in the group status.
    /// </summary>
    public class GroupStatusRowColumn
    {

        /// <summary>
        /// Gets or sets the ColumnId.
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the BackColor.
        /// </summary>
        public string BackColor { get; set; }

        /// <summary>
        /// Gets or sets the ForeColor.
        /// </summary>
        public string ForeColor { get; set; }

    }
}
