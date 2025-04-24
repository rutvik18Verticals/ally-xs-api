using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of IWellTestsProcessingService.
    /// </summary>
    public class WellTestsProcessingService : IWellTestsProcessingService
    {

        #region Private Members

        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IWellTests _wellTestsService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="WellTestsProcessingService"/>.
        /// </summary>
        public WellTestsProcessingService(IThetaLoggerFactory loggerFactory, IWellTests wellTestsService)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _wellTestsService = wellTestsService ?? throw new ArgumentNullException(nameof(wellTestsService));
        }

        #endregion

        #region IWellTestProcessingService Implementation

        /// <summary>
        /// Processes the provided well test request and generates a list of date when
        /// well test was performed.
        /// </summary>
        /// <param name="data">The <seealso cref="WithCorrelationId{WellTestInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="WellTestDataOutput"/>.</returns>
        public WellTestDataOutput GetESPAnalysisWellTestData(WithCorrelationId<WellTestInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.WellTest);

            WellTestDataOutput response = new WellTestDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
            };

            if (data == null)
            {
                var message = $"{nameof(data)} is null, cannot get WellTests results.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (data?.Value == null)
            {
                var message = $"{nameof(data)} is null, cannot get WellTest results.";
                logger.WriteCId(Level.Error, message, data?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = data?.CorrelationId;
            var request = data.Value;

            if (request.AssetId == Guid.Empty)
            {
                var message = $"{nameof(request.AssetId)}" +
                    $" should be provided to get Well Tests results.";
                logger.WriteCId(Level.Error, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var listWellTestModelData = _wellTestsService.GetESPWellTestsData(request.AssetId, correlationId);

            if (listWellTestModelData == null)
            {
                response.Result.Status = false;
                response.Result.Value = "well tests is null.";
            }
            else
            {
                response.Values = GetWellTestValuesReponseData(listWellTestModelData);
                response.Result.Status = true;
                response.Result.Value = string.Empty;
            }

            return response;
        }

        /// <summary>
        /// Processes the provided WellTest request and generates WellTest based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="WithCorrelationId{WellTestInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GLAnalysisWellTestDataOutput"/> object.</returns>
        public GLAnalysisWellTestDataOutput GetGLAnalysisWellTestData(WithCorrelationId<WellTestInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.WellTest);

            GLAnalysisWellTestDataOutput response = new GLAnalysisWellTestDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
            };

            if (data == null)
            {
                var message = $"{nameof(data)} is null, cannot get well test";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (data?.Value == null)
            {
                var message = $"{nameof(data)} is null, cannot get well test";
                logger.WriteCId(Level.Error, message, data?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = data?.CorrelationId;
            var request = data.Value;

            if (request.AssetId == Guid.Empty)
            {
                var message = $"{nameof(request.AssetId)}" +
                    $" should be provided to get WellTest results.";
                logger.WriteCId(Level.Error, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var listTblGLAnalysisResultData = _wellTestsService.GetGLAnalysisWellTestsData(
                request.AssetId,
                AnalysisType.Sensitivity.Key,
                AnalysisType.WellTest.Key, data.CorrelationId);

            if (listTblGLAnalysisResultData == null)
            {
                response.Result.Status = false;
                response.Result.Value = "well test results is null.";
                logger.WriteCId(Level.Error, response.Result.Value, correlationId);
            }
            else
            {
                response.Values = GetGLWellTestReponseData(listTblGLAnalysisResultData);
                response.Result.Status = true;
                response.Result.Value = string.Empty;
            }

            return response;
        }

        #endregion

        #region Private Methods

        private IList<WellTestData> GetWellTestValuesReponseData(IList<WellTestModel> listWellTestModelData)
        {
            IList<WellTestData> listWellTestValues = new List<WellTestData>();
            foreach (var wellTestModel in listWellTestModelData)
            {
                WellTestData values = new WellTestData
                {
                    Date = wellTestModel.TestDate,
                    AnalysisTypeName = AnalysisType.WellTest,
                };

                listWellTestValues.Add(values);
            }
            listWellTestValues = listWellTestValues.OrderByDescending(x => x.Date).ToList();

            return listWellTestValues;
        }

        private IList<GLAnalysisWellTestData> GetGLWellTestReponseData(IList<GLAnalysisResultModel> listGLAnalysisResultModel)
        {
            List<GLAnalysisWellTestData> listAnalysisKeyValues = new List<GLAnalysisWellTestData>();

            listAnalysisKeyValues = listGLAnalysisResultModel.Select(x => new GLAnalysisWellTestData()
            {
                Date = x.TestDate,
                AnalysisTypeId = x.AnalysisType,
                AnalysisTypeName = EnhancedEnumBase.GetValue<AnalysisType>(x.AnalysisType),
                AnalysisResultId = x.Id == 0 ? null : x.Id,
            }).ToList();

            listAnalysisKeyValues.Sort((x, y) => x.CompareTo(y));
            listAnalysisKeyValues.Reverse();

            return listAnalysisKeyValues;
        }

        #endregion

    }
}
