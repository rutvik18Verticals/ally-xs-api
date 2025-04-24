using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// A service which handles well control operations.
    /// </summary>
    public class WellControlService : IWellControlService
    {

        #region Private Fields

        private readonly IControlAction _controlActionStore;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an new instance of a <see cref="WellControlService"/>.
        /// </summary>
        /// <param name="controlActionStore">The control action store.</param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="controlActionStore"/> is null.
        /// </exception>
        public WellControlService(IControlAction controlActionStore, IThetaLoggerFactory loggerFactory)
        {
            _controlActionStore = controlActionStore ?? throw new ArgumentNullException(nameof(controlActionStore));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IWellControlService Implementation

        /// <summary>
        /// Gets the supported control actions for the well represented 
        /// by the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The supported control actions for the well represented 
        /// by the provided <paramref name="assetId"/>.</returns>
        public GetWellControlActionsOutput GetWellControlActions(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.UserDefault);
            logger.WriteCId(Level.Trace, $"Starting {nameof(WellControlService)} {nameof(GetWellControlActions)}", correlationId);
            var output = new GetWellControlActionsOutput()
            {
                WellControlActions = new List<WellControlAction>(),
            };

            if (assetId == Guid.Empty)
            {
                output.Result = new MethodResult<string>(true, "Asset id is empty.");

                return output;
            }

            var wellControlActions = _controlActionStore.GetControlActions(assetId, correlationId);

            foreach (var wellControlAction in wellControlActions)
            {
                output.WellControlActions.Add(new WellControlAction()
                {
                    Id = wellControlAction.Id,
                    Name = wellControlAction.Name,
                });
            }

            output.Result = new MethodResult<string>(true, "Retrieved control actions successfully.");
            logger.WriteCId(Level.Trace, $"Finished {nameof(WellControlService)} {nameof(GetWellControlActions)}", correlationId);

            return output;
        }

        #endregion

    }
}
