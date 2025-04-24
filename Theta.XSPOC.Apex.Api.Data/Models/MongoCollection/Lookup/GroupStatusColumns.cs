namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the group status columns MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class GroupStatusColumns : LookupBase
    {

        /// <summary>
        /// Gets or sets the column id.
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the Alias (nullable).
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the source id.
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// Gets or sets the Align.
        /// </summary>
        public int? Align { get; set; }

        /// <summary>
        /// Gets or sets the Visible.
        /// </summary>
        public int? Visible { get; set; }

        /// <summary>
        /// Gets or sets the Measure.
        /// </summary>
        public string Measure { get; set; }

        /// <summary>
        /// Gets or sets the Formula.
        /// </summary>
        public string Formula { get; set; }

    }
}
