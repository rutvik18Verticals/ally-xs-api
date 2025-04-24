using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers
{
    /// <summary>
    /// A customer class with properties that are common to all Document models tied to a customer.
    /// </summary>
    public class Customer : DocumentBase
    {

        /// <summary>
        /// Gets or sets the connexia customer id.
        /// </summary>
        public int? CNX_CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the legacy id's.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

        /// <summary>
        /// Gets or sets the isActive.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the B2Cdomainname.
        /// </summary>
        public string B2Cdomainname { get; set; }

        /// <summary>
        /// Gets or sets the GrafanaEditor.
        /// </summary>
        public string GrafanaEditor { get; set; }

        /// <summary>
        /// Gets or sets the License.
        /// </summary>
        public List<LicenseInfo> License { get; set; }

        /// <summary>
        /// Gets or sets the list of system parameters for a customer.
        /// </summary>
        public IList<SystemParameter> SystemParameters { get; set; }

    }
}
