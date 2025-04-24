namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The asset group status alarm data model.
    /// </summary>
    public class AssetGroupStatusAlarmsModel
    {

        /// <summary>
        /// Gets or sets the id. 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the alarm name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the count of active alarms.
        /// </summary>
        public double Count { get; set; }

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        public double Percent { get; set; }

    }
}
