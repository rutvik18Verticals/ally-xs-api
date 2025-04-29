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
        /// 
        /// </summary>
        public string ChartType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateSelector DateSelector { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ViewOptions ViewOptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ParameterConfig> ParameterConfig { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DateSelector
    {
        /// <summary>
        /// 
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomDateRange Custom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentCompletion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pop { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomDateRange
    {
        /// <summary>
        /// 
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndDate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ViewOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string LineType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShowTooltips { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Std { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShowEvents { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ParameterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool selected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Parameter> Parameters { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Parameter
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LineColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LineType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LineWeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MarkerColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MarkerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MarkerSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pst { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HighParamType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LowParamType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SetPointAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SetPointPST { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomY CustomY { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomY
    {
        /// <summary>
        /// 
        /// </summary>
        public string Enabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Max { get; set; }
    }
}