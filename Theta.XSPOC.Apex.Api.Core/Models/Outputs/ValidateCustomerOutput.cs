using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{/// <summary>
 /// Represent the class for validate customer details output response.
 /// </summary>
    public class ValidateCustomerOutput : CoreOutputBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="ValidateCustomer"/> values.
        /// </summary>
        public List<ValidateCustomer> Values { get; set; }
    }
}
