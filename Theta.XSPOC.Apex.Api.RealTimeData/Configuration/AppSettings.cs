using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.RealTimeData.Configuration
{
    /// <summary>
    /// The AppSettings Details.
    /// </summary>
    public class AppSettings
    {

        /// <summary>
        /// Gets and sets the AudienceSecret.
        /// </summary>
        public string AudienceSecret { get; set; }

        /// <summary>
        /// Gets and sets the Audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets and sets the AudienceIssuer.
        /// </summary>
        public string AudienceIssuer { get; set; }

        /// <summary>
        /// Gets and sets the ExcelContentType.
        /// </summary>
        public string ExcelContentType { get; set; }

        /// <summary>
        /// Gets and sets the AzureBlobStorageURL.
        /// </summary>
        public string AzureBlobStorageURL { get; set; }

        /// <summary>
        /// Gets and sets the AzureBlobStorageAccountName.
        /// </summary>
        public string AzureBlobStorageAccountName { get; set; }

        /// <summary>
        /// Gets and sets the AzureBlobStorageAccountKey.
        /// </summary>
        public string AzureBlobStorageAccountKey { get; set; }

        /// <summary>
        /// Gets and sets the AzureBlobStorageContainer.
        /// </summary>
        public string AzureBlobStorageContainer { get; set; }

        /// <summary>
        /// Gets and sets the KeyVaultURL.
        /// </summary>
        public string KeyVaultURL { get; set; }

        /// <summary>
        /// Gets and sets the TestAccountsMap.
        /// </summary>
        public Dictionary<string, string> TestAccountsMap { get; set; }

        /// <summary>
        /// Gets and sets the TokenExpireInMinutes.
        /// </summary>
        public int TokenExpireInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the ally connect api url.
        /// </summary>
        public string AllyConnectApiURL { get; set; }

    }
}
