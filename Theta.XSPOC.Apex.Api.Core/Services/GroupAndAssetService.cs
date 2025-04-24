using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of IGroupAndAssetService.
    /// </summary>
    public class GroupAndAssetService : IGroupAndAssetService
    {

        #region Private Dependencies

        private readonly IGroupAndAsset _groupAndAssetService;
        private readonly INodeMaster _nodeMasterService;
        private readonly IUserDefaultStore _userDefaultStore;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="GroupAndAssetService"/>.
        /// </summary>
        /// <param name="groupAndAssetService">The <seealso cref="IGroupAndAsset"/> service.</param>
        /// <param name="userDefaultStore">The <seealso cref="IUserDefaultStore"/> service.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="nodeMasterService">The <seealso cref="INodeMaster"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="groupAndAssetService"/> is null
        /// or
        /// <paramref name="userDefaultStore"/> is null
        /// or
        /// <paramref name="loggerFactory"/> is null.
        /// </exception>
        public GroupAndAssetService(IGroupAndAsset groupAndAssetService, IUserDefaultStore userDefaultStore,
            IThetaLoggerFactory loggerFactory, INodeMaster nodeMasterService)
        {
            _groupAndAssetService = groupAndAssetService ?? throw new ArgumentNullException(nameof(groupAndAssetService));
            _userDefaultStore = userDefaultStore ?? throw new ArgumentNullException(nameof(userDefaultStore));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _nodeMasterService = nodeMasterService ?? throw new ArgumentNullException(nameof(nodeMasterService));
        }

        #endregion

        #region IGroupAndAsset Implementation

        /// <summary>
        /// Retrieves the hierarchical group and asset data and maps it to the output model.
        /// </summary>
        /// <param name="userName">The user name to get the group and asset data.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="isNewArchitecture">Boolean specifying fetch new architecture wells.</param>
        /// <returns>A <see cref="GroupAndAssetDataOutput"/> object containing the result status and hierarchical 
        /// group and asset data.</returns>
        public GroupAndAssetDataOutput GetGroupAndAssetData(string userName, string correlationId, bool isNewArchitecture)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupAndAsset);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupAndAssetService)} {nameof(GetGroupAndAssetData)}", correlationId);

            GroupAndAssetDataOutput groupAndAssetData = new GroupAndAssetDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            try
            {
                var groups = _groupAndAssetService.GetGroupAssetAndRelationshipData(correlationId, string.Empty);
                var newArchitectureAssets = new List<Guid>();
                if (isNewArchitecture)
                {
                    newArchitectureAssets = _nodeMasterService.GetNewArchitectureWells(correlationId);
                }

                if (groups.ChildGroups?.Count == 0)
                {
                    var message = "There is no record.";
                    logger.Write(Level.Error, message);
                    groupAndAssetData.Result = new MethodResult<string>(status: false, value: message);

                    return groupAndAssetData;
                }

                var userDefaults = _userDefaultStore.GetItem(userName, "SingleBranch", "WellExplorer", correlationId);
                groupAndAssetData.Values = BuildGroups(groups, userDefaults, isNewArchitecture, newArchitectureAssets);
            }
            catch (Exception ex)
            {
                var message = $"An error occurred while getting group and asset data: {ex.Message}";
                logger.Write(Level.Error, message);
                groupAndAssetData.Result = new MethodResult<string>(status: false, value: message);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupAndAssetService)} {nameof(GetGroupAndAssetData)}", correlationId);

            return groupAndAssetData;
        }

        #endregion

        #region Private Methods

        private GroupAndAssetModel BuildGroups(GroupAndAssetModel wellGroupEntities, string userDefaults,
            bool isNewArchitecture, List<Guid> newArchitectureAssets)
        {
            var refreshDateTime = DateTime.UtcNow;
            var childrenOfRoot = wellGroupEntities.ChildGroups;

            if (userDefaults != null && userDefaults != "root")
            {
                foreach (var child in wellGroupEntities.ChildGroups)
                {
                    if (child.GroupName == userDefaults)
                    {
                        childrenOfRoot = new List<GroupAndAssetModel>()
                        {
                            child,
                        };

                        break;
                    }
                    else
                    {
                        var foundInChildren = SearchInChildren(child, userDefaults);

                        if (foundInChildren != null)
                        {
                            childrenOfRoot = foundInChildren;

                            break;
                        }
                    }
                }
            }

            var result = new List<GroupAndAssetModel>();
            foreach (var child in childrenOfRoot ?? new List<GroupAndAssetModel>())
            {
                if (isNewArchitecture && newArchitectureAssets.Any())
                {
                    child.Assets = child.Assets.Where(a => newArchitectureAssets.Contains(a.AssetId))
                        .ToList();
                }

                var item = new GroupAndAssetModel
                {
                    GroupName = child.GroupName,
                    SQLText = child.SQLText,
                    AllowStartFlag = child.AllowStartFlag,
                    AllowStartLock = child.AllowStartLock,
                    DrillDown = child.DrillDown,
                    EnableEquipmentDownload = child.EnableEquipmentDownload,
                    FacGroup = child.FacGroup,
                    FilterGroup = child.FilterGroup,
                    FilterId = child.FilterId,
                    GroupNotification = child.GroupNotification,
                    Hidden = child.Hidden,
                    Locked = child.Locked,
                    LastRefreshed = refreshDateTime,
                    Priority = child.Priority,
                    RefreshTimeout = child.RefreshTimeout,
                    ScoreValue = child.ScoreValue,
                    ShutdownEndDate = child.ShutdownEndDate,
                    ShutdownFlag = child.ShutdownFlag,
                    ShutdownStartDate = child.ShutdownStartDate,
                    StartupEndDate = child.StartupEndDate,
                    StartupFlag = child.StartupFlag,
                    StartupStartDate = child.StartupStartDate,
                    SuspendPorts = child.SuspendPorts,
                    ViewId = child.ViewId,
                    Assets = OrderAssets(child.Assets),
                    ChildGroups = GetChildren(child.ChildGroups, isNewArchitecture, newArchitectureAssets),
                };

                result.Add(item);
            }

            wellGroupEntities.ChildGroups = result;

            return wellGroupEntities;
        }

        private IList<GroupAndAssetModel> SearchInChildren(GroupAndAssetModel group, string userDefaults)
        {
            if (group.ChildGroups == null || group.ChildGroups.Count == 0)
            {
                return null;
            }

            foreach (var child in group.ChildGroups)
            {
                if (child.GroupName == userDefaults)
                {
                    return new List<GroupAndAssetModel>()
                    {
                        child,
                    };
                }
                else
                {
                    var foundInChildren = SearchInChildren(child, userDefaults);

                    if (foundInChildren != null)
                    {
                        return foundInChildren;
                    }
                }
            }

            return null;
        }

        private IList<AssetModel> OrderAssets(IList<AssetModel> assets)
        {
            return assets?.OrderBy(m => m.AssetName).ToList();
        }

        private IList<GroupAndAssetModel> GetChildren(IList<GroupAndAssetModel> children,
            bool isNewArchitecture, List<Guid> newArchitectureAssets)
        {
            if (children == null || children.Count == 0)
            {
                return null;
            }

            return children.Select(m => new GroupAndAssetModel()
            {
                GroupName = m.GroupName,
                SQLText = m.SQLText,
                AllowStartFlag = m.AllowStartFlag,
                AllowStartLock = m.AllowStartLock,
                DrillDown = m.DrillDown,
                EnableEquipmentDownload = m.EnableEquipmentDownload,
                FacGroup = m.FacGroup,
                FilterGroup = m.FilterGroup,
                FilterId = m.FilterId,
                GroupNotification = m.GroupNotification,
                Hidden = m.Hidden,
                Locked = m.Locked,
                LastRefreshed = m.LastRefreshed,
                Priority = m.Priority,
                RefreshTimeout = m.RefreshTimeout,
                ScoreValue = m.ScoreValue,
                ShutdownEndDate = m.ShutdownEndDate,
                ShutdownFlag = m.ShutdownFlag,
                ShutdownStartDate = m.ShutdownStartDate,
                StartupEndDate = m.StartupEndDate,
                StartupFlag = m.StartupFlag,
                StartupStartDate = m.StartupStartDate,
                SuspendPorts = m.SuspendPorts,
                ViewId = m.ViewId,
                Assets = isNewArchitecture ? OrderAssets(m.Assets.Where(a => newArchitectureAssets.Contains(a.AssetId)).ToList()) :
                OrderAssets(m.Assets),
                ChildGroups = GetChildren(m.ChildGroups, isNewArchitecture, newArchitectureAssets),
            }).ToList();
        }

        #endregion

    }
}
