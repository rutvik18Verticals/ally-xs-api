namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the file types MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class FileTypes : LookupBase
    {

        /// <summary>
        /// Gets or sets the File type Id.
        /// </summary>
        public int FileTypeId { get; set; }

        /// <summary>
        /// Gets or sets the File type.
        /// </summary>
        public string FileType { get; set; }

    }
}
