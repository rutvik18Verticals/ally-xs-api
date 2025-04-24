using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a setpoint data model.
    /// </summary>
    public class SetpointModel
    {

        /// <summary>
        /// Gets or sets the parameter/register address.
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the backupdate.
        /// </summary>
        public DateTime? BackupDate { get; set; }

        /// <summary>
        /// Gets or sets the backup value.
        /// </summary>
        public string BackupValue { get; set; }

        /// <summary>
        /// Gets or sets the flag whether the firmware version is supported.
        /// </summary>
        public bool IsSupported { get; set; }

        /// <summary>
        /// Gets or sets the back up look up values.
        /// </summary>
        public List<LookupValueModel> BackUpLookUpValues { get; set; }

        /// <summary>
        /// Gets or sets the stateid of the parameter.
        /// </summary>
        public int? StateId { get; set; }

    }

    /// <summary>
    /// Represents a lookup data model.
    /// </summary>
    public class LookupValueModel
    {

        /// <summary>
        /// Gets or sets the lookup text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the lookup value.
        /// </summary>
        public int Value { get; set; }

    }
}