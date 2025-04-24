using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The analysis curves set member table.
    /// </summary>
    [Table("tblAnalysisCurveSetMembers")]
    public class AnalysisCurveSetMembersEntity
    {

        /// <summary>
        /// Gets or sets curve set id.
        /// </summary>
        public int CurveSetId { get; set; }

        /// <summary>
        /// Gets or sets curve set member id.
        /// </summary>
        [Key]
        public int CurveSetMemberId { get; set; }

    }
}
