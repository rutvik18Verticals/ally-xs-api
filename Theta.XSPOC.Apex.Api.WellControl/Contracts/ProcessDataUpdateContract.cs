namespace Theta.XSPOC.Apex.Api.WellControl.Contracts
{
    /// <summary>
    /// Represents the data necessary to update a data store.
    /// </summary>
    public class ProcessDataUpdateContract
    {

        /// <summary>
        /// Gets or sets the action to take with this payload. For example: Update, Insert, Delete
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the payload type. In this case, the table to update. For example: 
        /// tblNodeMaster, tblDataHistory
        /// </summary>
        public string PayloadType { get; set; }

        /// <summary>
        /// Gets or sets the payload. For this contract, a json representation of the updated data.
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Gets or sets the response meta data in case of web-api calls.
        /// </summary>
        public string ResponseMetadata { get; set; }

    }
}
