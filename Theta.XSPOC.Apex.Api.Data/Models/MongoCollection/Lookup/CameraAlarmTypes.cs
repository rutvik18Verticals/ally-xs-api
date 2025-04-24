namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the CameraAlarmTypes MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class CameraAlarmTypes : LookupBase
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the PhraseId.
        /// </summary>
        public int? PhraseId { get; set; }
    }
}
