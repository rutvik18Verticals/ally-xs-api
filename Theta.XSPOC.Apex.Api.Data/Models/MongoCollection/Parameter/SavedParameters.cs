using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter
{
    /// <summary>
    /// Represents a collection of Saved Parameters.
    /// </summary>
    public class SavedParameters : DocumentBase
    {

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets Address
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// Gets or sets BackupDate
        /// </summary>
        public DateTime? BackupDate { get; set; }

        /// <summary>
        /// Gets or sets the legacy id's.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

    }
}

