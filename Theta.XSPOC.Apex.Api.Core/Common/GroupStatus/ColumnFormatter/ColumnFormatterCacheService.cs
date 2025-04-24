using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a cache service for column formatters.
    /// </summary>
    public class ColumnFormatterCacheService : IColumnFormatterCacheService
    {

        #region Private Fields

        private readonly IRod _rodStore;
        private readonly IPumpingUnit _pumpingUnitStore;
        private readonly IPumpingUnitManufacturer _pumpingUnitManufacturerStore;
        private readonly IDictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly IException _exceptionStore;
        private readonly IHostAlarm _hostAlarmStore;
        private readonly IPocType _pocTypeStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnFormatterCacheService"/> class.
        /// </summary>
        /// <param name="rodStore">The rod store.</param>
        /// <param name="pumpingUnitStore">The pumping unit store.</param>
        /// <param name="pumpingUnitManufacturerStore">The pumping unit manufacturer store.</param>
        /// <param name="exceptionStore">The exception store.</param>
        /// <param name="hostAlarmStore">The host alarm store.</param>
        /// <param name="pocTypeStore">The poc type store.</param>
        public ColumnFormatterCacheService(IRod rodStore, IPumpingUnit pumpingUnitStore,
            IPumpingUnitManufacturer pumpingUnitManufacturerStore, IException exceptionStore, IHostAlarm hostAlarmStore, IPocType pocTypeStore)
        {
            _rodStore = rodStore ?? throw new ArgumentNullException(nameof(rodStore));
            _pumpingUnitStore = pumpingUnitStore ?? throw new ArgumentNullException(nameof(pumpingUnitStore));
            _pumpingUnitManufacturerStore = pumpingUnitManufacturerStore ??
                throw new ArgumentNullException(nameof(pumpingUnitManufacturerStore));
            _exceptionStore = exceptionStore ?? throw new ArgumentNullException(nameof(exceptionStore));
            _hostAlarmStore = hostAlarmStore ?? throw new ArgumentNullException(nameof(hostAlarmStore));
            _pocTypeStore = pocTypeStore ?? throw new ArgumentNullException(nameof(pocTypeStore));
        }

        #endregion

        #region IColumnFormatterCacheService Members

        /// <summary>
        /// Gets the cached data for the specified name and node list.
        /// </summary>
        /// <param name="name">The name of the data.</param>
        /// <param name="nodeList">The list of nodes.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The cached data.</returns>
        public object GetData(string name, IList<string> nodeList, string correlationId)
        {
            if (string.IsNullOrWhiteSpace(name) || nodeList == null)
            {
                return null;
            }

            if (_cache.TryGetValue(name.ToUpper(), out object cacheValue))
            {
                return cacheValue;
            }

            object cache = name.ToUpper() switch
            {
                "ROD GRADE" => _rodStore.GetRodForGroupStatus(nodeList, correlationId),
                "PUMPING UNIT MANUFACTURER" => _pumpingUnitManufacturerStore.GetManufacturers(nodeList, correlationId),
                "PUMPING UNIT" => _pumpingUnitStore.GetUnitNames(nodeList, correlationId),
                "EXCEPTIONGROUPNAME" => _exceptionStore.GetExceptions(nodeList, correlationId),
                "HOSTALARMS" => _hostAlarmStore.GetAllGroupStatusHostAlarms(nodeList, correlationId),
                "CONTROLLER" => _pocTypeStore.GetAll(correlationId),
                "POCTYPE" => _pocTypeStore.GetAll(correlationId),
                "POC TYPE" => _pocTypeStore.GetAll(correlationId),
                _ => null
            };

            if (cache != null)
            {
                _cache[name.ToUpper()] = cache;
            }

            return cache;
        }

        #endregion

    }
}
