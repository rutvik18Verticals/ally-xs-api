using Newtonsoft.Json;

namespace Theta.XSPOC.Apex.Api.Core.Models.JWTToken
{
    /// <summary>
    /// The JWTAccessToken details.
    /// </summary>
    public class JWTAccessToken
    {

        /// <summary>
        /// Gets and sets the AccessToken.
        /// </summary>
        [JsonProperty("access_token")] 
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets and sets the Expires_in.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets and sets the Refresh_token.
        /// </summary>
        [JsonProperty("refresh_token")] 
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets and sets the UserName.
        /// </summary>
        [JsonProperty("userName")]
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
