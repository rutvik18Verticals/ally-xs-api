namespace Theta.XSPOC.Apex.Api.Core.Common
{

    /// <summary>
    /// 
    /// </summary>
    public class AssetDetails
    {
        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the assetID
        /// </summary>
        public string AssetID { get; set; }

        /// <summary>
        /// Gets or sets the asset name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        public short PocType { get; set; }

        /// <summary>
        /// Gets or sets the application Id.
        /// </summary>
        public int? ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the is enabled property for an asset
        /// </summary>
        public bool IsEnabled { get; set; }

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AssetDetails"/>.
        /// </summary>
        public AssetDetails()
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="AssetDetails"/>.
        /// </summary>
        /// <param name="assetID">The asset id.</param>       
        /// <param name="name">The asset name.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="applicationId">The application id.</param>  
        /// <param name="isEnabled">The application id.</param>  
        public AssetDetails(string assetID, string name, short pocType, int? applicationId, bool isEnabled)
        {
            this.AssetID = assetID;
            this.Name = name;
            this.PocType = pocType;
            this.ApplicationId = applicationId;
            this.IsEnabled = isEnabled;
        }
        #endregion
    }
}