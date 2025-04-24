namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represents the contract that map from rodlift analysis data model to response rodlift analysis data object.
    /// </summary>
    public class ValueItem
    {

        /// <summary>
        /// The parameter id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The object value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The display value.
        /// </summary>
        public string DisplayValue { get; set; }

        /// <summary>
        /// The date type id.
        /// </summary>
        public int DataTypeId { get; set; }

        /// <summary>
        /// The measurement abbreviation.
        /// </summary>
        public string MeasurementAbbreviation { get; set; }

        /// <summary>
        /// The source id.
        /// </summary>
        public int? SourceId { get; set; }

    }
}
