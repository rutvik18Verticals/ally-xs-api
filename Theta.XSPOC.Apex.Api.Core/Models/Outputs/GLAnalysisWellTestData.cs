using System;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the class for <seealso cref="GLAnalysisWellTestData"/>.
    /// </summary>
    public class GLAnalysisWellTestData
    {

        /// <summary>
        /// The gl analysis well test date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The analysis type id.
        /// </summary>
        public int AnalysisTypeId { get; set; }

        /// <summary>
        /// The analysis type name.
        /// </summary>
        public string AnalysisTypeName { get; set; }

        /// <summary>
        /// The analysis result id.
        /// </summary>
        public int? AnalysisResultId { get; set; }

        /// <summary>
        /// Implements the <seealso cref="IComparable{T}"/> interface for <see cref="GLAnalysisWellTestData"/>
        /// </summary>
        /// <param name="key">The <see cref="GLAnalysisWellTestData"/> to compare to</param>
        /// <returns>An int value specifying greater than (1), less than(-1) or equal to (0)</returns>
        public int CompareTo(GLAnalysisWellTestData key)
        {
            if (AnalysisTypeId == key?.AnalysisTypeId && Date == key.Date && AnalysisResultId == key.AnalysisResultId)
            {
                return 0;
            }

            if (AnalysisTypeId == AnalysisType.WellTest.Key && key.AnalysisTypeId == AnalysisType.WellTest.Key &&
                Date < key.Date)
            {
                return -1;
            }

            if (AnalysisTypeId == AnalysisType.Sensitivity.Key && key.AnalysisTypeId == AnalysisType.Sensitivity.Key
                && (Date < key.Date || (Date == key.Date &&
                        AnalysisResultId is int int1 && key.AnalysisResultId is int @int &&
                        int1 < @int)
                ))
            {
                return -1;
            }

            if (AnalysisTypeId == AnalysisType.Sensitivity.Key && key.AnalysisTypeId == AnalysisType.WellTest.Key
                && Date <= key.Date)
            {
                return -1;
            }

            if (AnalysisTypeId == AnalysisType.WellTest.Key && key.AnalysisTypeId == AnalysisType.Sensitivity.Key
                && Date < key.Date)
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// Implements <seealso cref="IComparable"/>
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>The result of the comparison</returns>
        public int CompareTo(object obj)
        {
            if (obj is not GLAnalysisWellTestData)
            {
                throw new InvalidOperationException("The provided object is not supported");
            }

            return CompareTo((GLAnalysisWellTestData)obj);
        }

    }
}
