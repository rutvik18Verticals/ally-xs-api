using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter
{
    /// <summary>
    /// Represents a collection of parameters.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ParametersBase : DocumentBase
    {

        /// <summary>
        /// Gets or sets if the parameter is deleted or not.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the product type.
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the parameter type.
        /// </summary>
        public string ParameterType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the POC type.
        /// </summary>
        public Lookup.Lookup POCType { get; set; }

        /// <summary>
        /// Gets or sets the channel Id.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the series
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets a value is enabled or not.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the number of decimal places.
        /// </summary>
        public int Decimals { get; set; }

        /// <summary>
        /// Gets or sets the group status view.
        /// </summary>
        public int? GroupStatusView { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the destination type.
        /// </summary>
        public int? DestinationType { get; set; }

        /// <summary>
        /// Gets or sets the parameter standard type.
        /// </summary>
        public Lookup.Lookup ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the archive function.
        /// </summary>
        public int ArchiveFunction { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public Lookup.Lookup UnitType { get; set; }

        /// <summary>
        /// Gets or sets the data type.
        /// </summary>
        public Lookup.Lookup DataType { get; set; }

        /// <summary>
        /// Gets or sets the state ID.
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// Gets or sets the legacy Id of the asset.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

        /// <summary>
        /// Gets of sets the unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; }

    }
}
