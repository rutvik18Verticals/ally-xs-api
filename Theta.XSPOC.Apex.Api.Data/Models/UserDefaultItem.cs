namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Gets or sets the default item for a user, property, and default group.
    /// </summary>
    public record UserDefaultItem
    {

        /// <summary>
        /// Gets or sets the defaults group for the user default.
        /// </summary>
        public string DefaultsGroup { get; set; }

        /// <summary>
        /// Gets or sets the property for the user default.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the user id for the user default.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the value for the user default.
        /// </summary>
        public string Value { get; set; }

    }
}