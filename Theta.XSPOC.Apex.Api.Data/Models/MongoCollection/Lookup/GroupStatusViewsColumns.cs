namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the group status view comuns to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class GroupStatusViewsColumns : LookupBase
    {

        /// <summary>
        /// Gets or sets the View Id.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the Column Id.
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Gets or sets the Width (nullable).
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the Position (nullable).
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// Gets or sets the Orientation.
        /// </summary>
        public int Orientation { get; set; }

        /// <summary>
        /// Gets or sets the Format Id.
        /// </summary>
        public int FormatId { get; set; }

    }
}
