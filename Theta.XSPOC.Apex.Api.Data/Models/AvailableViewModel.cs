namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the available view model.
    /// </summary>
    public class AvailableViewModel
    {

        /// <summary>
        /// Gets or sets the view id.
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
