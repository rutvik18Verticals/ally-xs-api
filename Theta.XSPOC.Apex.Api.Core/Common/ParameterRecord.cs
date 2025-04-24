using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Parameter Records class.
    /// </summary>
    public class ParameterRecord
    {

        /// <summary>
        /// The record date time.
        /// </summary>
        public DateTime? RecordDateTime { get; set; }

        /// <summary>
        /// The value.
        /// </summary>
        public float? Value { get; set; }

    }
}
