using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents analysis correlation operations.
    /// </summary>
    public interface IWellAnalysisCorrelation
    {

        /// <summary>
        /// Get the analysis correlation by id.
        /// </summary>
        /// <returns>The <seealso cref="AnalysisCorrelationModel"/>.</returns>
        public AnalysisCorrelationModel GetAnalysisCorrelation(int? id, string correlationId);

    }
}
