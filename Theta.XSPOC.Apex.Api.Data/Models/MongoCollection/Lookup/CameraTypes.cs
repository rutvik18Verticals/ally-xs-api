namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the CameraTypes MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class CameraTypes : LookupBase
    {
        /// <summary>
        /// Gets or sets the Name of the Camera Configuration.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the PhraseId of the Camera Configuration.
        /// </summary>
        public int? PhraseId { get; set; }
    }
}
