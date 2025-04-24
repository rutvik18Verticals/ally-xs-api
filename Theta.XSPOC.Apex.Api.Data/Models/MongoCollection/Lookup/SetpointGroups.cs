namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the setpoint groups MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class SetpointGroups : LookupBase
    {

        /// <summary>
        /// Gets or sets the Setpoint groups Id.
        /// </summary>
        public int SetpointGroupsId { get; set; }

        /// <summary>
        /// Gets or sets the Display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the Display order.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the Phrase Id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}
