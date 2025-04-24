namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for available view  data
    /// </summary>
    public class AvailableViewData
    {

        /// <summary>
        /// The view id.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// The view name.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// The selected.
        /// </summary>
        public bool IsSelectedView { get; set; }

    }
}
