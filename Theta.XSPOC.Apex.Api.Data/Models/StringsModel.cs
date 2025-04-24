namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// THe WellStatistics table.
    /// </summary>
    public class StringsModel
    {

        /// <summary>
        /// Get and set the String Id.
        /// </summary>
        public int StringId { get; set; }

        /// <summary>
        /// Get and set the String Name.
        /// </summary>
        public string StringName { get; set; }

        /// <summary>
        /// Get and set the ContactList Id.
        /// </summary>
        public int? ContactListId { get; set; }

        /// <summary>
        /// Get and set the Responder ListId.
        /// </summary>
        public int? ResponderListId { get; set; }

    }
}
