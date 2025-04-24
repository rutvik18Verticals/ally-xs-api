namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Represents the response values for group status.
    /// </summary>
    public class GroupStatusAlarmsResponseValues
    {

        /// <summary>
        /// Gets or sets the id. 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the count of assets with alarms.
        /// </summary>
        public double Count { get; set; }

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        public double Percent { get; set; }
    }
}
