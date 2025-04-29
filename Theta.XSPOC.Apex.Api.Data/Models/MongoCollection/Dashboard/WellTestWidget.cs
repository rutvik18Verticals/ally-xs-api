using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// This class defines the Well Test Widget MongoDB sub document.
    /// </summary>
    [BsonDiscriminator("WellTest")]
    public class WellTest : WidgetType
    {
        /// <summary>
        /// 
        /// </summary>
        public List<FixedColumns> FixedColumns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Columns> Columns { get; set; }
    }

    /// <summary>
    /// Fixed Column Model.
    /// </summary>
    public class FixedColumns
    {
        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the column label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the key for retrival from api.
        /// </summary>
        public string Key { get; set; }

    }

    /// <summary>
    /// Column Model.
    /// </summary>
    public class Columns
    {
        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the column label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the key for retrival from api.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets flag if it is selected.
        /// </summary>
        public bool Selected { get; set; }

    }
}
