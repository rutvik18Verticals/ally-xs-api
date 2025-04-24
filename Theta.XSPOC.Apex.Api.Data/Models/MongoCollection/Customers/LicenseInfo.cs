using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers
{
    /// <summary>
    /// A LicenseInfo models tied to a customer.
    /// </summary>
    public class LicenseInfo
    {

        /// <summary>
        /// Gets or sets the LicenseType.
        /// </summary>
        public string LicenseType { get; set; }

        /// <summary>
        /// Gets or sets the ExpirationDate.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

    }
}
