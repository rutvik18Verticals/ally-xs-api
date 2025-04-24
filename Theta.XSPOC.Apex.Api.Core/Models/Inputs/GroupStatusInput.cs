namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Describes a groupstatus request received.
    /// </summary>
    public class GroupStatusInput
    {

        /// <summary>
        /// Gets or sets the ViewId.
        /// </summary>
        public string ViewId { get; set; }

        /// <summary>
        /// Gets or sets the Group Name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public string[] AssetId { get; set; }

    }
}
