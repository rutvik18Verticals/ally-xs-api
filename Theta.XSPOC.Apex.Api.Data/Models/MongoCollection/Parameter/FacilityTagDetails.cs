using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter
{
    /// <summary>
    /// Represents the details of a facility tag.
    /// </summary>
    /// 
    [BsonKnownTypes(typeof(FacilityTagDetails))]
    public class FacilityTagDetails : ParameterDocumentBase
    {

        /// <summary>
        /// Gets or sets the asset ID associated with the facility tag.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the trend type of the facility tag.
        /// </summary>
        public int? TrendType { get; set; }

        /// <summary>
        /// Gets or sets the engineering units of the facility tag.
        /// </summary>
        public string EngUnits { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the facility tag is writeable.
        /// </summary>
        public bool Writeable { get; set; }

        /// <summary>
        /// Gets or sets the trend to asset of the facility tag.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string TrendToAsset { get; set; }

        /// <summary>
        /// Gets or sets the display order of the facility tag.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the well group name of the facility tag.
        /// </summary>
        public string WellGroupName { get; set; }

        /// <summary>
        /// Gets or sets the bit value of the facility tag.
        /// </summary>
        public int Bit { get; set; }

        /// <summary>
        /// Gets or sets the capture type of the facility tag.
        /// </summary>
        public int CaptureType { get; set; }

        /// <summary>
        /// Gets or sets the last capture date of the facility tag.
        /// </summary>
        public DateTime? LastCaptureDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the facility tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the facility tag group ID associated with the facility tag.
        /// </summary>
        public int? FacilityTagGroupID { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the facility tag.
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the facility tag.
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the camera ID associated with the facility tag.
        /// </summary>
        public int? CameraID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the facility tag is for detail view only.
        /// </summary>
        public bool? DetailViewOnly { get; set; }

        /// <summary>
        /// Gets or sets the expression of the facility tag.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the topic of the facility tag.
        /// </summary>
        public string Topic { get; set; }

    }
}
