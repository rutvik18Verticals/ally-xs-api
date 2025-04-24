namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the GL flow control device states MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class GLFlowControlDeviceStates : LookupBase
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int GLFlowControlDeviceStatesId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Phrase Id.
        /// </summary>
        public int PhraseId { get; set; }

    }
}
