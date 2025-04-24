using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common.Communications.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;

namespace Theta.XSPOC.Apex.Api.Common.Communications
{
    /// <summary>
    /// Service which prepares data for an xspoc transaction object, 
    /// and returns it in the form of a <see cref="UpdatePayload"/>.
    /// </summary>
    public interface ITransactionPayloadCreator
    {

        /// <summary>
        /// Creates a transaction payload for a read register action.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="addresses">The addresses.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="payload">The payload to populate, as an out variable.</param>
        /// <returns>
        /// A <see cref="MethodResult{T}"/> containing a success/error message.
        /// Additionally, the method returns the transaction payload through 
        /// the out variable <paramref name="payload"/>.
        /// </returns>
        MethodResult<string> CreateReadRegisterPayload(
            Guid assetId, string[] addresses, string correlationId, out UpdatePayload payload);

        /// <summary>
        /// Creates a transaction payload for a write register action.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="addressValues">A dictionary of address values.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="payload">The payload to populate, as an out variable.</param>
        /// <returns>
        /// A <see cref="MethodResult{T}"/> containing a success/error message.
        /// Additionally, the method returns the transaction payload through 
        /// the out variable <paramref name="payload"/>.
        /// </returns>
        MethodResult<string> CreateWriteRegisterPayload(
            Guid assetId, IDictionary<string, string> addressValues, string correlationId, out UpdatePayload payload);

        /// <summary>
        /// Creates a transaction payload for a well control action.
        /// NOTE: This overloaded method includes <paramref name="equipmentSelection"/> option which is used for upload equipment control type.
        /// </summary>
        /// <param name="payload">The payload to populate, as an out variable.</param>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="deviceControlType">The device control type which specifies what action to perform on a well.</param>
        /// <param name="equipmentSelection">The equipment selection. Used to specify which equipment to upload for a upload equipment control type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>
        /// A <see cref="MethodResult{T}"/> containing a success/error message.
        /// Additionally, the method returns the transaction payload through 
        /// the out variable <paramref name="payload"/>.
        /// </returns> 
        MethodResult<string> CreateWellControlPayload(out UpdatePayload payload, Guid assetId,
            DeviceControlType deviceControlType, int equipmentSelection, string correlationId);

        /// <summary>
        /// Creates a transaction payload for a well control action.
        /// </summary>
        /// <param name="payload">The payload to populate, as an out variable.</param>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="deviceControlType">The device control type which specifies what action to perform on a well.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>
        /// A <see cref="MethodResult{T}"/> containing a success/error message.
        /// Additionally, the method returns the transaction payload through 
        /// the out variable <paramref name="payload"/>.
        /// </returns> 
        MethodResult<string> CreateWellControlPayload(out UpdatePayload payload, Guid assetId,
            DeviceControlType deviceControlType, string correlationId);

    }
}
