using MongoDB.Driver;
using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// This is the implementation that represents the configuration of a group and asset
    /// on the current XSPOC database.
    /// </summary>
    public class GroupAndAssetMongoStore : IGroupAndAsset
    {

        #region Private Constants

        private const string COLLECTION_NAME = "Group";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="GroupAndAssetMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public GroupAndAssetMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IGroupAndAsset Implementation

        /// <summary>
        /// Gets all groups, assets, and their relationships.
        /// </summary>
        /// <returns>A <see cref="GroupAssetAndRelationshipData"/> object containing groups, assets, and
        /// their relationships.</returns>
        public GroupAndAssetModel GetGroupAssetAndRelationshipData(string correlationId, string groupName = "")
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupAndAssetMongoStore)}" +
                $" {nameof(GetGroupAssetAndRelationshipData)}", correlationId);

            var mongoCollection = _database.GetCollection<GroupAndAssetModel>(COLLECTION_NAME);
            var allGroups = mongoCollection.FindSync(_ => true).ToList();

            if (groupName == string.Empty)
            {
                return allGroups.FirstOrDefault() ?? new GroupAndAssetModel();
            }

            GroupAndAssetModel result = null;

            foreach (var group in allGroups)
            {
                result = FindGroupInNestedChildGroups(group, groupName);

                if (result != null)
                {
                    break;
                }
            }

            return result ?? new GroupAndAssetModel();
        }

        #endregion

        private GroupAndAssetModel FindGroupInNestedChildGroups(GroupAndAssetModel group, string groupName, int depth = 0)
        {
            if (depth > 10)
            {
                return null;
            }

            if (group.GroupName == groupName)
            {
                return group;
            }

            if (group.ChildGroups == null)
            {
                return null;
            }

            foreach (var childGroup in group.ChildGroups)
            {
                var result = FindGroupInNestedChildGroups(childGroup, groupName, depth + 1);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

    }
}