namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the alarm event types MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class LocalePhrases : LookupBase
    {

        /// <summary>
        /// Gets or sets the Phrase Id.
        /// </summary>
        public int PhraseId { get; set; }

        /// <summary>
        /// Gets or sets the Phrase type.
        /// </summary>
        public int PhraseType { get; set; }

        /// <summary>
        /// Gets or sets the English version of the phrase.
        /// </summary>
        public string English { get; set; }

        /// <summary>
        /// Gets or sets the German version of the phrase.
        /// </summary>
        public string German { get; set; }

        /// <summary>
        /// Gets or sets the Spanish version of the phrase.
        /// </summary>
        public string Spanish { get; set; }

        /// <summary>
        /// Gets or sets the Russian version of the phrase.
        /// </summary>
        public string Russian { get; set; }

        /// <summary>
        /// Gets or sets the Chinese version of the phrase.
        /// </summary>
        public string Chinese { get; set; }

        /// <summary>
        /// Gets or sets the French version of the phrase.
        /// </summary>
        public string French { get; set; }

        /// <summary>
        /// Gets or sets if the alarm configuration is locked.
        /// </summary>
        public bool? Locked { get; set; }

    }
}
