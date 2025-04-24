namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the model for Group Status Columns.
    /// </summary>
    public class GroupStatusColumnsModels
    {

        /// <summary>
        /// Gets or sets the view Id.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the user Id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the column Id.
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public int Orientation { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the source Id.
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        public int? Align { get; set; }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        public int? Visible { get; set; }

        /// <summary>
        /// Gets or sets the measure.
        /// </summary>
        public string Measure { get; set; }

        /// <summary>
        /// Gets or sets the format Id.
        /// </summary>
        public int FormatId { get; set; }

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Gets or sets the parameter standard type.
        /// </summary>
        public int? ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the format mask.
        /// </summary>
        public string FormatMask { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public int UnitType { get; set; }

    }
}
