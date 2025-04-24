using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.GroupAndAssetDataMapper to GroupAndAssetResponse object
    /// </summary>
    public static class GroupAndAssetDataMapper
    {

        /// <summary>
        /// Maps the <seealso cref="GroupAndAssetDataOutput"/> core model to
        /// <seealso cref="GroupAndAssetResponse"/> object.
        /// </summary>
        /// <param name="coreModel">The <seealso cref="GroupAndAssetDataOutput"/> object</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="GroupAndAssetResponse"/></returns>
        public static GroupAndAssetResponse Map(GroupAndAssetDataOutput coreModel, string correlationId)
        {
            if (coreModel?.Values == null)
            {
                return null;
            }

            var values = Map(coreModel.Values);

            return new GroupAndAssetResponse()
            {
                Id = correlationId,
                DateCreated = DateTime.UtcNow,
                Values = values,
            };
        }

        private static IList<Responses.Values.GroupAndAssetData> Map(GroupAndAssetModel input)
        {
            if (input == null)
            {
                return null;
            }

            var result = new List<Responses.Values.GroupAndAssetData>
            {
                new Responses.Values.GroupAndAssetData()
                {
                    GroupName = input.GroupName,
                    Assets = Map(input.Assets),
                    ChildGroups = Map(input.ChildGroups),
                }
            };

            return result;
        }

        private static Responses.Values.GroupAndAssetData MapOne(GroupAndAssetModel input)
        {
            if (input == null)
            {
                return null;
            }

            return new Responses.Values.GroupAndAssetData()
            {
                ChildGroups = Map(input.ChildGroups),
                Assets = Map(input.Assets),
                GroupName = input.GroupName,
            };
        }

        private static IList<Responses.Values.GroupAndAssetData> Map(IList<GroupAndAssetModel> children)
        {
            if (children == null)
            {
                return null;
            }

            return children.Select(m => MapOne(m)).ToList();
        }

        private static IList<Responses.Values.AssetData> Map(IList<AssetModel> inputs)
        {
            return inputs?.Select(m => Map(m)).ToList();
        }

        private static Responses.Values.AssetData Map(AssetModel input)
        {
            if (input == null)
            {
                return null;
            }

            return new Responses.Values.AssetData()
            {
                AssetName = input.AssetName,
                IndustryApplicationId = input.IndustryApplicationId,
                AssetId = input.AssetId,
                WellOpType = input.WellOpType,
                Enabled = input.Enabled,
            };
        }

    }
}
