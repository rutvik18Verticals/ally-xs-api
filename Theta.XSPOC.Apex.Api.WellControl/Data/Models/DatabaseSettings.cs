namespace Theta.XSPOC.Apex.Api.WellControl.Data.Models
{
    /// <summary>
    /// Holds the settings for connecting to the XSPOC database. Modeled after the appsettings.json setting for ease.
    /// </summary>
    public class DatabaseSettings
    {

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; } = null!;

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

    }
}
