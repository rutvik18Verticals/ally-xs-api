using System;
//using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Responses;
//using Theta.XSPOC.Apex.Api.Core.Common;
//using DataPoint = Theta.XSPOC.Apex.Api.Contracts.Responses.Values.DataPoint;

namespace Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Mappers
{
    /// <summary>
    /// Maps <seealso cref="Core.Models.Outputs.DataHistoryListOutput"/> to DataHistoryResponse object.
    /// </summary>
    public static class TimeSeriesMapper
    {
        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.TimeSeriesOutput"/> core model to
        /// <seealso cref="TimeSeriesTrendDataResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.TimeSeriesTrendDataOutput"/> object.</param>
        /// <returns>The <seealso cref="TimeSeriesTrendDataResponse"/></returns>
        public static AllyTimeSeriesTrendDataResponse Map(string correlationId,
            Core.Models.Outputs.TimeSeriesOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null || coreModel.Values.Count == 0)
            {
                return null;
            }

            var responseValues = coreModel.Values
                .Select(x => new AllyTimeSeriesTrendDataResponseValues
                {
                    AssetId = x.AssetID,                   
                    TagId = x.TagId,                   
                    Value = x.Value,                   
                    Timestamp = x.Timestamp,
                }).ToList();

            return new AllyTimeSeriesTrendDataResponse()
            {
                TotalRecords = coreModel.Values[0].TotalRecords,
                TotalPages = coreModel.Values[0].TotalPages,
                DateCreated = DateTime.Parse(DateTime.UtcNow.ToString()).ToUniversalTime(),
                Id = correlationId,
                Values = responseValues
            };
        }

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.TimeSeriesOutput"/> core model to
        /// <seealso cref="TimeSeriesDataResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.TimeSeriesOutput"/> object.</param>
        /// <returns>The <seealso cref="TimeSeriesDataResponse"/></returns>
        public static TimeSeriesDataResponse MapTimeSeriesData(string correlationId,
            Core.Models.Outputs.TimeSeriesOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null || coreModel.Values.Count == 0)
            {
                return null;
            }

            var responseValues = coreModel.Values
                .Select(x => new TimeSeriesDataResponseValues
                {
                    AssetId = x.AssetID,
                    TagIds = x.TagIds,
                    Values = x.Values,
                    Timestamp = x.Timestamp,
                }).ToList();

            return new TimeSeriesDataResponse()
            {
                TotalRecords = coreModel.Values[0].TotalRecords,
                TotalPages = coreModel.Values[0].TotalPages,
                DateCreated = DateTime.Parse(DateTime.UtcNow.ToString()).ToUniversalTime(),
                Id = correlationId,
                Values = responseValues
            };
        }

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.AssetDetailsOutput"/> core model to
        /// <seealso cref="AssetDetailsDataResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.AssetDetailsOutput"/> object.</param>
        /// <returns>The <seealso cref="AssetDetailsDataResponse"/></returns>
        public static AssetDetailsDataResponse MapAssetDetails(string correlationId,
           Core.Models.Outputs.AssetDetailsOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null || coreModel.Values.Count == 0)
            {
                return null;
            }

            var responseValues = coreModel.Values
                .Select(x => new AssetDetailsResponseValues
                {
                    CustomerId = x.CustomerId,
                    AssetID = x.AssetID,
                    Name = x.Name,
                    PocType = x.PocType,
                    ApplicationId = x.ApplicationId,
                    IsEnabled = x.IsEnabled
                }).ToList();

            return new AssetDetailsDataResponse()
            {   
                DateCreated = DateTime.Parse(DateTime.UtcNow.ToString()).ToUniversalTime(),
                Id = correlationId,
                Values = responseValues
            };
        }

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.ParameterStandardTypeOutput"/> core model to
        /// <seealso cref="ParameterStandardTypeDataResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.ParameterStandardTypeOutput"/> object.</param>
        /// <returns>The <seealso cref="ParameterStandardTypeDataResponse"/></returns>
        public static ParameterStandardTypeDataResponse MapParameterStandardTypes(string correlationId,
         Core.Models.Outputs.ParameterStandardTypeOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null || coreModel.Values.Count == 0)
            {
                return null;
            }

            var responseValues = coreModel.Values
                .Select(x => new ParameterStandardTypeResponseValues
                {
                   Id = x.Id,
                   Description = x.Description,
                }).ToList();

            return new ParameterStandardTypeDataResponse()
            {
                DateCreated = DateTime.Parse(DateTime.UtcNow.ToString()).ToUniversalTime(),
                Id = correlationId,
                Values = responseValues
            };
        }

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.ValidateCustomerOutput"/> core model to
        /// <seealso cref="ValidateCustomerDataResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.ValidateCustomerOutput"/> object.</param>
        /// <returns>The <seealso cref="ValidateCustomerDataResponse"/></returns>
        public static ValidateCustomerDataResponse MapValidateCustomer(string correlationId,
           Core.Models.Outputs.ValidateCustomerOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null || coreModel.Values.Count == 0)
            {
                return null;
            }

            var responseValues = coreModel.Values
                .Select(x => new ValidateCustomerResponseValues
                {
                    UserAccount = x.UserAccount,
                    IsValid = x.IsValid
                }).ToList();

            return new ValidateCustomerDataResponse()
            {
                DateCreated = DateTime.Parse(DateTime.UtcNow.ToString()).ToUniversalTime(),
                Id = correlationId,
                Values = responseValues
            };
        }
    }
}
