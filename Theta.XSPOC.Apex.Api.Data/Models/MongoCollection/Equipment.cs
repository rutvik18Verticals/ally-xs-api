using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// This asset class to store all equipment in the Mongo DB Store. In particular, it represents the
    /// Equipment model that would be saved to Mongo as a document. This uses the polymorphic pattern and the
    /// specification property contains the equipment details.
    /// </summary>
    public class Equipment : DocumentBase
    {

        /// <summary>
        /// Gets or sets the type of the equipment 
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public EquipmentType Type { get; set; }

        /// <summary>
        /// Gets or sets the legacy id's used for lookups.
        /// </summary>
        public IDictionary<string, string> LegacyIds { get; set; }

        /// <summary>
        /// Gets or sets the equipment specification. This is the sub document that defines the equipment.
        /// </summary>
        public EquipmentSpecification Specification { get; set; }

    }
}