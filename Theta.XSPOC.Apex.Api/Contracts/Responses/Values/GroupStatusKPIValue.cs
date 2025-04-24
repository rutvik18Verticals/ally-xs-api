namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represents the response values for group status.
    /// </summary>
    public class GroupStatusKPIValue
    {

        /// <summary>
        /// Gets or sets the id. 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        public double Count { get; set; }

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        public double Percent { get; set; }
    }
}
