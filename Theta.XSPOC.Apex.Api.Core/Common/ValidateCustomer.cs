namespace Theta.XSPOC.Apex.Api.Core.Common
{

    /// <summary>
    /// 
    /// </summary>
    public class ValidateCustomer
    {
        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the UserAccount
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// Gets or sets the weather provided token is valid
        /// </summary>
        public bool IsValid { get; set; }

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="ValidateCustomer"/>.
        /// </summary>
        public ValidateCustomer()
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="ValidateCustomer"/>.
        /// </summary>
        /// <param name="userAccount">The User Account.</param>       
        /// <param name="isValid">Indicates if provided token is valid.</param>        
        public ValidateCustomer(string userAccount, bool isValid)
        {
            this.UserAccount = userAccount;
            this.IsValid = isValid;            
        }
        #endregion
    }
}
