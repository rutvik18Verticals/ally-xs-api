namespace Theta.XSPOC.Apex.Api.Contracts
{
    /// <summary>
    /// This is the contract the UI uses to display the overlay data.
    /// </summary>
    public class OverlayStatusDataContract : PropertyValueContract
    {

        /// <summary>
        /// Gets or sets the overlay field enum value.
        /// </summary>
        public OverlayFields OverlayField { get; set; }

    }
}
