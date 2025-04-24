using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// Impletments the IRodLiftAnalysis interface
    /// </summary>
    public class RodLiftAnalysisSQLStore : SQLStoreBase, IRodLiftAnalysis
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private const string UPLIFT_OPP_MINIMUM_PRODUCTION_THRESHOLD = "UpliftOppMinimumProductionThreshold";
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IDateTimeConverter _dateTimeConverter;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="RodLiftAnalysisSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <param name="localePhrases">The <seealso cref="ILocalePhrases"/>.</param>
        /// <param name="dateTimeConverter">The date time converter.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// or
        /// <paramref name="localePhrases"/> is null.
        /// or
        /// </exception>
        public RodLiftAnalysisSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory,
            ILocalePhrases localePhrases, IThetaLoggerFactory loggerFactory, IDateTimeConverter dateTimeConverter) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _ = localePhrases ?? throw new ArgumentNullException(nameof(localePhrases));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
        }

        #endregion

        #region IRodLiftAnalysis Implementation

        /// <summary>
        /// Get the rod left analysis data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="cardDateString">The card date.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="RodLiftAnalysisResponse"/>.</returns>
        public RodLiftAnalysisResponse GetRodLiftAnalysisData(Guid assetId, string cardDateString, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisSQLStore)} {nameof(GetRodLiftAnalysisData)}", correlationId);

            var cardDate = DateTime.Parse(cardDateString);

            RodLiftAnalysisResponse response = new RodLiftAnalysisResponse();

            using (var context = _contextFactory.GetContext())
            {
                var wellDetail = GetWellDetails(assetId);

                if (wellDetail == null)
                {
                    logger.WriteCId(Level.Info, "Missing well details", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisSQLStore)} {nameof(GetRodLiftAnalysisData)}", correlationId);

                    return null;
                }

                response.WellDetails = wellDetail;

                var cardData = GetCardData(assetId, cardDate);

                var wellTest = GetWellTestData(assetId, cardDate);

                var node = GetNodeMasterData(assetId);

                response.NodeMasterData = node;
                response.CardData = cardData;
                response.WellTestData = wellTest;

                var pumpingUnitManufacturer = string.Empty;
                var pumpingUnitAPIDesignation = string.Empty;

                if (wellDetail.PumpingUnitId != null)
                {
                    if (HasCustomPumpingUnit(wellDetail.PumpingUnitId))
                    {
                        var customPumpingUnit = GetCustomPumpingUnits(wellDetail.PumpingUnitId);
                        if (customPumpingUnit != null)
                        {
                            pumpingUnitManufacturer = customPumpingUnit.Manufacturer;
                            pumpingUnitAPIDesignation = customPumpingUnit.APIDesignation;
                        }
                    }
                    else
                    {
                        var result = context.PumpingUnits.AsNoTracking()
                            .GroupJoin(context.PumpingUnitManufacturer.AsNoTracking(),
                                pu => pu.ManufacturerId, pum => pum.ManufacturerAbbreviation,
                                (pumpingUnit, pumpingUnitManufacturer) => new
                                {
                                    PumpingUnit = pumpingUnit,
                                    PumpingUnitManufacturer = pumpingUnitManufacturer,
                                })
                            .SelectMany(x => x.PumpingUnitManufacturer.DefaultIfEmpty(),
                                (x, pumpingUnitManufacturer) => new
                                {
                                    x.PumpingUnit,
                                    PumpingUnitManufacturer = pumpingUnitManufacturer,
                                })
                            .FirstOrDefault(x => x.PumpingUnit.UnitId == wellDetail.PumpingUnitId);

                        if (result?.PumpingUnit != null)
                        {
                            pumpingUnitManufacturer = result.PumpingUnitManufacturer?.ManufacturerAbbreviation;
                            pumpingUnitAPIDesignation = result.PumpingUnit.APIDesignation;
                        }
                    }
                }

                response.PumpingUnitManufacturer = pumpingUnitManufacturer;
                response.PumpingUnitAPIDesignation = pumpingUnitAPIDesignation;

                response.XDiagResults = GetXDiagResultData(assetId, cardDate);
                response.CurrentRawScanData = GetCurrentRawScanData(assetId);
                var upliftOppMinimumProductionThresholdValue
                    = GetSystemParameterData(UPLIFT_OPP_MINIMUM_PRODUCTION_THRESHOLD);
                response.SystemParameters =
                    string.IsNullOrEmpty(upliftOppMinimumProductionThresholdValue)
                        ? "3"
                        : upliftOppMinimumProductionThresholdValue;

                response.CardType = cardData != null ? cardData.CardType : string.Empty;
                response.CauseId = cardData?.CauseId;
                response.PocType = node.PocType;

                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisSQLStore)} {nameof(GetRodLiftAnalysisData)}", correlationId);

                return response;
            }
        }

        /// <summary>
        /// Get the card dates by asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{CardDateModel}"/>.</returns>
        public IList<CardDateModel> GetCardDatesByAssetId(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisSQLStore)} {nameof(GetCardDatesByAssetId)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                List<CardDateModel> result = context.NodeMasters.AsNoTracking()
                    .Join(context.CardData.AsNoTracking(),
                        nm => nm.NodeId, cd => cd.NodeId, (nm, cd) => new
                        {
                            NodeMaster = nm,
                            CardDate = cd
                        })
                    .Where(x => x.NodeMaster.AssetGuid == assetId)
                    .Select(x => new CardDateModel()
                    {
                        Date = x.CardDate.CardDate,
                        CauseId = x.CardDate.CauseId,
                        CardTypeId = x.CardDate.CardType,
                        PocType = x.NodeMaster.PocType
                    })
                    .OrderByDescending(x => x.Date)
                    .ToList();

                foreach (var card in result)
                {
                    card.Date = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(card.Date, correlationId, LoggingModel.SQLStore);
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisSQLStore)} {nameof(GetCardDatesByAssetId)}", correlationId);

                return result;
            }
        }

        #endregion

        #region Private Methods

        private bool HasCustomPumpingUnit(string pumpingUnitId)
        {
            if (string.IsNullOrWhiteSpace(pumpingUnitId))
            {
                return false;
            }

            return pumpingUnitId.IndexOf("~X") == 0;
        }

        #endregion

    }
}