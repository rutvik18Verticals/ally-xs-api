using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses;

namespace Theta.XSPOC.Apex.Api.WellControl.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.SetpointsGroupOutput to SetpointsResponse object
    /// </summary>
    public static class SetpointGroupDataMapper
    {

        /// <summary>
        /// Maps Core.Models.Outputs.SetpointsGroupOutput to SetpointsResponse object
        /// </summary>
        /// <param name="dataOutput"></param>
        /// <returns></returns>
        public static SetpointsGroupDataResponse Map(SetpointsGroupOutput dataOutput)
        {
            if (dataOutput?.Values == null)
            {
                return null;
            }

            var response = new SetpointsGroupDataResponse
            {
                Values = new List<SetpointsGroupsData>()
            };

            var setpointsValues = dataOutput.Values;

            var values = dataOutput.Values.Select(x => new SetpointsGroupsData()
            {
                SetpointGroupName = x.SetpointGroupName,
                RegisterCount = x.RegisterCount,
                SetpointGroup = x.SetpointGroup,
                Setpoints = MapSetpointsData(x.Setpoints)
            }).ToList();

            response.DateCreated = DateTime.UtcNow;
            response.Values = values;

            return response;
        }

        private static List<SetpointData> MapSetpointsData(List<SetpointData> setpoints)
        {
            return setpoints.Select(x => new SetpointData()
            {
                Parameter = x.Parameter,
                BackupValue = x.BackupValue,
                BackupDate = x.BackupDate,
                Description = x.Description,
                IsSupported = x.IsSupported,
                BackUpLookUpValues = x.BackUpLookUpValues,
            }).ToList();
        }
    }
}
