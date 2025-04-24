using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the RowColumnModel.
    /// </summary>
    public class RowColumnModel
    {

        /// <summary>
        /// Get and set the Column id.
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Get and set the Decimals.
        /// </summary>
        public int? Decimals { get; set; }

        /// <summary>
        /// Get and set the BackColor.
        /// </summary>
        public string BackColor { get; set; }

        /// <summary>
        /// Get and set the ForeColor.
        /// </summary>
        public string ForeColor { get; set; }

        /// <summary>
        /// Get and set the Align.
        /// </summary>
        public TextHAlign Align { get; set; }

        /// <summary>
        /// Get and set the Value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get or set the type of the value.
        /// </summary>
        public Type ValueType { get; set; }

    }
}
