using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers
{
    /// <summary>
    /// A system parameter class with properties for system parameters.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class SystemParameter
    {

        /// <summary>
        /// Gets or sets the parameter name
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the parameter comment.
        /// </summary>
        public string Comment { get; set; }

    }
}
