using System;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Service which handles processing of asset data.
    /// </summary>
    public class AssetDataService : IAssetDataService
    {

        #region Private Fields

        private readonly IAssetData _assetDataStore;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of a <see cref="AssetDataService"/>.
        /// </summary>
        public AssetDataService(IAssetData assetDataStore, IThetaLoggerFactory loggerFactory)
        {
            _assetDataStore = assetDataStore ?? throw new ArgumentNullException(nameof(assetDataStore));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IAssetDataService Implementation

        /// <summary>
        /// Gets the enabled status of a asset by its <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The asset guid.</param>
        /// <returns>The well's enabled status data.</returns>
        public WellEnabledStatusOutput GetEnabledStatus(WithCorrelationId<Guid> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.APIService);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AssetDataService)} {nameof(GetEnabledStatus)}", input?.CorrelationId);

            if (input == null)
            {
                logger.Write(Level.Info, "Missing input");
                logger.Write(Level.Trace, $"Starting {nameof(AssetDataService)} {nameof(GetEnabledStatus)}");

                throw new ArgumentNullException(nameof(input));
            }

            var enabled = _assetDataStore.GetWellEnabledStatus(input.Value, input.CorrelationId);

            if (enabled == null)
            {
                logger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataService)} {nameof(GetEnabledStatus)}", input.CorrelationId);

                return new WellEnabledStatusOutput()
                {
                    Result = new MethodResult<string>(false, "Unable to retrieve well enabled status.")
                };
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataService)} {nameof(GetEnabledStatus)}", input.CorrelationId);

            return new WellEnabledStatusOutput()
            {
                Enabled = enabled.Value,
                Result = new MethodResult<string>(true, ""),
            };
        }

        #endregion

    }
}
