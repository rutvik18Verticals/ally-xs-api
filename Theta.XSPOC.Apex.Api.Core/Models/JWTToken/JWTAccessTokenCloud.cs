namespace Theta.XSPOC.Apex.Api.Core.Models.JWTToken
{
    /// <summary>
    /// The JWTAccessToken details for Cloud.
    /// </summary>
    public class JWTAccessTokenCloud
    {

        /// <summary>
        /// Gets and sets the AccessToken.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets and sets the Expires_in.
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets and sets the Refresh_token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets and sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets if the user is a first time login.
        /// </summary>
        public bool IsFirstTimeLogin { get; set; }

        /// <summary>
        /// Gets or sets the value which indicates user is admin or not.
        /// </summary>
        public bool IsAdmin { get; set; }

    }
}
