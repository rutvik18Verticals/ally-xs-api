using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.Common
{
    /// <summary>
    /// Represents the CommonWorkingSet.
    /// </summary>
    public class CommonWorkingSetModel
    {

        /// <summary>
        /// Get and set the nodemaster.
        /// </summary>
        public IList<NodeMasterModel> NodeMasterModels { get; set; }

        /// <summary>
        /// Get and set the WellStatistic.
        /// </summary>
        public IList<WellStatisticModel> WellStatisticModel { get; set; }

        /// <summary>
        /// Get and set the WellDetail.
        /// </summary>
        public IList<WellDetailsModel> WellDetailModel { get; set; }

        /// <summary>
        /// Get and set the String.
        /// </summary>
        public IList<StringsModel> StringModel { get; set; }

        /// <summary>
        /// Get and set the vwAggregateCameraAlarmStatus.
        /// </summary>
        public IList<VwAggregateCameraAlarmStatusModel> VwAggregateCameraAlarmStatusModel { get; set; }

        /// <summary>
        /// Get and set the vwOperationalScoresLast.
        /// </summary>
        public IList<VwOperationalScoresLastModel> VwOperationalScoresLastModel { get; set; }

        /// <summary>
        /// Get and set the vw30DayRuntimeAverage.
        /// </summary>
        public IList<Vw30DayRuntimeAverageModel> Vw30DayRuntimeAverageModel { get; set; }

    }
}
