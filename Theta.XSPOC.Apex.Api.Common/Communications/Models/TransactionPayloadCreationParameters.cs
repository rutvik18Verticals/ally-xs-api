using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Common.Communications.Models
{
    /// <summary>
    /// Specifies the inputs for the transaction payload creation process.
    /// </summary>
    public class TransactionPayloadCreationParameters
    {

        #region Private Constants

        private const int DEFAULT_PRIORITY = 5;
        private const int DEFAULT_INTERVAL = 0;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// Gets or sets the asset guid.
        /// </summary>
        public Guid? AssetGUID { get; set; }

        /// <summary>
        /// Gets or sets the address values.
        /// </summary>
        public IDictionary<int, float> AddressValues { get; set; }

        /// <summary>
        /// Gets or sets the event group id.
        /// </summary>
        public int? EventGroupId { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; } = DEFAULT_PRIORITY;

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        public int Interval { get; set; } = DEFAULT_INTERVAL;

        /// <summary>
        /// Gets or sets the control type.
        /// </summary>
        public DeviceControlType ControlType { get; set; }

        /// <summary>
        /// Gets or sets the equipment selection.
        /// </summary>
        public int EquipmentSelection { get; set; }

        #endregion

    }
}
