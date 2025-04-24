namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// This represents the validate customer Input.
    /// </summary>
    public class ValidateCustomerInput
    {
        /// <summary>
        /// Customer Id
        /// </summary>      
        public string CustomerId { get; set; }

        /// <summary>
        /// Customer API Token Key
        /// </summary>      
        public string TokenKey { get; set; }

        /// <summary>
        /// Customer API Token Value
        /// </summary>      
        public string TokenValue { get; set; }

        /// <summary>
        /// Customer Id
        /// </summary>      
        public string UserAccount { get; set; }
    }

}

