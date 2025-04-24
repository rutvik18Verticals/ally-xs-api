namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the esp well pump data model.
    /// </summary>
    public class ESPWellPumpModel
    {

        /// <summary>
        /// Get or sets the esp well id.
        /// </summary>
        public string ESPWellId { get; set; }

        /// <summary>
        /// Get or sets the esp pump id.
        /// </summary>
        public int ESPPumpId { get; set; }

        /// <summary>
        /// Get or sets the number of stages.
        /// </summary>
        public int? NumberOfStages { get; set; }

        /// <summary>
        /// Get or sets the order number.
        /// </summary>
        public int OrderNumber { get; set; }

    }
}
