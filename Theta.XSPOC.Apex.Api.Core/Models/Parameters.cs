using Newtonsoft.Json;

namespace Theta.XSPOC.Apex.Api.Core.Models
{
    /// <summary>
    /// The Parameters Details.
    /// </summary>
    public class Parameters
    {

        /// <summary>
        /// Gets and sets the Grant_type.
        /// </summary>
        [JsonProperty("grantType")]
        public string GrantType { get; set; } = null!;

        /// <summary>
        /// Gets and sets the Refresh_token.
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; } = null!;

        /// <summary>
        /// Gets and sets the Reference.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; } = null!;

    }
}
