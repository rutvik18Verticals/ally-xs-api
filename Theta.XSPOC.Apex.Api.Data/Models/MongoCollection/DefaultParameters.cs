namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// This is a record that represents the param standard data.
    /// </summary>
    public class DefaultParameters : DocumentBase
    {

        /// <summary>
        /// Gets or sets the Lift Type.
        /// </summary>
        public string LiftType { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Pst.
        /// </summary>
        public string Pst { get; set; }

        /// <summary>
        /// Gets or sets the Units.
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// Gets or sets the Display Order.
        /// </summary>
        public string DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the High Param Type.
        /// </summary>
        public string HighParamType { get; set; }

        /// <summary>
        /// Gets or sets the Low Param Type.
        /// </summary>
        public string LowParamType { get; set; }

    }
}