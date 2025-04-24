using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the data points for <seealso cref="ESPAnalysisTrendData"/>.
    /// </summary>
    /// <typeparam name="TX">The type of the x coordinate value.</typeparam>
    /// <typeparam name="TY">The type of the y coordinate value.</typeparam>
    public class DataPoint<TX, TY>
    {

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public TX X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        public TY Y { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the trend name.
        /// </summary>
        public string TrendName { get; set; }

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        public DataPoint()
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public DataPoint(TX x, TY y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="note">The note.</param>
        public DataPoint(TX x, TY y, string note)
            : this(x, y)
        {
            this.Note = note;
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="note">The note.</param>
        /// <param name="trendName">The trend name.</param>
        public DataPoint(TX x, TY y, string note, string trendName)
            : this(x, y, note)
        {
            this.TrendName = trendName;
        }

        #endregion

        /// <summary>
        /// Function to check the x and y coordinates of the data point are same.
        /// </summary>
        /// <param name="obj">The <seealso cref="DataPoint"/>.</param>
        /// <returns>Bool value whether x and y coordinates of the data point are same.</returns>
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is DataPoint<TX, TY> dataPoint)
            {
                flag = Equals(X, dataPoint.X) &&
                    Equals(Y, dataPoint.Y);
            }

            return flag;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return MathUtility.GenerateHashCode(X, Y);
        }
    }

    /// <summary>
    /// Represent the data points for <seealso cref="ESPAnalysisTrendData"/>.
    /// </summary>
    public class DataPoint : DataPoint<DateTime, Decimal>
    {

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        public DataPoint()
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public DataPoint(DateTime x, Decimal y)
            : base(x, y)
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="note">The note.</param>
        public DataPoint(DateTime x, Decimal y, string note)
            : base(x, y, note)
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="note">The note.</param>
        /// <param name="trendName">The trend name.</param>
        public DataPoint(DateTime x, Decimal y, string note, string trendName)
            : base(x, y, note, trendName)
        {
        }
    }

    /// <summary>
    /// Represent the data points for <seealso cref="ESPAnalysisTrendData"/>.
    /// </summary>
    public class AllyTimeSeriesTrendDataPoint
    {

        /// <summary>
        /// Gets or sets the assetID
        /// </summary>
        public Guid AssetID { get; set; }

        /// <summary>
        /// Gets or sets the tag name
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets or sets the tag ID
        /// </summary>
        public string TagId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Gets or sets the time recorded value.
        /// </summary>
        public DateTime TimeRecorded { get; set; }

        /// <summary>
        /// Gets or sets the time written value.
        /// </summary>
        public DateTime TimeWritten { get; set; }

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        public AllyTimeSeriesTrendDataPoint()
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="assetID">The x coordinate.</param>
        /// <param name="tagName">The y coordinate.</param>
        /// <param name="tagId">The tag id.</param>
        /// <param name="value">The value.</param>
        /// <param name="stringValue">The string balue.</param>
        /// <param name="timeRecorded">The time recorded.</param>
        /// <param name="timeWritten">The time written.</param>

        public AllyTimeSeriesTrendDataPoint(Guid assetID, string tagName, string tagId, decimal value, string stringValue, DateTime timeRecorded, DateTime timeWritten)
        {
            this.AssetID = assetID;
            this.TagName = tagName;
            this.TagId = tagId;
            this.Value = value;
            this.StringValue = stringValue;
            this.TimeRecorded = timeRecorded;
            this.TimeWritten = timeWritten;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class TimeSeriesDataPoint
    {

        /// <summary>
        /// Gets or sets the assetID
        /// </summary>
        public string AssetID { get; set; }

        /// <summary>
        /// Gets or sets the tag ID
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the time recorded value.
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the total Page.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or set the TagId
        /// </summary>
        public List<int> TagIds { get; set; }

        /// <summary>
        /// Gets or set the Values.
        /// </summary>
        public List<double?> Values { get; set; }

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        public TimeSeriesDataPoint()
        {
        }
       
        /// <summary>
        /// Constructs a new <seealso cref="DataPoint"/>.
        /// </summary>
        /// <param name="assetID">The x coordinate.</param>       
        /// <param name="tagId">The tag id.</param>
        /// <param name="value">The value.</param>
        /// <param name="timestamp">The string value.</param>        
        public TimeSeriesDataPoint(string assetID, int tagId, double value, string timestamp)
        {
            this.AssetID = assetID;
            this.TagId = tagId;
            this.Value = value;
            this.Timestamp = timestamp;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParamStandardTypeDetails
    {
        /// <summary>
        /// Gets or sets the ParamStandardType
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="ParamStandardTypeDetails"/>.
        /// </summary>
        public ParamStandardTypeDetails()
        {
        }

        /// <summary>
        /// Constructs a new <seealso cref="ParamStandardTypeDetails"/>.
        /// </summary>
        /// <param name="id">The parameter standard type id.</param>       
        /// <param name="description">The asset name.</param>        
        public ParamStandardTypeDetails(int? id, string description)
        {
            this.Id = id;
            this.Description = description;
        }
    }
    #endregion
    #endregion
  
}
