using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This interface represents EnumEntity.
    /// </summary>
    public interface IEnumEntity
    {

        /// <summary>
        /// Fetches the ESP pump with a specified ID
        /// </summary>
        /// <returns>The ESP pump with the specified ID if found; otherwise, null</returns>
        public IList<EnumEntityModel> GetAnalysisTypeEntities();

        /// <summary>
        /// Gets the AnalysisTypeEntities data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetCorrelationEntities();

        /// <summary>
        /// Gets the AnalysisTypeEntities data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetCorrelationTypeEntities();

        /// <summary>
        /// Gets the AnalysisCurveSetTypes data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetAnalysisCurveSetTypes();

        /// <summary>
        /// Gets the Application data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetApplicationEntities();

        /// <summary>
        /// Gets the UnitTypes data.
        /// </summary>
        /// <returns>The <seealso cref="EnumEntityModel"/>.</returns>
        public IList<EnumEntityModel> GetUnitCategories();

        /// <summary>
        /// Gets the CurveTypes.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The IDictionary of int</returns>
        public IDictionary<int, int> GetCurveTypes(int key);

        /// <summary>
        /// Gets the GLFlowControlDeviceState.
        /// </summary>
        ///// <returns>The IDictionary of int</returns>
        public IDictionary<int, int> GetGLFlowControlDeviceState();

        /// <summary>
        /// Gets the GLValveConfigurationOption.
        /// </summary>
        /// <returns>The IDictionary of int</returns>
        public IDictionary<int, int> GetGLValveConfigurationOption();

    }
}
