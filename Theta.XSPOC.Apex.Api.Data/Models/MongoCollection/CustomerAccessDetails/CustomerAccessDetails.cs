using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.CustomerAccessDetails
{ 
    /// <summary>
    /// A customer access details class with properties that are common to all Document models tied to a customer access.
    /// </summary>
    public class CustomerAccessDetails : DocumentBase
    {
        /// <summary>
        /// Gets or sets the customer guid.
        /// </summary>
        public string CustomerGUID { get; set; }

        /// <summary>
        /// Gets or sets the API Token key
        /// </summary>
        public string APITokenKey { get; set; }

        /// <summary>
        /// Gets or sets the API Token value
        /// </summary>
        public string APITokenValue { get; set; }

        /// <summary>
        /// Gets or sets the Expiration Date of the token
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the customer guid.
        /// </summary>
        public string UserAccount { get; set; }
    }
}
