using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset
{
    /// <summary>
    /// A class with properties that are common to all assets.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Asset : CustomerDocumentBase
    {

        /// <summary>
        /// Gets or sets the connexia site id.
        /// </summary>
        public int? CNX_SiteId { get; set; }

        /// <summary>
        /// Gets or sets the connexia well id.
        /// </summary>
        public int? CNX_WellId { get; set; }

        /// <summary>
        /// Gets or sets the legacy Id of the asset.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the asset. This is the node id from the legacy system.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the contact group id.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string RouteId { get; set; }

        /// <summary>
        /// Gets or sets the group ids.
        /// </summary>
        public IList<string> GroupIds { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        public Lookup.Lookup POCType { get; set; }

        /// <summary>
        /// Gets or sets the artificial lift type. This will be GasLift, ESP, RodPump, PlungerLift,, PCP, etc. This is the string value
        /// from the POCTypeApplication <seealso cref="Lookup.Lookup"/>.
        /// </summary>
        public string ArtificialLiftType { get; set; }

        /// <summary>
        /// Gets or sets the configuration of the asset.
        /// </summary>
        public AssetConfig AssetConfig { get; set; }

        /// <summary>
        /// Gets or sets the casing for the asset.
        /// </summary>
        public IList<Casing> Casings { get; set; }

        /// <summary>
        /// Gets or sets the tubing for the asset.
        /// </summary>
        public IList<Tubing> Tubings { get; set; }

        /// <summary>
        /// Gets or sets the asset details.
        /// </summary>
        public AssetDetailBase AssetDetails { get; set; }

    }
}
