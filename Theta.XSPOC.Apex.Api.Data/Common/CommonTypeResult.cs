using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.Common
{
    /// <summary>
    /// Represents the CommonTypeResult.
    /// </summary>
    public class CommonTypeResult
    {

        /// <summary>
        /// Get and sets the NodeId.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Get and sets the PocType.
        /// </summary>
        public short PocType { get; set; }

        /// <summary>
        /// Get and sets the Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Get and sets the StateId.
        /// </summary>
        public string StateId { get; set; }

        /// <summary>
        /// Get and sets the AlarmState.
        /// </summary>
        public string AlarmState { get; set; }

        /// <summary>
        /// Get and sets the DataType.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Get and sets the AlarmTextClear.
        /// </summary>
        public string AlarmTextClear { get; set; }

        /// <summary>
        /// Get and sets the AlarmTextHi.
        /// </summary>
        public string AlarmTextHi { get; set; }

        /// <summary>
        /// Get and sets the AlarmTextLo.
        /// </summary>
        public string AlarmTextLo { get; set; }

        /// <summary>
        /// Get and sets the NodeMaster.
        /// </summary>
        public NodeMasterModel NodeMasterModel { get; set; }

        /// <summary>
        /// Get and sets the WellDetail.
        /// </summary>
        public WellDetailsModel WellDetailsModel { get; set; }

    }

}
