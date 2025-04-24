using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents node master.
    /// </summary>
    public interface INodeMaster
    {

        /// <summary>
        /// Get the node id from node master by asset id.
        /// </summary>
        /// <param name="assetId">The asset id</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="string"/>The node.</returns>
        string GetNodeIdByAsset(Guid assetId, string correlationId);

        /// <summary>
        /// Gets a node's port id by its asset guid.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="portId">The port id retrieved, returned as an out variable.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>
        /// True if port id successfully retrieved from node, otherwise, false.
        /// Additionally, the port id is returned through the out variable <paramref name="portId"/>.
        /// </returns>
        bool TryGetPortIdByAssetGUID(Guid assetId, out short portId, string correlationId);

        /// <summary>
        /// Gets a node's poctype id by its asset guid.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="pocTypeId">The poctype id retrieved, returned as an out variable.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>
        /// True if poc type successfully retrieved from node, otherwise, false.
        /// Additionally, the poctype id is returned through the out variable <paramref name="pocTypeId"/>.
        /// </returns>
        bool TryGetPocTypeIdByAssetGUID(Guid assetId, out short pocTypeId, string correlationId);

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="NodeMasterModel"/></returns>
        public NodeMasterModel GetNode(Guid assetId, string correlationId);

        /// <summary>
        /// Gets the NodeMaster data based on node id.
        /// </summary>
        /// <param name="nodeId">The asset's node id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The NodeMaster data.</returns>
        public NodeMasterModel GetNode(string nodeId, string correlationId);

        /// <summary>
        /// Gets the NodeMasterData.
        /// </summary>
        /// <param name="guid">The guid.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="NodeMasterDictionary"/></returns>
        public NodeMasterDictionary GetNodeMasterData(Guid guid, string[] columns, string correlationId);

        /// <summary>
        /// Get the node id from node master by asset id.
        /// </summary>
        /// <param name="assetIds">The asset ids</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="List{NodeMasterModel}"/> node names</returns>
        IList<NodeMasterModel> GetNodeIdsByAssetGuid(string[] assetIds, string correlationId);

        /// <summary>
        /// Determines if the wells are running comms on the legacy system.
        /// </summary>
        /// <param name="assetIds">The list of asset GUIDs.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A dictionary containing the asset GUIDs as keys and a boolean value indicating
        /// if the well is running on the legacy system.</returns>
        Task<IDictionary<Guid, bool>> GetLegacyWellAsync(IList<Guid> assetIds, string correlationId);

        /// <summary>
        /// Retrieves the node data based on asset ids.
        /// </summary>
        /// <param name="assetId">The asset ids.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The list of <seealso cref="NodeMasterModel"/>.</returns>
        Task<IList<NodeMasterModel>> GetByAssetIdsAsync(IList<Guid> assetId, string correlationId);        

        /// <summary>
        /// Filters the new architecture wells.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="List{Guid}"/> object containing New Architecture asset guids.</returns>
        List<Guid> GetNewArchitectureWells(string correlationId);
    }
}
