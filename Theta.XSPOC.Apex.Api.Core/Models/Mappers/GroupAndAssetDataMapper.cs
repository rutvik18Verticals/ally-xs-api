using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Mappers
{
    /// <summary>
    /// The mapper for the group and asset data.
    /// </summary>
    public class GroupAndAssetDataMapper
    {

        #region Public Methods

        /// <summary>
        /// Maps the <paramref name="groupAndAssetData"/> core object to a <seealso cref="GroupAndAssetModel"/> 
        /// data object.
        /// </summary>
        /// <param name="groupAndAssetData">The groupAndAssetData core model to map from.</param>
        /// <returns>A <seealso cref="GroupAndAssetModel"/> representing the provided 
        /// <paramref name="groupAndAssetData"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="groupAndAssetData"/> is null.</exception>
        public static GroupAndAssetModel Map(GroupAndAssetData groupAndAssetData)
        {
            if (groupAndAssetData == null)
            {
                throw new ArgumentNullException(nameof(groupAndAssetData));
            }

            return new GroupAndAssetModel()
            {
                GroupName = groupAndAssetData.GroupName,
                Assets = groupAndAssetData.Assets?.Select(asset => new AssetModel
                {
                    AssetId = asset.AssetId,
                    AssetName = asset.AssetName,
                    IndustryApplicationId = asset.IndustryApplicationId,
                }).ToList(),
                ChildGroups = groupAndAssetData.ChildGroups?.Select(childGroup => Map(childGroup)).ToList()
            };
        }

        /// <summary>
        /// Maps the group and asset data model to the group and asset core model.
        /// </summary>
        /// <param name="groupAndAssetModel">The <see cref="GroupAndAssetModel"/> data model to map.</param>
        /// <returns>A <see cref="GroupAndAssetData"/> core model.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="groupAndAssetModel"/> is null.</exception>
        public static GroupAndAssetData Map(GroupAndAssetModel groupAndAssetModel)
        {
            if (groupAndAssetModel == null)
            {
                throw new ArgumentNullException(nameof(groupAndAssetModel));
            }

            return new GroupAndAssetData()
            {
                GroupName = groupAndAssetModel.GroupName,
                Assets = groupAndAssetModel.Assets?.Select(asset => new AssetData
                {
                    AssetId = asset.AssetId,
                    AssetName = asset.AssetName,
                    IndustryApplicationId = asset.IndustryApplicationId,
                }).ToList(),
                ChildGroups = groupAndAssetModel.ChildGroups?.Select(childGroup => Map(childGroup)).ToList()
            };
        }

        #endregion

    }
}
