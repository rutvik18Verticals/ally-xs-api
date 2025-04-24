using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the group and asset data model.
    /// </summary>
    public class GroupAndAssetModel : CustomerDocumentBase
    {

        /// <summary>
        /// Gets or sets the asset group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the list of children groups.
        /// </summary>
        public IList<GroupAndAssetModel> ChildGroups { get; set; }

        /// <summary>
        /// Gets or sets the list of assets.
        /// </summary>
        public IList<AssetModel> Assets { get; set; }

        /// <summary>
        /// Gets or sets the SQL text.
        /// </summary>
        public string SQLText { get; set; }

        /// <summary>
        /// Gets or sets shutdown flag.
        /// </summary>
        public bool ShutdownFlag { get; set; }

        /// <summary>
        /// Gets or sets shutdown start date.
        /// </summary>
        public DateTime? ShutdownStartDate { get; set; }

        /// <summary>
        /// Gets or sets shutdown end date.
        /// </summary>
        public DateTime? ShutdownEndDate { get; set; }

        /// <summary>
        /// Gets or sets the startup flag. 
        /// </summary>
        public bool StartupFlag { get; set; }

        /// <summary>
        /// Gets or sets startup start date.
        /// </summary>
        public DateTime? StartupStartDate { get; set; }

        /// <summary>
        /// Gets or sets startup end date.
        /// </summary>
        public DateTime? StartupEndDate { get; set; }

        /// <summary>
        /// Gets or sets the drill down flag.
        /// </summary>
        public bool DrillDown { get; set; }

        /// <summary>
        /// Gets or sets the fac group flag.
        /// </summary>
        public bool FacGroup { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the filter id.
        /// </summary>
        public int? FilterId { get; set; }

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        public int? ViewId { get; set; }

        /// <summary>
        /// Gets or sets the locked flag.
        /// </summary>
        public bool? Locked { get; set; }

        /// <summary>
        /// Gets or sets the suspend ports.
        /// </summary>
        public int? SuspendPorts { get; set; }

        /// <summary>
        /// Gets or sets the enable equipment download.
        /// </summary>
        public int? EnableEquipmentDownload { get; set; }

        /// <summary>
        /// Gets or sets the allow start flag.
        /// </summary>
        public bool AllowStartFlag { get; set; }

        /// <summary>
        /// Gets or sets the allow start lock.
        /// </summary>
        public bool? AllowStartLock { get; set; }

        /// <summary>
        /// Gets or sets the score value.
        /// </summary>
        public float? ScoreValue { get; set; }

        /// <summary>
        /// Gets or sets the last refreshed date.
        /// </summary>
        public DateTime? LastRefreshed { get; set; }

        /// <summary>
        /// Gets or sets the refresh timeout value.
        /// </summary>
        public int? RefreshTimeout { get; set; }

        /// <summary>
        /// Gets or sets the group notification value.
        /// </summary>
        public int? GroupNotification { get; set; }

        /// <summary>
        /// Gets or sets the filter group flag.
        /// </summary>
        public bool? FilterGroup { get; set; }

        /// <summary>
        /// Gets or sets the hidden flag.
        /// </summary>
        public bool Hidden { get; set; }

    }
}
