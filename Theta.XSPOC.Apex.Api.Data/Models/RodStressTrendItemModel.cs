using System;
using System.ComponentModel.DataAnnotations;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the xdiag rod data model.
    /// </summary>
    public class RodStressTrendItemModel
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the rod number.
        /// </summary>  
        public short? RodNum { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the loading.
        /// </summary>
        public float? Loading { get; set; }

        /// <summary>
        /// Gets or sets the bottom min stress.
        /// </summary>
        public float? BottomMinStress { get; set; }

        /// <summary>
        /// Gets or sets the top min stress.
        /// </summary>
        public float? TopMinStress { get; set; }

        /// <summary>
        /// Gets or sets the top max stress.
        /// </summary>
        public float? TopMaxStress { get; set; }

        /// <summary>
        /// Gets or sets the rod guide id.
        /// </summary>
        public int? RodGuideID { get; set; }

        /// <summary>
        /// Gets or sets the drag friction coefficient.
        /// </summary>
        public float? DragFrictionCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the guide count per rod.
        /// </summary>
        public int? GuideCountPerRod { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

    }

    /// <summary>
    /// Represents the stress component of the rod stress trend item.
    /// </summary>
    public struct Stress
    {

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        public int PhraseId { get; set; }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the column phrase.
        /// </summary>
        public string ColumnPhrase { get; set; }

    }
}
