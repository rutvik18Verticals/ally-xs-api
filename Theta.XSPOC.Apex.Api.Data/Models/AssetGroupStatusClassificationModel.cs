namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The asset group status classification data model.
    /// </summary>
    public class AssetGroupStatusClassificationModel
    {

        /// <summary>
        /// Gets or sets the id. 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the classification name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hours.
        /// </summary>
        public double Hours { get; set; }

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        public double Percent { get; set; }

        /// <summary>
        /// Gets or sets the count of assets/node with classification type.
        /// </summary>
        public double Count { get; set; }

        /// <summary>
        /// Gets or sets the Priority. 
        /// </summary>
        public int Priority { get; set; }

    }
}
