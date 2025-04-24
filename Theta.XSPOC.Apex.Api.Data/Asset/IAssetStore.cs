using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Asset
{
    /// <summary>
    /// This is the interface that defines the methods for the asset store.
    /// </summary>
    public interface IAssetStore
    {

        /// <summary>
        /// Gets the asset status data for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the status data.</param>
        /// <returns>
        /// A <seealso cref="RodLiftAssetStatusCoreData"/> that contains the asset status data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then <c>null</c> is returned.
        /// </returns>
        Task<RodLiftAssetStatusCoreData> GetAssetStatusDataAsync(Guid assetId);

        /// <summary>
        /// Gets the list of rod string for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the rod strings.</param>
        /// <returns>
        /// A <seealso cref="IList{RodStrings}"/> that contains the rod strings for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        Task<IList<RodStringData>> GetRodStringAsync(Guid assetId);

        /// <summary>
        /// Gets the esp motor infomation for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the esp motors.</param>
        /// <returns>
        /// A <seealso cref="ESPMotorInformationModel"/> that contains the ESP Motors for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        Task<ESPMotorInformationModel> GetESPMotorInformation(Guid assetId);

        /// <summary>
        /// Gets the esp pump infomation for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the esp motors.</param>
        /// <returns>
        /// A <seealso cref="ESPPumpInformationModel"/> that contains the ESP Motors for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        Task<ESPPumpInformationModel> GetESPPumpInformation(Guid assetId);

        /// <summary>
        /// Gets the gl analysis infomation for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the esp motors.</param>
        /// <returns>
        /// A <seealso cref="GLAnalysisInformationModel"/> that contains the GL Analysis Details for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        Task<GLAnalysisInformationModel> GetGasLiftAnalysisInformation(Guid assetId);

        /// <summary>
        /// Gets the gl analysis infomation for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the esp motors.</param>
        /// <returns>
        /// A <seealso cref="GLAnalysisInformationModel"/> that contains the GL Analysis Details for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        Task<ChemicalInjectionInformationModel> GetChemicalInjectionInformation(Guid assetId);

        /// <summary>
        /// Gets the ip host name from port master by a node's port id.
        /// </summary>
        /// <param name="portId">The port id.</param>
        /// <returns>The ip host name of the port master entry.</returns>
        Task<string> GetIpHostNameByPortId(short? portId);

        /// <summary>
        /// Gets the curr raw scan data information for the provided <paramref name="nodeId"/>.
        /// </summary>
        /// <param name="nodeId">The asset id used to get the curr raw scan data information.</param>
        /// <param name="registerList">The register list.</param>
        /// <returns>
        /// A dictionary containing the address and values for the <paramref name="registerList"/>.
        /// If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        Dictionary<string, float?> GetCurrRawScanDataItems(string nodeId, List<int> registerList);

        /// <summary>
        /// Gets the curr raw scan data information for the provided <paramref name="nodeId"/>.
        /// </summary>
        /// <param name="nodeId">The asset id used to get the curr raw scan data information.</param>
        /// <param name="tagList">The Parameter Tag.</param>
        /// <returns>
        /// If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        Dictionary<string, string> GetCurrRawScanDataItemsStringValue(string nodeId, List<string> tagList);

    }
}
