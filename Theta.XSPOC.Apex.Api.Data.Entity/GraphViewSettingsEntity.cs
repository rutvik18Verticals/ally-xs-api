using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GL Well Valves database table.
    /// </summary>
    [Table("tblGraphViewSettings")]
    public class GraphViewSettingsEntity
    {

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        [Key]
        [Column("ViewID")]
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        [Column("Property")]
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Column("Value")]
        public string Value { get; set; }

    }
}
