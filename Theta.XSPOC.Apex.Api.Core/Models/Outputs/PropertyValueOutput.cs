
namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// This is the contract the UI uses to display the property value data.
    /// </summary>
    public class PropertyValueOutput
    {

        /// <summary>
        /// Gets or sets the label used in the overlay
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the value used in the overlay.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets if the data should be shown.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the display status state to be used by the UI for color scheme.
        /// </summary>
        public DisplayState DisplayState { get; set; }

    }
}