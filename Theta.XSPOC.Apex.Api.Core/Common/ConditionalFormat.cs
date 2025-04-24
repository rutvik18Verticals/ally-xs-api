namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the ConditionalFormat.
    /// </summary>
    public class ConditionalFormat
    {

        /// <summary>
        /// Get and sets the conditional format properties.
        /// </summary>
        public ConditionalFormat()
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the column id.
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Gets or sets the operator used for comparison of values.
        /// </summary>
        public ComparisonOperator Operator { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float? Value { get; set; }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public float? MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public float? MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the back color.
        /// </summary>
        public int BackColor { get; set; }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public int ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the value for text comparison.
        /// </summary>
        public string StringValue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new ConditionalFormat
        /// </summary>
        /// <param name="columnId">The conditional format column id</param>
        /// <param name="operatorId">The conditional format operator id</param>
        /// <param name="value">The conditional format value</param>
        /// <param name="minValue">The conditional format min value</param>
        /// <param name="maxValue">The conditional format max value</param>
        /// <param name="backColor">The conditional format color</param>
        /// <param name="foreColor">The conditional format color</param>
        /// <param name="stringValue">The conditional format text value</param>
        public ConditionalFormat(int columnId, int operatorId, float? value,
            float? minValue, float? maxValue, int backColor, int foreColor, string stringValue)
        {
            ColumnId = columnId;
            Operator = EnhancedEnumBase.GetValue<ComparisonOperator>(operatorId);
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
            BackColor = backColor;
            ForeColor = foreColor;
            StringValue = stringValue;
        }

        #endregion

    }
}
