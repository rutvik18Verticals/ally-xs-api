namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The Measurement Trend Item Model.
    /// </summary>
    public class MeasurementTrendItemModel
    {

        /// <summary>
        /// Gets or sets the Param Standard Type.
        /// </summary>
        public int? ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Unit Type ID.
        /// </summary>
        public int UnitTypeID { get; set; }

        /// <summary>
        /// Gets or sets the Address.
        /// </summary>
        public int? Address { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Phrase ID.
        /// </summary>
        public int? PhraseID { get; set; }

        /// <summary>
        /// Gets or sets the Parameter Type.
        /// </summary>
        public string ParameterType { get; set; }

    }
}
