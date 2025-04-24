using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// This is the POCType class for equipment specifications.
    /// </summary>
    public class POCTypeSpecification : EquipmentSpecification
    {

        /// <summary>
        /// Gets or sets the poc type description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets if the poc type is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the icon location.
        /// </summary>
        // todo Is this needed anymore?
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the protocol id.
        /// </summary>
        public int? ProtocolId { get; set; }

        /// <summary>
        /// Gets or sets if the poc type is a master poc type.
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// Gets or sets the master POC type id.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string MasterPOCTypeId { get; set; }

    }
}