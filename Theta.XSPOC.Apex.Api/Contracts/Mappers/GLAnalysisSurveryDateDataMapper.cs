using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.GLAnalysisDataOutput to GLAnalysisSurveyDateResponse object.
    /// </summary>
    public static class GLAnalysisSurveyDateDataMapper
    {

        /// <summary>
        /// Maps the <seealso cref="GLAnalysisSurveyDataOutput"/> core model to
        /// <seealso cref="GLAnalysisSurveyDateResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="GLAnalysisSurveyDataOutput"/> object</param>
        /// <returns>The <seealso cref="GLAnalysisSurveyDateResponse"/></returns>
        public static GLAnalysisSurveyDateResponse Map(string correlationId, GLAnalysisSurveyDataOutput coreModel)
        {
            if (coreModel?.Values == null)
            {
                return new GLAnalysisSurveyDateResponse()
                {
                    DateCreated = DateTime.UtcNow,
                    Id = correlationId,
                    Values = new List<Responses.Values.GLAnalysisData>(),
                };
            }

            var values = coreModel.Values.Select(x => new Responses.Values.GLAnalysisData()
            {
                Date = x.Date,
            }).ToList();

            return new GLAnalysisSurveyDateResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = values,
            };
        }

    }
}
