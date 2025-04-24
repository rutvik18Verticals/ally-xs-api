namespace Theta.XSPOC.Apex.Api.Users.Contracts.Requests
{
    /// <summary>
    /// Represents the inputs for user default.
    /// </summary>
    public class UserDefaultRequest
    {

        #region Properties

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        #endregion

    }
}
