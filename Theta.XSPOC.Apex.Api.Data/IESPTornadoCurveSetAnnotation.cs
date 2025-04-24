using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents esp tornado  curve set annotations.
    /// </summary>
    public interface IESPTornadoCurveSetAnnotation
    {

        /// <summary>
        /// Processes the provided esp tornado  curve set annotations request
        /// and generates esp tornado  curve set annotations based on that data.
        /// </summary>
        /// <returns>The <seealso cref="ESPTornadoCurveSetAnnotationModel"/>.</returns>
        IList<ESPTornadoCurveSetAnnotationModel> GetESPTornadoCurveSetAnnotations(string correlationId);

    }
}
