using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The esp tornado  curve set annotations table.
    /// </summary>
    [Table("tblESPTornadoCurveSetAnnotations")]
    public class ESPTornadoCurveSetAnnotationsEntity
    {

        /// <summary>
        /// Gets or sets the curve set member id.
        /// </summary>
        [Key]
        public int CurveSetMemberId { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        public double Frequency { get; set; }

    }
}
