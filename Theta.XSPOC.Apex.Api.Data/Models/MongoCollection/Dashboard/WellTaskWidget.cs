using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// This class defines the Well Test Widget MongoDB sub document.
    /// </summary>
    [BsonDiscriminator("TaskDetail")]
    public class TaskDetail : WidgetType
    {
        /// <summary>
        /// The list of fixed columns.
        /// </summary>
        public List<FixedColumns> FixedColumns { get; set; }

        /// <summary>
        /// The list of other columns.
        /// </summary>
        public List<Columns> Columns { get; set; }
    }   
}
