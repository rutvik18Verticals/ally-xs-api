namespace Theta.XSPOC.Apex.Api.WellControl.Contracts.V2
{
    /// <summary>
    /// The data contract that represents a data update event in the system. Used to send data updates in the XSPOC
    /// database out to services and vice versa. The main purpose of this contract is to maintain backward compatibility 
    /// with the XSPOC system.
    /// While these data updates may result in side effects in a service, the primary intent is to send updated data to
    /// modules like the edge module, and in some cases flow data meant for informational purposes from it back to
    /// the XSPOC database ( for example, port schedule updates from a Comms module back to the XSPOC database ).
    /// </summary>
    public class DataUpdateEvent
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
