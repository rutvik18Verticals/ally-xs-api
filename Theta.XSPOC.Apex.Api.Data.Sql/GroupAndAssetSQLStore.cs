using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the configuration of a group and asset
    /// on the current XSPOC database.
    /// </summary>
    public class GroupAndAssetSQLStore : SQLStoreBase, IGroupAndAsset
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="GroupAndAssetSQLStore"/> using the provided 
        /// <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public GroupAndAssetSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        #region IGroupAndAsset Implementation

        /// <summary>
        /// Gets all groups, assets, and their relationships.
        /// </summary>
        /// <param name="groupName">The group name default parameter</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="GroupAssetAndRelationshipData"/> object containing groups, assets, and
        /// their relationships.</returns>
        public GroupAndAssetModel GetGroupAssetAndRelationshipData(string correlationId, string groupName = "")
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(GroupAndAssetSQLStore)} {nameof(GetGroupAssetAndRelationshipData)}",
                correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var groupsAndAssets = context.NodeTree.AsNoTracking()
                    .Where(nt => nt.Type == 0 || nt.Type == 1 || nt.Type == 2)
                    .Select(nt => new NodeTreeModel
                    {
                        Id = nt.Id,
                        Node = nt.Node,
                        Parent = nt.Parent,
                        NumDescendants = nt.NumDescendants,
                        Type = nt.Type
                    })
                    .ToList();

                var nodeMasterData = context.NodeMasters.AsNoTracking()
                    .ToDictionary(nm => nm.NodeId, nm => new NodeMasterModel
                    {
                        NodeId = nm.NodeId,
                        AssetGuid = nm.AssetGuid,
                        ApplicationId = nm.ApplicationId
                    });

                var assetMemberships = context.GroupMembershipCache.AsNoTracking()
                    .GroupBy(gmc => gmc.NodeId)
                    .ToDictionary(group => group.Key, group => group.Select(gmc => gmc.GroupName).ToList());

                var input = new GroupAssetAndRelationshipData
                {
                    Groups = groupsAndAssets.Where(x => x.Type == 0 || x.Type == 1).ToList(),
                    Assets = groupsAndAssets.Where(x => x.Type == 2).ToList(),
                    NodeMasterData = nodeMasterData,
                    AssetMemberships = assetMemberships
                };

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupAndAssetSQLStore)}" +
               $" {nameof(GetGroupAssetAndRelationshipData)}", correlationId);

                return BuildGroupsAndAssets(input);
            }
        }

        #endregion

        #region Private Methods

        private GroupAndAssetModel BuildGroupsAndAssets(GroupAssetAndRelationshipData input)
        {
            var groupsDict = new Dictionary<string, GroupAndAssetModel>();
            var topLevelGroups = new List<GroupAndAssetModel>();

            var result = new GroupAndAssetModel()
            {
                GroupName = "Well Groups",
            };

            foreach (var defaultGroup in input.Groups)
            {
                var groupModel = new GroupAndAssetModel { GroupName = defaultGroup.Node };
                groupsDict[defaultGroup.Node] = groupModel;

                if (defaultGroup.Parent == null || defaultGroup.Parent == "root")
                {
                    topLevelGroups.Add(groupModel);
                }
            }

            foreach (var group in input.Groups.Where(x => x.Parent != null && x.Parent != "root"))
            {
                if (groupsDict.TryGetValue(group.Parent, out var parentGroup))
                {
                    parentGroup.ChildGroups ??= new List<GroupAndAssetModel>();
                    parentGroup.ChildGroups.Add(groupsDict[group.Node]);
                }
            }

            foreach (var asset in input.Assets)
            {
                if (!input.AssetMemberships.TryGetValue(asset.Node, out var parentGroupNames))
                {
                    continue;
                }

                foreach (var parentGroupName in parentGroupNames)
                {
                    if (!groupsDict.TryGetValue(parentGroupName, out var group))
                    {
                        continue;
                    }

                    if (!input.NodeMasterData.TryGetValue(asset.Node, out var nodeMaster))
                    {
                        continue;
                    }

                    var assetData = new AssetModel
                    {
                        AssetId = nodeMaster.AssetGuid,
                        AssetName = nodeMaster.NodeId,
                        IndustryApplicationId = nodeMaster.ApplicationId,
                    };

                    group.Assets ??= new List<AssetModel>();

                    if (!group.Assets.Any(a => a.AssetId == assetData.AssetId))
                    {
                        group.Assets.Add(assetData);
                    }

                    group.Assets = group.Assets.OrderBy(m => m.AssetName).ToList();
                }
            }

            result.ChildGroups = topLevelGroups.OrderBy(g => g.GroupName).ToList();

            return result;
        }

        #endregion

    }
}
