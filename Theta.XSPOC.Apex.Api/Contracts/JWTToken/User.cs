namespace Theta.XSPOC.Apex.Api.Contracts.JWTToken
{
    /// <summary>
    /// The User Details.
    /// </summary>
    public class User
    {

        /// <summary>
        /// Gets and sets the Grant_type.
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// Gets and sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the PasswordHash.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets and sets the RefreshToken.
        /// </summary>
        public string RefreshToken { get; set; }

    }
}
