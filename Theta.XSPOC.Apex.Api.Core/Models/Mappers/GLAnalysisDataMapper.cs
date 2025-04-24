using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Mappers
{
    /// <summary>
    /// Maps the GlAnalysisSurveyDateModel to GLAnalysisData object.
    /// </summary>
    public class GLAnalysisDataMapper
    {

        #region Public Methods

        /// <summary>
        /// Maps the <paramref name="entity"/> to a <seealso cref="GlAnalysisSurveyDateModel"/> domain object.
        /// </summary>
        /// <param name="entity">A <seealso cref="GLAnalysisData"/> domain object.</param>
        /// <returns>A <seealso cref="GLAnalysisData"/> representing the provided <paramref name="entity"/> 
        /// in the domain.</returns>
        public static GLAnalysisData MapToDomainObject(GlAnalysisSurveyDateModel entity)
        {
            var result = new GLAnalysisData()
            {
                Date = entity != null ? entity.SurveyDate : DateTime.Now,
            };

            return result;
        }

        /// <summary>
        /// Maps the <paramref name="entity"/> to a <seealso cref="GlAnalysisSurveyCurveCoordinateDataModel"/> domain object.
        /// </summary>
        /// <param name="entity">A <seealso cref="GLAnalysisCurveCoordinateData"/> domain object.</param>
        /// <returns>A <seealso cref="GLAnalysisCurveCoordinateData"/> representing the provided <paramref name="entity"/> 
        /// in the domain.</returns>
        public static GLAnalysisCurveCoordinateData MapToCurveCoordinateDomainObject(SurveyData entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = new GLAnalysisCurveCoordinateData()
            {
                Id = (int)(entity != null ? entity.Id : 0),
                CurveTypeId = entity != null ? entity.SurveyCurve.GetCurveTypeId() : 0,
                Name = EnhancedEnumBase.GetValue<SurveyCurveType>(entity.SurveyCurve.GetCurveTypeId()).Name.Amount
                    .Replace(" ", string.Empty),
                DisplayName =
                    entity != null
                        ? EnhancedEnumBase.GetValue<SurveyCurveType>(entity.SurveyCurve.GetCurveTypeId())
                        : string.Empty,
                CoordinatesOutput = entity.SurveyCurve.GetCurveCoordinateList().Select(c => new CoordinatesData<float>()
                {
                    X = (float)c.XValue,
                    Y = (float)c.YValue,
                }).ToList(),
            };

            return result;
        }

        #endregion

    }
}
