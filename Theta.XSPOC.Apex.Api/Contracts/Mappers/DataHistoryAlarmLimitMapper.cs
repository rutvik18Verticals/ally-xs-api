using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using DataHistoryAlarmLimitsValues = Theta.XSPOC.Apex.Api.Contracts.Responses.Values.DataHistoryAlarmLimitsValues;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps <seealso cref="DataHistoryAlarmLimitsOutput"/> to DataHistoryAlarmLimitsResponse object.
    /// </summary>
    public static class DataHistoryAlarmLimitMapper
    {

        /// <summary>
        /// Maps the <seealso cref="DataHistoryAlarmLimitsOutput"/> core model to
        /// <seealso cref="DataHistoryAlarmLimitsResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="dataOutput">The <seealso cref="DataHistoryAlarmLimitsOutput"/> object.</param>
        /// <returns>The <seealso cref="DataHistoryAlarmLimitsResponse"/></returns>
        public static DataHistoryAlarmLimitsResponse Map(string correlationId, DataHistoryAlarmLimitsOutput dataOutput)
        {
            if (dataOutput?.Values == null)
            {
                return null;
            }

            var responseValues = new List<DataHistoryAlarmLimitsValues>();

            foreach (var value in dataOutput.Values.AsEnumerable())
            {
                responseValues.Add(new DataHistoryAlarmLimitsValues
                {
                    Address = value.Address,
                    HiHiLimit = value.HiHiLimit,
                    HiLimit = value.HiLimit,
                    LoLoLimit = value.LoLoLimit,
                    LoLimit = value.LoLimit,
                });
            }

            var response = new DataHistoryAlarmLimitsResponse
            {
                Id = correlationId,
                DateCreated = DateTime.UtcNow,
                Values = responseValues,
            };

            return response;
        }

    }
}
