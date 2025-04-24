using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// Impletments the IESPPump interface
    /// </summary>
    public class ESPPumpDataSQLStore : SQLStoreBase, IESPPump
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="ESPPumpDataSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public ESPPumpDataSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        #region IESPPump Implementation

        /// <summary>
        /// Fetches the ESP pump with a specified ID
        /// </summary>
        /// <param name="pumpId">The ID of the ESP pump</param>
        /// <param name="correlationId"></param>
        /// <returns>The ESP pump with the specified ID if found; otherwise, null</returns>
        /// <exception cref="ArgumentNullException">id is null.</exception>
        public ESPPumpDataModel GetESPPumpData(object pumpId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(ESPPumpDataSQLStore)} {nameof(GetESPPumpData)}",
                correlationId);

            if (pumpId == null)
            {
                throw new ArgumentNullException(nameof(pumpId));
            }

            ESPPumpDataModel espPumpData = null;

            using (var context = _contextFactory.GetContext())
            {
                var entity = context.ESPPumps.AsNoTracking().FirstOrDefault(x => x.ESPPumpId == (int)pumpId);

                if (entity != null)
                {
                    ESPManufacturerEntity manufacturer = null;

                    if (entity.ManufacturerId.HasValue)
                    {
                        manufacturer =
                            context.ESPManufacturers.AsNoTracking().FirstOrDefault(x => x.ManufacturerId == entity.ManufacturerId.Value);
                    }

                    var curvePoints = context.ESPCurvePoints.Where(x => x.ESPPumpID == entity.ESPPumpId).ToList();
                    List<ESPCurvePointModel> curvePointsModels = new List<ESPCurvePointModel>();
                    foreach (var curvePoint in curvePoints)
                    {
                        curvePointsModels.Add(
                            new ESPCurvePointModel
                            {
                                ESPPumpID = curvePoint.ESPPumpID,
                                Efficiency = curvePoint.Efficiency,
                                FlowRate = curvePoint.FlowRate,
                                HeadFeetPerStage = curvePoint.HeadFeetPerStage,
                                PowerInHP = curvePoint.PowerInHP,
                            }
                        );
                    }

                    espPumpData = new ESPPumpDataModel
                    {
                        ESPPumpId = entity.ESPPumpId,
                        Pump = entity.Pump,
                        MinCasingSize = entity.MinCasingSize,
                        HousingPressureLimit = entity.HousingPressureLimit,
                        MinBPD = entity.MinBPD,
                        MaxBPD = entity.MaxBPD,
                        BEPBPD = entity.HousingPressureLimit,
                        UseCoefficients = entity.UseCoefficients,
                        HeadIntercept = entity.HeadIntercept,
                        HPIntercept = entity.HPIntercept,
                        EfficiencyIntercept = entity.EfficiencyIntercept,
                        Head1Coef = entity.Head1Coef,
                        Head2Coef = entity.Head2Coef,
                        Head3Coef = entity.Head3Coef,
                        Head4Coef = entity.Head4Coef,
                        Head5Coef = entity.Head5Coef,
                        Head6Coef = entity.Head6Coef,
                        Head7Coef = entity.Head7Coef,
                        Head8Coef = entity.Head8Coef,
                        Head9Coef = entity.Head9Coef,
                        HP1Coef = entity.HP1Coef,
                        HP2Coef = entity.HP2Coef,
                        HP3Coef = entity.HP3Coef,
                        HP4Coef = entity.HP4Coef,
                        HP5Coef = entity.HP5Coef,
                        HP6Coef = entity.HP6Coef,
                        HP7Coef = entity.HP7Coef,
                        HP8Coef = entity.HP8Coef,
                        HP9Coef = entity.HP9Coef,
                        Eff1Coef = entity.Eff1Coef,
                        Eff2Coef = entity.Eff2Coef,
                        Eff3Coef = entity.Eff3Coef,
                        Eff4Coef = entity.Eff4Coef,
                        Eff5Coef = entity.Eff5Coef,
                        Eff6Coef = entity.Eff6Coef,
                        Eff7Coef = entity.Eff7Coef,
                        Eff8Coef = entity.Eff8Coef,
                        Eff9Coef = entity.Eff9Coef,
                        Data = entity.Data,
                        Series = entity.Series,
                        PumpModel = entity.PumpModel,
                        CurvePoints = curvePointsModels
                    };

                    if (entity.ManufacturerId.HasValue)
                    {
                        espPumpData.Manufacturer = new ESPManufacturerModel
                        {
                            ManufacturerId = manufacturer.ManufacturerId,
                            Manufacturer = manufacturer.Manufacturer,
                        };
                    }
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(ESPPumpDataSQLStore)}" +
               $" {nameof(GetESPPumpData)}", correlationId);

            return espPumpData;
        }

        #endregion

    }
}
