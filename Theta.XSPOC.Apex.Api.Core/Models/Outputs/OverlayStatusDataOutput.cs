
namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// This is the contract the UI uses to display the overlay data.
    /// </summary>
    public class OverlayStatusDataOutput : PropertyValueOutput
    {

        /// <summary>
        /// Gets or sets the overlay field enum value.
        /// </summary>
        public OverlayFields OverlayField { get; set; }

    }
}
