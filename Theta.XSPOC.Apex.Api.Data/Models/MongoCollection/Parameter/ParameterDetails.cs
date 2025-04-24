using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter
{
    /// <summary>
    /// Represents a collection of parameter details.
    /// </summary>
    [BsonKnownTypes(typeof(ParameterDetails))]
    public class ParameterDetails : ParameterDocumentBase
    {

        /// <summary>
        /// Gets or sets the access of the parameter.
        /// </summary>
        public string Access { get; set; }

        /// <summary>
        /// Gets or sets the scale factor of the parameter.
        /// </summary>
        public float? ScaleFactor { get; set; }

        /// <summary>
        /// Gets or sets the offset of the parameter.
        /// </summary>
        public float? Offset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is a setpoint.
        /// </summary>
        public bool Setpoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is included in status scan.
        /// </summary>
        public bool StatusScan { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is included in data collection.
        /// </summary>
        public bool DataCollection { get; set; }

        /// <summary>
        /// Gets or sets the collection mode of the parameter.
        /// </summary>
        public int CollectionMode { get; set; }

        /// <summary>
        /// Gets or sets the setpoint group of the parameter.
        /// </summary>
        public int? SetpointGroup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is locked.
        /// </summary>
        public bool? Locked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is included in fast scan.
        /// </summary>
        public bool? FastScan { get; set; }

        /// <summary>
        /// Gets or sets the earliest supported version of the parameter.
        /// </summary>
        public float EarliestSupportedVersion { get; set; }

        /// <summary>
        /// Get or sets the PhraseId
        /// </summary>
        public int? PhraseId { get; set; }
    }
}
