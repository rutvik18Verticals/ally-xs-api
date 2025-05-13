using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// This class defines the Time Series Chart MongoDB sub document.
    /// </summary>
    public class TimeSeriesChart : WidgetType
    {
        /// <summary>
        /// The chart type
        /// </summary>
        public string ChartType { get; set; }

        /// <summary>
        /// The date selector object.
        /// </summary>
        public DateSelector DateSelector { get; set; }

        /// <summary>
        /// The view options object.
        /// </summary>
        public ViewOptions ViewOptions { get; set; }

        /// <summary>
        /// The parameter configurations object.
        /// </summary>
        public List<ParameterConfig> ParameterConfig { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateSelector
    {
        /// <summary>
        /// The date selector interval.
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// The date selector custom date range.
        /// </summary>
        public CustomDateRange Custom { get; set; }

        /// <summary>
        /// The flag for the current completion option.
        /// </summary>
        public string CurrentCompletion { get; set; }

        /// <summary>
        /// The flag for the POP option.
        /// </summary>
        public string Pop { get; set; }
    }

    /// <summary>
    /// The custom date range class.
    /// </summary>
    public class CustomDateRange
    {
        /// <summary>
        /// The start date of the custom date range.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// The end date of the custom date range.
        /// </summary>
        public string EndDate { get; set; }
    }

    /// <summary>
    /// The view options class.
    /// </summary>
    public class ViewOptions
    {
        /// <summary>
        /// The Line type(solid, dotted, dashed, etc).
        /// </summary>
        public string LineType { get; set; }

        /// <summary>
        /// The flag for the show tooltip option.
        /// </summary>
        public string ShowTooltips { get; set; }

        /// <summary>
        /// The flag for the showing std deviation option.
        /// </summary>
        public string Std { get; set; }

        /// <summary>
        /// The flag for the show events option.
        /// </summary>
        public string ShowEvents { get; set; }
    }

    /// <summary>
    /// The parameter configuration class.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ParameterConfig
    {
        /// <summary>
        /// The parameter configuration id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The flag for the selected parameter.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// The parameter configuration name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The flag stating the parameter is default.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// The list of parameters.
        /// </summary>
        public List<Parameter> Parameters { get; set; }
    }

    /// <summary>
    /// The parameter class.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Parameter
    {
        /// <summary>
        /// The parameter id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameter description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The line color.
        /// </summary>
        public string LineColor { get; set; }

        /// <summary>
        /// The line type.
        /// </summary>
        public string LineType { get; set; }

        /// <summary>
        /// The line weight.
        /// </summary>
        public string LineWeight { get; set; }

        /// <summary>
        /// The marker color.
        /// </summary>
        public string MarkerColor { get; set; }

        /// <summary>
        /// The marker type.
        /// </summary>
        public string MarkerType { get; set; }

        /// <summary>
        /// The marker size.
        /// </summary>
        public string MarkerSize { get; set; }

        /// <summary>
        /// The parameter std type.
        /// </summary>
        public string Pst { get; set; }

        /// <summary>
        /// The measurement units of the parameter.
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// The parameter display order.
        /// </summary>
        public string DisplayOrder { get; set; }

        /// <summary>
        /// The flag for the selected parameter.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// The high parameter PST
        /// </summary>
        public string HighParamType { get; set; }

        /// <summary>
        /// The low parameter PST
        /// </summary>
        public string LowParamType { get; set; }

        /// <summary>
        /// The set point address.
        /// </summary>
        public string SetPointAddress { get; set; }

        /// <summary>
        /// The set point PST.
        /// </summary>
        public string SetPointPST { get; set; }

        /// <summary>
        /// The custom Y axis object.
        /// </summary>
        public CustomY CustomY { get; set; }
    }

    /// <summary>
    /// The custom Y axis class.
    /// </summary>
    public class CustomY
    {
        /// <summary>
        /// The flag for the custom Y axis enabled or not.
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// The minimum value of the Y axis.
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// The maximum value of the Y axis.
        /// </summary>
        public string Max { get; set; }
    }
}