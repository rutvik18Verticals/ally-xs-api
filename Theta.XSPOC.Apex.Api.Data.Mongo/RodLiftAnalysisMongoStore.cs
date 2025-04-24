using MongoDB.Driver;
using System;
using System.Linq;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.RodPump;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Ports;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using MongoAsset = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using LiftTypeEnum = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;
using WellTest = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis.WellTest;
using PumpingUnitManufacturer = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup.PumpingUnitManufacturer;
using MongoDB.Bson;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Microsoft.Extensions.Configuration;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Implementation of mongo operations related to node master data.
    /// </summary>
    public class RodLiftAnalysisMongoStore : MongoOperations, IRodLiftAnalysis
    {
        #region Private Constants

        private const string COLLECTION_NAME_ASSET = "Asset";
        private const string COLLECTION_NAME_CARD = "Card";
        private const string COLLECTION_NAME_WELLTEST = "WellTest";
        private const string COLLECTION_NAME_PORT = "Port";
        private const string COLLECTION_NAME_CUSTOMERS = "Customers";
        private const string COLLECTION_NAME_LOOKUP = "Lookup";
        private const string COLLECTION_NAME_ANALYSIS = "Analysis";
        private const string COLLECTION_NAME_SYSTEMPARAMETERS = "SystemParameter";
        private const string COLLECTION_NAME_PARAMETERS = "MasterVariables";
        private const string UPLIFT_OPP_MINIMUM_PRODUCTION_THRESHOLD = "UpliftOppMinimumProductionThreshold";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IGetDataHistoryItemsService _dataHistoryInfluxStore;
        private readonly IDateTimeConverter _dateTimeConverter;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="RodLiftAnalysisMongoStore"/> using the provided 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <param name="dataHistoryInfluxStore">Influx datastore service</param>
        /// <param name="dateTimeConverter">Date time converter from Kernel</param>
        public RodLiftAnalysisMongoStore(IConfiguration configuration, IMongoDatabase database, IThetaLoggerFactory loggerFactory, 
            IGetDataHistoryItemsService dataHistoryInfluxStore, IDateTimeConverter dateTimeConverter)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _dataHistoryInfluxStore = dataHistoryInfluxStore ?? throw new ArgumentNullException(nameof(dataHistoryInfluxStore));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)} {nameof(GetRodLiftAnalysisData)}", correlationId);

            var cardDate = DateTime.Parse(cardDateString);

            DateTime cardDateUTC = _dateTimeConverter.ConvertFromApplicationServerTimeToUTC(cardDate, correlationId, LoggingModel.MongoDataStore);

            RodLiftAnalysisResponse response = new RodLiftAnalysisResponse();

            MongoAsset.Asset asset = GetAsset(assetId, correlationId);

            WellDetailsModel wellDetail = MapWellDetailsModelFromAsset(asset);
            if (wellDetail == null)
            {
                logger.WriteCId(Level.Info, "Missing well details", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)} {nameof(GetRodLiftAnalysisData)}", correlationId);
                return null;
            }
            response.WellDetails = wellDetail;

            string mongoAssetId = asset.Id;
            string nodeId = asset.LegacyId["NodeId"];
            
            Card card = GetCard(mongoAssetId, cardDateUTC, correlationId);
            CardDataModel cardData = MapCardDataModelFromCard(card, nodeId, correlationId);
            response.CardData = cardData;

            WellTest wellTest = GetWellTest(mongoAssetId, cardDateUTC, correlationId);
            WellTestModel wellTestData = MapWellTestModelFromWellTest(wellTest, nodeId, correlationId);
            response.WellTestData = wellTestData;

            Ports ports = GetPort(asset.AssetConfig.PortId, correlationId);
            short portId = (short)ports.PortID;

            Customer customer = GetCustomer(asset.CustomerId, correlationId);
            Guid customerGuid = Guid.TryParse(customer?.LegacyId["CustomerGUID"], out var parsedGuid) ? parsedGuid : Guid.Empty;

            NodeMasterModel node = MapNodeMasterModelFromAsset(asset, portId, customerGuid);
            response.NodeMasterData = node;

            var pumpingUnitManufacturer = string.Empty;
            var pumpingUnitAPIDesignation = string.Empty;

            if (wellDetail.PumpingUnitId != null)
            {
                if (HasCustomPumpingUnit(wellDetail.PumpingUnitId))
                {
                    Lookup puCustomLookup = GetPuCustomLookup(wellDetail.PumpingUnitId, correlationId);
                    if (puCustomLookup?.LookupDocument is PUCustom puCustom)
                    {
                        pumpingUnitManufacturer = puCustom.Manufacturer;
                        pumpingUnitAPIDesignation = puCustom.APIDesignation;
                    }
                }
                else
                {
                    Lookup pumpingUnits = GetPumpingUnitsLookup(wellDetail.PumpingUnitId, correlationId);
                    if (pumpingUnits?.LookupDocument is PumpingUnit pumpingUnit)
                    {
                        pumpingUnitManufacturer = pumpingUnit.ManufId;
                        pumpingUnitAPIDesignation = pumpingUnit.APIDesignation;
                    }
                }
            }
            response.PumpingUnitManufacturer = pumpingUnitManufacturer;
            response.PumpingUnitAPIDesignation = pumpingUnitAPIDesignation;

            Analysis xDiagAnalysis = GetXDiagResult(mongoAssetId, cardDateUTC, correlationId);
            response.XDiagResults = MapXDiagResultsModelFromAnalysis(xDiagAnalysis, nodeId, correlationId);

            List<CurrentRawScanDataInfluxModel> currentRawScanDataInflux = (List<CurrentRawScanDataInfluxModel>)_dataHistoryInfluxStore.GetCurrentRawScanData(assetId).Result;
            if (currentRawScanDataInflux != null)
            {
                var parameterKeys = currentRawScanDataInflux.Select(item => (POCType: int.Parse(item.POCType), item.ChannelId)).Distinct().ToList();
                var parameters = GetParametersBulk(parameterKeys, correlationId);
                response.CurrentRawScanData = MapCurrentRawScanData(currentRawScanDataInflux, nodeId, parameters);
            }

            SystemParameter systemParamter = GetSystemParameter(UPLIFT_OPP_MINIMUM_PRODUCTION_THRESHOLD, correlationId);
            string upliftOppMinimumProductionThresholdValue = string.IsNullOrWhiteSpace(systemParamter?.Value) ? string.Empty : systemParamter?.Value;
            response.SystemParameters = string.IsNullOrEmpty(upliftOppMinimumProductionThresholdValue) ? "3" : upliftOppMinimumProductionThresholdValue;

            response.CardType = cardData != null ? cardData.CardType : string.Empty;
            response.CauseId = cardData?.CauseId;
            response.PocType = node.PocType;

            logger.WriteCId(Level.Trace, $"Finished {nameof(GetRodLiftAnalysisData)} {nameof(GetRodLiftAnalysisData)}", correlationId);

            return response;

        }

        /// <summary>
        /// Get the card dates by asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{CardDateModel}"/>.</returns>
        public IList<CardDateModel> GetCardDatesByAssetId(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)} {nameof(GetCardDatesByAssetId)}", correlationId);

            List<CardDateModel> result = new List<CardDateModel>();

            // Fetch the asset using the assetId Guid
            MongoAsset.Asset asset = GetAsset(assetId, correlationId);
            if (asset == null)
            {
                logger.WriteCId(Level.Warn, "Asset not found for the given ID.", correlationId);
                return result;
            }
            string mongoAssetId = asset.Id;

            // Fetch all cards using Mongo AssetId 
            List<Card> cards = (List<Card>)GetCardsList(mongoAssetId, correlationId);
            if (cards == null || !cards.Any())
            {
                logger.WriteCId(Level.Warn, "No cards found for the given asset ID.", correlationId);
                return result;
            }

            result = cards
                .Select(card => new CardDateModel()
                    {
                        Date = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(card.Date, correlationId, LoggingModel.MongoDataStore),
                        CauseId = card.CauseID,
                        CardTypeId = card.CardType,
                        PocType = (short)(asset.POCType.LookupDocument as POCTypes).POCType
                })
                .OrderByDescending(x => x.Date)
                .ToList();

            return result;
        }

        #endregion

        #region Private mongo fetch methods

        private MongoAsset.Asset GetAsset(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetAsset)}", correlationId);

            try
            {
                var assetGuidString = assetId.ToString();

                var filter = Builders<MongoAsset.Asset>.Filter.And(
                    Builders<MongoAsset.Asset>.Filter.Exists($"LegacyId.AssetGUID"),
                    Builders<MongoAsset.Asset>.Filter.Ne(x => x.LegacyId["AssetGUID"], null),
                    Builders<MongoAsset.Asset>.Filter.Regex("LegacyId.AssetGUID", new BsonRegularExpression($"^{assetGuidString}$", "i"))
                );

                var assetData = Find<MongoAsset.Asset>(COLLECTION_NAME_ASSET, filter, correlationId);

                if (assetData == null || !assetData.Any())
                {
                    logger.WriteCId(Level.Warn, $"Asset not found for AssetGUID: {assetId}", correlationId);
                    return null;
                }

                var asset = assetData.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetAsset)}", correlationId);
                return asset;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetAsset)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private Card GetCard(string mongoAssetId, DateTime cardDate, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCard)}", correlationId);

            if (string.IsNullOrEmpty(mongoAssetId))
            {
                logger.WriteCId(Level.Warn, $"Invalid Asset ID provided: {mongoAssetId}", correlationId);
                return null;
            }

            try
            {
                var filter = Builders<Card>.Filter.And(
                    Builders<Card>.Filter.Eq(x => x.AssetId, mongoAssetId),
                    Builders<Card>.Filter.Eq(x => x.Date, cardDate)
                );

                var cardDocs = Find<Card>(COLLECTION_NAME_CARD, filter, correlationId);

                if (cardDocs == null || !cardDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"Card not found for AssetId: {mongoAssetId} and Date: {cardDate}", correlationId);
                    return null;
                }

                var card = cardDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCard)}", correlationId);

                return card;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCard)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private IList<Card> GetCardsList(string mongoAssetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCardsList)}", correlationId);

            try
            {
                var filter = Builders<Card>.Filter.Eq(x => x.AssetId, mongoAssetId);
                var cardDocs = Find<Card>(COLLECTION_NAME_CARD, filter, correlationId);

                if (cardDocs == null || !cardDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"No cards found for AssetId: {mongoAssetId}", correlationId);
                    return new List<Card>();
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCardsList)}", correlationId);
                return cardDocs.ToList();
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCardsList)}: {ex.Message}", correlationId);
                return new List<Card>();
            }
        }

        private WellTest GetWellTest(string mongoAssetId, DateTime cardDate, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetWellTest)}", correlationId);

            if (string.IsNullOrEmpty(mongoAssetId))
            {
                logger.WriteCId(Level.Warn, $"Invalid Asset ID provided: {mongoAssetId}", correlationId);
                return null;
            }

            try
            {
                var filter = Builders<WellTest>.Filter.And(
                    Builders<WellTest>.Filter.Eq(x => x.AssetId, mongoAssetId),
                    Builders<WellTest>.Filter.Lte(x => x.TestDate, cardDate),
                    Builders<WellTest>.Filter.Eq(x => x.Approved, true)
                );

                var wellTestDocs = Find<WellTest>(COLLECTION_NAME_WELLTEST, filter, correlationId);

                if (wellTestDocs == null || !wellTestDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"WellTest not found for AssetId: {mongoAssetId} and Date: {cardDate}", correlationId);
                    return null;
                }

                var wellTest = wellTestDocs.OrderByDescending(x => x.TestDate).FirstOrDefault();

                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetWellTest)}", correlationId);

                return wellTest;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetWellTest)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private Ports GetPort(string mongoPortId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPort)}", correlationId);

            if (string.IsNullOrEmpty(mongoPortId))
            {
                logger.WriteCId(Level.Warn, $"Invalid Port ID provided: {mongoPortId}", correlationId);
                return null;
            }

            try
            {
                var filter = Builders<Ports>.Filter.Eq(x => x.Id, mongoPortId);
                var portDocs = Find<Ports>(COLLECTION_NAME_PORT, filter, correlationId);

                if (portDocs == null || !portDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"Port not found for ID: {mongoPortId}", correlationId);
                    return null;
                }

                var port = portDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPort)}", correlationId);

                return port;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPort)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private Customer GetCustomer(string mongoCustomerId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCustomer)}", correlationId);

            if (string.IsNullOrEmpty(mongoCustomerId))
            {
                logger.WriteCId(Level.Warn, $"Invalid Customer ID provided: {mongoCustomerId}", correlationId);
                return null;
            }

            try
            {
                var filter = Builders<Customer>.Filter.Eq(x => x.Id, mongoCustomerId);
                var customerDocs = Find<Customer>(COLLECTION_NAME_CUSTOMERS, filter, correlationId);

                if (customerDocs == null || !customerDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"Customer not found for ID: {mongoCustomerId}", correlationId);
                    return null;
                }

                var customer = customerDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCustomer)}", correlationId);

                return customer;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetCustomer)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private Lookup GetPuCustomLookup(string pumpingUnitId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPuCustomLookup)}", correlationId);

            if (string.IsNullOrEmpty(pumpingUnitId))
            {
                logger.WriteCId(Level.Warn, $"Invalid PumpingUnitId provided: {pumpingUnitId}", correlationId);
                return null;
            }

            try
            {
                var filter = Builders<Lookup>.Filter.And(
                    Builders<Lookup>.Filter.Eq(x => x.LookupType, LookupTypes.PUCustom.ToString()),
                    Builders<Lookup>.Filter.Eq(x => ((PUCustom)x.LookupDocument).UnitId, pumpingUnitId)
                );

                var puCustomDocs = Find<Lookup>(COLLECTION_NAME_LOOKUP, filter, correlationId);

                if (puCustomDocs == null || !puCustomDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"Custom Pumping Unit not found for ID: {pumpingUnitId}", correlationId);
                    return null;
                }

                var puCustom = puCustomDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPuCustomLookup)}", correlationId);

                return puCustom;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPuCustomLookup)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private Lookup GetPumpingUnitsLookup(string pumpingUnitId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPumpingUnitsLookup)}", correlationId);

            if (string.IsNullOrEmpty(pumpingUnitId))
            {
                logger.WriteCId(Level.Warn, $"Invalid PumpingUnitId provided: {pumpingUnitId}", correlationId);
                return null;
            }

            try
            {
                var filter = Builders<Lookup>.Filter.And(
                    Builders<Lookup>.Filter.Eq(x => x.LookupType, LookupTypes.PumpingUnit.ToString()),
                    Builders<Lookup>.Filter.Eq(x => ((PumpingUnit)x.LookupDocument).UnitId, pumpingUnitId)
                );

                var pumpingUnitDocs = Find<Lookup>(COLLECTION_NAME_LOOKUP, filter, correlationId);

                if (pumpingUnitDocs == null || !pumpingUnitDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"Pumping Unit not found for ID: {pumpingUnitId}", correlationId);
                    return null;
                }

                var pumpingUnit = pumpingUnitDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPumpingUnitsLookup)}", correlationId);

                return pumpingUnit;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPumpingUnitsLookup)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private Lookup GetPumpingUnitManufacturerLookup(string manufId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPumpingUnitManufacturerLookup)}", correlationId);

            if (string.IsNullOrEmpty(manufId))
            {
                logger.WriteCId(Level.Warn, $"Invalid ManufactureId provided: {manufId}", correlationId);
                return null;
            }

            try
            {
                var filter = Builders<Lookup>.Filter.And(
                    Builders<Lookup>.Filter.Eq(x => x.LookupType, LookupTypes.PumpingUnitManufacturer.ToString()),
                    Builders<Lookup>.Filter.Eq(x => ((PumpingUnitManufacturer)x.LookupDocument).Abbrev, manufId)
                );

                var puManufacturerDocs = Find<Lookup>(COLLECTION_NAME_LOOKUP, filter, correlationId);

                if (puManufacturerDocs == null || !puManufacturerDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"Pumping Unit Manufacturer not found for ID: {manufId}", correlationId);
                    return null;
                }

                var puManufacturer = puManufacturerDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPumpingUnitManufacturerLookup)}", correlationId);

                return puManufacturer;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetPumpingUnitManufacturerLookup)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private Analysis GetXDiagResult(string mongoAssetId, DateTime cardDate, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetXDiagResult)}", correlationId);

            try
            {
                var filter = Builders<Analysis>.Filter.And(
                    Builders<Analysis>.Filter.Eq(x => x.AssetId, mongoAssetId),
                    Builders<Analysis>.Filter.Eq(x => x.AnalysisType, AnalysisResultType.RodLiftAnalysisType.ToString()),
                    Builders<Analysis>.Filter.Eq(x => x.AnalysisDate, cardDate)
                );

                var analysisDocs = Find<Analysis>(COLLECTION_NAME_ANALYSIS, filter, correlationId);

                if (analysisDocs == null || !analysisDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"Analysis not found for AssetId: {mongoAssetId} and Date: {cardDate}", correlationId);
                    return null;
                }

                var analysis = analysisDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetXDiagResult)}", correlationId);

                return analysis;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetXDiagResult)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private SystemParameter GetSystemParameter(string paramName, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetSystemParameter)}", correlationId);

            try
            {
                var filter = Builders<SystemParameter>.Filter.Eq(x => x.Parameter, paramName);
                var systemParameterDocs = Find<SystemParameter>(COLLECTION_NAME_SYSTEMPARAMETERS, filter, correlationId);

                if (systemParameterDocs == null || !systemParameterDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"SystemParameter '{paramName}' not found.", correlationId);
                    return null;
                }

                var systemParameter = systemParameterDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetSystemParameter)}", correlationId);

                return systemParameter;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetSystemParameter)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private Parameters GetParameters(string pocType, string channelId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetParameters)}", correlationId);

            try
            {
                var filter = Builders<Parameters>.Filter.And(
                    Builders<Parameters>.Filter.Eq("POCType.LookupDocument.POCType", pocType),
                    Builders<Parameters>.Filter.Eq("ChannelId", channelId)
                );

                var parameterDocs = Find<Parameters>(COLLECTION_NAME_PARAMETERS, filter, correlationId);

                if (parameterDocs == null || !parameterDocs.Any())
                {
                    logger.WriteCId(Level.Warn, $"POCType :{pocType}, ChannelId: {channelId} not found.", correlationId);
                    return null;
                }

                var systemParameter = parameterDocs.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetParameters)}", correlationId);

                return systemParameter;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetParameters)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private IDictionary<(int POCType, string ChannelId), Parameters> GetParametersBulk(List<(int POCType, string ChannelId)> parameterKeys, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetParametersBulk)}", correlationId);

            try
            {
                // Build an Or filter for all POCType and ChannelId pairs
                var filters = parameterKeys.Select(key =>
                    Builders<Parameters>.Filter.And(
                        Builders<Parameters>.Filter.Eq("POCType.LookupDocument.POCType", key.POCType),
                        Builders<Parameters>.Filter.Eq("ChannelId", key.ChannelId)
                    )
                );
                var filter = Builders<Parameters>.Filter.Or(filters);

                var parameterDocs = Find<Parameters>(COLLECTION_NAME_PARAMETERS, filter, correlationId);

                if (parameterDocs == null || !parameterDocs.Any())
                {
                    logger.WriteCId(Level.Warn, "No parameters found for provided keys.", correlationId);
                    return new Dictionary<(int POCType, string ChannelId), Parameters>();
                }

                // Map results to a dictionary for quick lookup
                var parameterDict = parameterDocs
                    .GroupBy(param => (((POCTypes)param.POCType.LookupDocument).POCType, param.ChannelId))
                    .ToDictionary(
                        group => group.Key,
                        group => group.First()
                    );

                logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetParametersBulk)}", correlationId);
                return parameterDict;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(RodLiftAnalysisMongoStore)}.{nameof(GetParametersBulk)}: {ex.Message}", correlationId);
                return new Dictionary<(int POCType, string ChannelId), Parameters>();
            }
        }

        #endregion

        #region Private Mapping methods

        private WellDetailsModel MapWellDetailsModelFromAsset(MongoAsset.Asset asset)
        {
            if (asset?.AssetDetails is RodPumpDetail rodPumpDetail)
            {
                return new WellDetailsModel
                {
                    NodeId = asset.LegacyId.TryGetValue("NodeId", out var nodeId) ? nodeId : null,
                    Runtime = rodPumpDetail.Runtime.HasValue ? (float?)rodPumpDetail.Runtime : null,
                    PlungerDiameter = rodPumpDetail.PlungerDiameter.HasValue ? (float?)rodPumpDetail.PlungerDiameter : null,
                    PumpDepth = rodPumpDetail.PumpDepth,
                    Cycles = rodPumpDetail.Cycles.HasValue ? (float?)rodPumpDetail.Cycles : null,
                    IdleTime = rodPumpDetail.IdleTime.HasValue ? (float?)rodPumpDetail.IdleTime : null,
                    POCGrossRate = rodPumpDetail.POCGrossRate,
                    FluidLevel = rodPumpDetail.FluidLevel,
                    StrokesPerMinute = rodPumpDetail.StrokesPerMinute.HasValue ? (float?)rodPumpDetail.StrokesPerMinute : null,
                    PumpType = rodPumpDetail.PumpType,
                    StrokeLength = rodPumpDetail.StrokeLength.HasValue ? (float?)rodPumpDetail.StrokeLength : null,
                    PumpingUnitId = ((PumpingUnit)(rodPumpDetail.PumpingUnit?.LookupDocument))?.UnitId
                };
            }

            return null;
        }

        private CardDataModel MapCardDataModelFromCard(Card card, string nodeId, string correlationId)
        {
            if (card == null)
            {
                return null;
            }

            return new CardDataModel
            {
                NodeId = nodeId,
                StrokesPerMinute = card.StrokesPerMinute.HasValue ? (float?)card.StrokesPerMinute : null,
                CardDate = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(card.Date, correlationId, LoggingModel.MongoDataStore),
                Fillage = card.Fillage.HasValue ? (float?)card.Fillage : null,
                FillBasePercent = card.FillBasePct,
                SecondaryPumpFillage = card.SecondaryPumpFillage.HasValue ? (float?)card.SecondaryPumpFillage : null,
                AreaLimit = card.AreaLimit,
                Area = card.Area,
                LoadSpanLimit = card.LoadSpanLimit,
                CardArea = card.CardArea,
                CardType = card.CardType,
                CauseId = card.CauseID,
                Runtime = card.Runtime.HasValue ? (float?)card.Runtime : null,
                StrokeLength = card.StrokeLength
            };
        }

        private WellTestModel MapWellTestModelFromWellTest(WellTest wellTest, string nodeId, string correlationId)
        {
            if (wellTest == null)
            {
                return null;
            }

            return new WellTestModel
            {
                TestDate = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(wellTest.TestDate, correlationId, LoggingModel.MongoDataStore),
                OilRate = wellTest.OilRate.HasValue ? (float?)wellTest.OilRate : null,
                GasRate = wellTest.GasRate.HasValue ? (float?)wellTest.GasRate : null,
                WaterRate = wellTest.WaterRate.HasValue ? (float?)wellTest.WaterRate : null,
                NodeId = nodeId,
                Approved = wellTest.Approved
            };
        }

        private NodeMasterModel MapNodeMasterModelFromAsset(MongoAsset.Asset asset, short portId, Guid companyGuid)
        {
            if (asset == null)
            {
                return null;
            }

            return new NodeMasterModel
            {
                NodeId = asset.LegacyId.TryGetValue("NodeId", out var nodeId) ? nodeId : null,
                AssetGuid = (Guid)(asset.LegacyId.TryGetValue("AssetGUID", out var assetGUID) && Guid.TryParse(assetGUID, out var parsedGuid)
                    ? parsedGuid
                    : (Guid?)null),
                PocType = asset.POCType.LookupDocument is POCTypes pocType
                    ? (short)pocType.POCType
                    : (short)0,
                RunStatus = asset.AssetConfig?.RunStatus,
                TimeInState = asset.AssetConfig?.TimeInState,
                TodayCycles = asset.AssetConfig?.TodayCycles,
                TodayRuntime = (bool)(asset.AssetConfig?.TodayRuntime.HasValue) ? (float?)asset.AssetConfig.TodayRuntime : null,
                InferredProd = (bool)(asset.AssetConfig?.InferredProduction.HasValue) ? (float?)asset.AssetConfig.InferredProduction : null,
                Enabled = asset.AssetConfig?.IsEnabled ?? false,
                PortId = portId,
                ApplicationId = Enum.TryParse<LiftTypeEnum.Applications>(asset.ArtificialLiftType, out var applicationEnum)
                    ? (int)applicationEnum
                    : (int)LiftTypeEnum.Applications.None,
                CompanyGuid = companyGuid
            };
        }

        private XDiagResultsModel MapXDiagResultsModelFromAnalysis(Analysis xDiagAnalysis, string nodeId, string correlationId)
        {
            if (xDiagAnalysis == null)
            {
                return null;
            }

            if (xDiagAnalysis.Result is not XDiagResults xDiagAnalysisResult)
            {
                return null;
            }

            return new XDiagResultsModel
            {
                Date = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(xDiagAnalysis.AnalysisDate, correlationId, LoggingModel.MongoDataStore),
                GrossPumpStroke = (short?)xDiagAnalysisResult.GrossPumpStroke,
                FluidLoadonPump = xDiagAnalysisResult.FluidLoadonPump,
                BouyRodWeight = xDiagAnalysisResult.BouyRodWeight,
                DryRodWeight = xDiagAnalysisResult.DryRodWeight,
                PumpFriction = xDiagAnalysisResult.PumpFriction,
                PofluidLoad = xDiagAnalysisResult.POFluidLoad,
                PumpSize = (float?)xDiagAnalysisResult.PumpSize,
                DownholeCapacity24 = xDiagAnalysisResult.DownholeCapacity24,
                DownholeCapacityRuntime = xDiagAnalysisResult.DownholeCapacityRuntime,
                DownholeCapacityRuntimeFillage = xDiagAnalysisResult.DownholeCapacityRuntimeFillage,
                AdditionalUplift = (float?)xDiagAnalysisResult.AdditionalUplift,
                AdditionalUpliftGross = (float?)xDiagAnalysisResult?.AdditionalUpliftGross,
                PumpEfficiency = xDiagAnalysisResult?.PumpEfficiency,
                UpliftCalculationMissingRequirements = xDiagAnalysisResult.UpliftCalculationMissingRequirements.ToString(),
                MaximumSpm = (float?)xDiagAnalysisResult?.MaximumSPM,
                ProductionAtMaximumSpm = (float?)xDiagAnalysisResult?.ProductionAtMaximumSPM,
                OilProductionAtMaximumSpm = (float?)xDiagAnalysisResult!.OilProductionAtMaximumSPM,
                MaximumSpmoverloadMessage = xDiagAnalysisResult?.MaximumSPMOverloadMessage,
                DownholeAnalysis = xDiagAnalysisResult?.DownholeAnalysis?.ToString(),
                InputAnalysis = xDiagAnalysisResult?.InputAnalysis,
                RodAnalysis = xDiagAnalysisResult?.RodAnalysis,
                SurfaceAnalysis = xDiagAnalysisResult?.SurfaceAnalysis,
                NodeId = nodeId,
                PumpIntakePressure = xDiagAnalysisResult?.PumpIntakePressure,
                AddOilProduction = xDiagAnalysisResult?.AddOilProduction,
                AvgDHDSLoad = xDiagAnalysisResult?.AvgDHDSLoad,
                AvgDHDSPOLoad = xDiagAnalysisResult?.AvgDHDSPOLoad,
                AvgDHUSLoad = xDiagAnalysisResult?.AvgDHUSLoad,
                AvgDHUSPOLoad = xDiagAnalysisResult?.AvgDHUSPOLoad,
                CasingPressure = xDiagAnalysisResult?.CasingPressure,
                CurrentCBE = xDiagAnalysisResult?.CurrentCBE,
                CurrentCLF = (float?)xDiagAnalysisResult?.CurrentCLF,
                CurrentElecBG = (float?)xDiagAnalysisResult?.CurrentElecBG,
                CurrentElecBO = (float?)xDiagAnalysisResult?.CurrentElecBO,
                CurrentKWH = xDiagAnalysisResult?.CurrentKWH,
                CurrentMCB = (short)xDiagAnalysisResult?.CurrentMCB,
                CurrentMonthlyElec = xDiagAnalysisResult?.CurrentMonthlyElec,
                ElecCostMinTorquePerBO = (decimal?)(xDiagAnalysisResult.ElecCostMinTorquePerBO),
                ElecCostMonthly = (decimal?)(xDiagAnalysisResult?.ElecCostMonthly),
                ElecCostPerBG = (decimal?)(xDiagAnalysisResult?.ElecCostPerBG),
                ElecCostPerBO = (decimal?)(xDiagAnalysisResult?.ElecCostPerBO),
                ElectCostMinEnergyPerBO = (decimal?)(xDiagAnalysisResult?.ElecCostMinEnergyPerBO),
                FillagePct = (short?)(xDiagAnalysisResult?.FillagePct),
                FluidLevelXDiag = xDiagAnalysisResult?.FluidLevelXDiag,
                Friction = xDiagAnalysisResult?.Friction,
                GearBoxLoadPct = xDiagAnalysisResult?.GearBoxLoadPct,
                GearboxTorqueRating = xDiagAnalysisResult?.GearboxTorqueRating,
                GrossProd = xDiagAnalysisResult?.GrossProd,
                LoadShift = xDiagAnalysisResult?.LoadShift,
                MaxRodLoad = (short?)(xDiagAnalysisResult?.MaxRodLoad),
                MinElecCostPerBG = (decimal?)xDiagAnalysisResult.MinElecCostPerBG,
                MinEnerGBTorque = (int?)(xDiagAnalysisResult?.MinEnerGBTorque),
                MinEnergyCBE = xDiagAnalysisResult?.MinEnergyCBE,
                MinEnergyCLF = (float?)(xDiagAnalysisResult?.MinEnergyCLF),
                MinEnergyElecBG = (float?)(xDiagAnalysisResult?.MinEnergyElecBG),
                MinEnergyGBLoadPct = xDiagAnalysisResult?.MinEnergyGBLoadPct,
                MinEnergyMCB = (short?)(xDiagAnalysisResult?.MinEnergyMCB),
                MinEnergyMonthlyElec = xDiagAnalysisResult.MinEnergyMonthlyElec,
                MinGBTorque = xDiagAnalysisResult?.MinGBTorque,
                MinHP = xDiagAnalysisResult?.MinHP,
                MinMonthlyElecCost = (decimal?)(xDiagAnalysisResult?.MinMonthlyElecCost),
                MinTorqueCBE = (int?)(xDiagAnalysisResult?.MinTorqueCBE),
                MinTorqueCLF = (float?)(xDiagAnalysisResult?.MinTorqueCLF),
                MinTorqueElecBG = (float?)(xDiagAnalysisResult?.MinTorqueElecBG),
                MinTorqueElecBO = (float?)(xDiagAnalysisResult?.MinTorqueElecBO),
                MinTorqueGBLoadPct = xDiagAnalysisResult?.MinTorqueGBLoadPct,
                MinTorqueKWH = (int?)(xDiagAnalysisResult?.MinTorqueKWH),
                MinTorqueMCB = (short?)(xDiagAnalysisResult?.MinTorqueMCB),
                MinTorqueMonthlyElec = xDiagAnalysisResult?.MinTorqueMonthlyElec,
                MotorLoad = xDiagAnalysisResult?.MotorLoad,
                MPRL = xDiagAnalysisResult?.MPRL,
                NetProd = (short?)(xDiagAnalysisResult?.NetProd),
                NetStrokeApparent = (float?)(xDiagAnalysisResult?.NetStrokeApparent),
                OilAPIGravity = (float?)(xDiagAnalysisResult?.OilAPIGravity),
                PeakGBTorque = xDiagAnalysisResult?.PeakGBTorque,
                POFluidLoad = xDiagAnalysisResult?.POFluidLoad,
                PolRodHP = (float?)(xDiagAnalysisResult?.PolRodHP),
                PPRL = xDiagAnalysisResult?.PPRL,
                ProductionAtMaximumSPM = (float?)(xDiagAnalysisResult?.ProductionAtMaximumSPM),
                PumpEffPct = xDiagAnalysisResult?.PumpEffPct,
                SystemEffPct = xDiagAnalysisResult?.SystemEffPct,
                TubingMovement = xDiagAnalysisResult?.TubingMovement,
                TubingPressure = xDiagAnalysisResult?.TubingPressure,
                UnitStructLoad = xDiagAnalysisResult?.UnitStructLoad,
                WaterCutPct = xDiagAnalysisResult.WaterCutPct,
                WaterSG = (float?)(xDiagAnalysisResult?.WaterSG),
            };
        }

        private IList<CurrentRawScanDataModel> MapCurrentRawScanData(List<CurrentRawScanDataInfluxModel> crsdi, string nodeId,
            IDictionary<(int POCType, string ChannelId), Parameters> parameters)
        {
            IList<CurrentRawScanDataModel> currentRawScanData = new List<CurrentRawScanDataModel>();

            foreach (var item in crsdi)
            {
                var key = (POCType: int.Parse(item.POCType), item.ChannelId);
                Parameters parameter = parameters.TryGetValue(key, out var param) ? param : null;

                var crsm = new CurrentRawScanDataModel
                {
                    NodeId = nodeId,
                    Value = item?.Value,
                    DateTimeUpdated = item?.DateTimeUpdated,
                    Address = parameter != null ? parameter.Address : 0,
                    StringValue = string.Empty // Need to Check
                };

                currentRawScanData.Add(crsm);
            }

            return currentRawScanData;
        }

        #endregion

        #region Private Helper methods

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