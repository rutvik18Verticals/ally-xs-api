
namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// This is a record that represents the default parameter data.
    /// </summary>
    public class DefaultParameterDataModel
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

        /// <summary>
        /// Gets or sets the flag for selected or not
        /// </summary>
        public bool Selected { get; set; }
    }
}
