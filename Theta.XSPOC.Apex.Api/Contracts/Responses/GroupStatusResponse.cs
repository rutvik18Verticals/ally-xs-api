namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{

    /// <summary>
    /// Represents a response containing group status data.
    /// </summary>
    public class GroupStatusResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the Values.
        /// </summary>
        public GroupStatusResponseValues Values { get; set; } = new GroupStatusResponseValues();

    }
}
