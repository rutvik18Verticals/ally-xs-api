namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the group status tables MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class GroupStatusTables : LookupBase
    {

        /// <summary>
        /// Gets or sets the table id.
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string TableName { get; set; }

    }
}
