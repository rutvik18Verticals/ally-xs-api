using System;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.NodeMasterSelectedColumnsOutput to NodeMasterSelectedColumnsOutput object
    /// </summary>
    public static class NodeMasterColumnMapper
    {

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.NodeMasterSelectedColumnsOutput"/> core model to
        /// <seealso cref="NodeMasterSelectedColumnResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.NodeMasterSelectedColumnsOutput"/> object</param>
        /// <returns>The <seealso cref="NodeMasterSelectedColumnResponse"/></returns>
        public static NodeMasterSelectedColumnResponse Map(string correlationId, Core.Models.Outputs.NodeMasterSelectedColumnsOutput coreModel)
        {
            if (coreModel == null || coreModel.Data == null || coreModel.Data.Count == 0)
            {
                return null;
            }

            NodeMasterSelectedColumnsOutput nodeMasterSelectedColumnsOutput = new NodeMasterSelectedColumnsOutput();

            foreach (var item in coreModel.Data)
            {
                nodeMasterSelectedColumnsOutput.Data.Add(item.Key, item.Value);
            }

            return new NodeMasterSelectedColumnResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = nodeMasterSelectedColumnsOutput
            };
        }

    }
}
