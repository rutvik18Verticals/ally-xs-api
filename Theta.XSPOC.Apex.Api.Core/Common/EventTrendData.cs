using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Event Trend Data.
    /// </summary>
    public class EventTrendData : TrendData
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        #region Contructors

        /// <summary>
        /// Initializes a new EventTrendData with a specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public EventTrendData(string key)
          : base(key)
        {
        }

        /// <summary>
        /// Initializes a new EventTrendData with name and event type id.
        /// </summary>
        /// <param name="name">The event type name.</param>
        /// <param name="eventTypeId">The event type id.</param>
        public EventTrendData(string name, int eventTypeId)
          : base(string.Empty)
        {
            this.Name = name;
            this.Description = this.Name;
            this.Id = eventTypeId;
            this.Key = this.Id.ToString();
        }

        #endregion

        /// <summary>
        /// Overridden method to get the Data Points in the given date range.
        /// </summary>
        /// <param name="startDate">The start Date.</param>
        /// <param name="endDate">The end Date.</param>
        /// <returns>The <seealso cref="IList{DataPoint}"/>.</returns>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();

            return data;
        }

    }
}
