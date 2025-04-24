using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The curve set coordinates table.
    /// </summary>
    [Table("tblCurveSetCoordinates")]
    public class CurveSetCoordinatesEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the curve id.
        /// </summary>
        [Column("CurveID")]
        public int CurveId { get; set; }

    }
}
