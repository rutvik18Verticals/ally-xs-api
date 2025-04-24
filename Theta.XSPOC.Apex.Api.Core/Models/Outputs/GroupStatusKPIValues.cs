namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the group status KPI values.
    /// </summary>
    public class GroupStatusKPIValues
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
