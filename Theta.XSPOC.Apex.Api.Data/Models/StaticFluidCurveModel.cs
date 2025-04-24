using System.Collections.Generic;
namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the StaticFluidCurveModel.
    /// </summary>
    public class StaticFluidCurveModel
    {

        /// <summary>
        /// Get and sets the Perforations.
        /// </summary>
        public IList<PerforationModel> Perforations { get; set; }

        /// <summary>
        /// Get and sets the Perforations.
        /// </summary>
        public float? ProductionDepth { get; set; }

        /// <summary>
        /// Get and sets the ReservoirPressure.
        /// </summary>
        public float? ReservoirPressure { get; set; }

        /// <summary>
        /// Get and sets the KillFluidLevel.
        /// </summary>
        public float? KillFluidLevel { get; set; }

        /// <summary>
        /// Get and sets the ReservoirFluidLevel.
        /// </summary>
        public float? ReservoirFluidLevel { get; set; }

    }
}