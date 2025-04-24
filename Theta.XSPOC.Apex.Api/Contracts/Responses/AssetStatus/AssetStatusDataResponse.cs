namespace Theta.XSPOC.Apex.Api.Contracts.Responses.AssetStatus
{
    /// <summary>
    /// This is the contract the UI uses to display the overlay, communication, exceptions, alarm, last well test,
    /// and status register data for the rod lift artificial lift type.
    /// </summary>
    public class AssetStatusDataResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the rod list asset status data values.
        /// </summary>
        public AssetStatusDataValue Values { get; set; }

    }
}