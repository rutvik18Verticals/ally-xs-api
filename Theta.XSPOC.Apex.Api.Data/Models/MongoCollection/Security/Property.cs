namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security
{
    /// <summary>
    /// This class defines the property sub document of a <seealso cref="Preference"/> MongoDB sub document.
    /// </summary>
    public class Property
    {

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

    }
}