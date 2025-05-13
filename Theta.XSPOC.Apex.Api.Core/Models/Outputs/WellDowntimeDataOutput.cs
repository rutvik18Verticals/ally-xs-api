using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the output model for well downtime data.
    /// </summary>
    public class WellDowntimeDataOutput : CoreOutputBase
    {
        /// <summary>
        /// The asset guid.
        /// </summary>
        public string AssetGuid { get; set; }

        /// <summary>
        /// The well name.
        /// </summary>
        public string Well { get; set; }

        /// <summary>
        /// The business unit name.
        /// </summary>
        public string WellFieldAreaBusinessUnitName { get; set; }

        /// <summary>
        /// The method of Production/application type.
        /// </summary>
        public string WellMethodOfProduction { get; set; }

        /// <summary>
        /// The field area name.
        /// </summary>
        public string WellFieldAreaName { get; set; }

        /// <summary>
        /// The field name.
        /// </summary>
        public string WellFieldName { get; set; }

        /// <summary>
        /// The well group.
        /// </summary>
        public string WellGroup { get; set; }

        /// <summary>
        /// The well identifier.
        /// </summary>
        public string WellIdentifier { get; set; }

        /// <summary>
        /// The well timezone.
        /// </summary>
        public string WellTimezone { get; set; }

        /// <summary>
        /// Gets or sets the downtime data for the well.
        /// </summary>
        public IList<WellDowntimeData> DowntimeData { get; set; }
        /// <summary>
        /// Gets or sets the downtime filters applied to the data.
        /// </summary>
        public IList<WellDowntimeData> Last5Shutdowns { get; set; }

    }

    /// <summary>
    /// Represents the downtime data for a well.
    /// </summary>
    public class WellDowntimeData
    {

        /// <summary>
        /// The start date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The start date in milliseconds.
        /// </summary>
        public double DateMilliseconds { get; set; }

        /// <summary>
        /// The start date.
        /// </summary>
        public DateTime DateLocal { get; set; }

        /// <summary>
        /// The start date local string.
        /// </summary>
        public string DateLocalString { get; set; }

        /// <summary>
        /// The start date local string in milliseconds.
        /// </summary>
        public double DateLocalMilliseconds { get; set; }

        /// <summary>
        /// The end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The end date.
        /// </summary>
        public DateTime EndDateLocal { get; set; }

        /// <summary>
        /// The end date in milliseconds.
        /// </summary>
        public double EndDateMilliseconds { get; set; }

        /// <summary>
        /// The end date local string.
        /// </summary>
        public string EndDateLocalString { get; set; }

        /// <summary>
        /// The end date local string in milliseconds.
        /// </summary>
        public double EndDateLocalMilliseconds { get; set; }

        /// <summary>
        /// Flag specifying if the downtime is active.
        /// </summary>
        public bool IsActiveDowntime { get; set; }

        /// <summary>
        /// The offline hours.
        /// </summary>
        public int OfflineHours { get; set; }

        /// <summary>
        /// The hours.
        /// </summary>
        public double Hours { get; set; }

        /// <summary>
        /// The current hours.
        /// </summary>
        public int CurrentHours { get; set; }

    }
}
