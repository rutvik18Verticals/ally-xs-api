namespace Theta.XSPOC.Apex.Api.Contracts.JWTToken
{
    /// <summary>
    /// Represents the AdminToolsJwtAuthenticationOptions.
    /// </summary>
    public class AdminToolsJwtAuthenticationOptions
    {

        /// <summary>
        /// Gets and sets the AudienceSecret.
        /// </summary>
        public string AudienceSecret { get; set; }

        /// <summary>
        /// Gets and sets the AudienceIssuer.
        /// </summary>
        public string AudienceIssuer { get; set; }

        /// <summary>
        /// Gets and sets the Audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets and sets the Module.
        /// </summary>
        public AdminToolsModule Module { get; set; }

        /// <summary>
        /// Gets and sets the TimeOut.
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// Gets and set the use secure cookies.
        /// </summary>
        public bool UseSecureCookies { get; set; }

    }
}
