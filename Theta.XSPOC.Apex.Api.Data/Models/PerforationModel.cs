using System.ComponentModel.DataAnnotations;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the PerforationModel.
    /// </summary>
    public class PerforationModel
    {

        /// <summary>
        /// Gets and sets the NodeId.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets and sets the TopDepth.
        /// </summary>
        [Key]
        public float TopDepth { get; set; }

        /// <summary>
        /// Gets and sets the Length.
        /// </summary>
        public float Length { get; set; }

        /// <summary>
        /// Gets and sets the Diameter.
        /// </summary>
        public float? Diameter { get; set; }

        /// <summary>
        /// Gets and sets the HoleCountPerUnit.
        /// </summary>
        public short? HoleCountPerUnit { get; set; }

    }
}
