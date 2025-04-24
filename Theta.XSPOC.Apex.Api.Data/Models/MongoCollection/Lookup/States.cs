namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the states MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class States : LookupBase
    {

        /// <summary>
        /// Gets or sets the State Id.
        /// </summary>
        public int StatesId { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the Text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Back color.
        /// </summary>
        public int? BackColor { get; set; }

        /// <summary>
        /// Gets or sets the Alarm priority.
        /// </summary>
        public int? AlarmPriority { get; set; }

        /// <summary>
        /// Gets or sets if the alarm configuration is locked.
        /// </summary>
        public bool? IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the Fore color.
        /// </summary>
        public int? ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the Phrase Id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}
