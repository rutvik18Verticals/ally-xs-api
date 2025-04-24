using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Services
{
    /// <summary>
    /// Implementation of ISetpointGroupProcessingService.
    /// </summary>
    public class SetpointGroupProcessingService : ISetpointGroupProcessingService
    {

        #region Private Members

        private readonly ISetpointGroup _service;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="SetpointGroupProcessingService"/>.
        /// </summary>
        /// <param name="service">The <seealso cref="ISetpointGroup"/> service.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="service"/> is null
        /// or
        /// <paramref name="loggerFactory"/> is null
        /// </exception>
        public SetpointGroupProcessingService(ISetpointGroup service, IThetaLoggerFactory loggerFactory)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region ISetpointGroupProcessingService Implementation

        /// <summary>
        /// Get setpoint groups and setpoint registers
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="SetpointsGroupOutput"/> data.</returns>
        public SetpointsGroupOutput GetSetpointGroups(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.WellControl);

            var response = new SetpointsGroupOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            var setpointsGroupData = _service.GetSetPointGroupData(assetId, correlationId);

            if (setpointsGroupData == null)
            {
                var message = $"{nameof(setpointsGroupData)} is null, cannot get results.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;
            }
            else
            {
                var responseValues = setpointsGroupData;

                response.Values = CreateResponse(responseValues.ToList());
                response.Result.Status = true;
                response.Result.Value = string.Empty;
            }

            return response;
        }

        #endregion

        #region Private Methods

        private IList<SetPointGroupsData> CreateResponse(List<SetpointGroupModel> responseValues)
        {
            var setPointGroupsData = new List<SetPointGroupsData>();
            if (responseValues != null)
            {
                foreach (var responseValue in responseValues)
                {
                    setPointGroupsData.Add(new SetPointGroupsData()
                    {
                        SetpointGroupName = responseValue.SetpointGroupName,
                        SetpointGroup = responseValue.SetpointGroup,
                        RegisterCount = responseValue.RegisterCount,
                        Setpoints = responseValue.Setpoints == null ? null : MapToDomain(responseValue.Setpoints)
                    });
                }
            }

            return setPointGroupsData;
        }

        private List<SetpointData> MapToDomain(List<SetpointModel> setpoints)
        {
            var setpointData = new List<SetpointData>();

            foreach (var setpoint in setpoints)
            {
                setpointData.Add(new SetpointData
                {
                    Parameter = setpoint.Parameter,
                    Description = setpoint.Description,
                    BackupDate = setpoint.BackupDate,
                    BackupValue = setpoint.BackupValue,
                    IsSupported = setpoint.IsSupported,
                    BackUpLookUpValues = setpoint.BackUpLookUpValues == null ? null : MapToDomain(setpoint.BackUpLookUpValues)
                });
            }

            return setpointData;
        }

        private List<LookupValue> MapToDomain(List<LookupValueModel> lookupValues)
        {
            var backupLookUps = new List<LookupValue>();
            foreach (var lookup in lookupValues)
            {
                backupLookUps.Add(new LookupValue
                {
                    Text = lookup.Text,
                    Value = lookup.Value,
                });
            }

            return backupLookUps;
        }

        #endregion

    }
}
