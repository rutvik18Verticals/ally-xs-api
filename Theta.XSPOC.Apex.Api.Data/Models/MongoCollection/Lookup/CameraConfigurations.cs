namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the CameraConfigurations MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class CameraConfigurations : LookupBase
    {
        /// <summary>
        /// Gets or sets the description of the Camera Configuration.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the format string for the video.
        /// </summary>
        public string FormatStringVideo { get; set; }

        /// <summary>
        /// Gets or sets the format string for the snapshot.
        /// </summary>
        public string FormatStringSnapshot { get; set; }

        /// <summary>
        /// Gets or sets the HTTP authentication option for the snapshot URI.
        /// </summary>
        public int SnapshotUriHttpAuthentication { get; set; }
    }
}
