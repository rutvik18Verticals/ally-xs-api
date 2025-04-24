using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Common.Calculators;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Utilities;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using MongoCardCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Card;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// The service to retrieve card coordinates.
    /// </summary>
    public class CardCoordinateMongoStore : MongoOperations, ICardCoordinate
    {
        #region Private Fields
        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new <seealso cref="CardCoordinateMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public CardCoordinateMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        #endregion

        #region ICardCoordinate Implementation
        /// <summary>
        /// Get the Card Coordinate data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset id/node id.</param>
        /// <param name="cardDateString">The card date.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns></returns>
        public CardCoordinateModel GetCardCoordinateData(Guid assetId, string cardDateString, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(CardCoordinateMongoStore)} {nameof(GetCardCoordinateData)}", correlationId);

            var assetFilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(Constants.MongoDBCollection.ASSET_COLLECTION, assetFilter, correlationId);

            if (assetData == null)
            {
                logger.WriteCId(Level.Info, "Missing node", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(CardCoordinateMongoStore)} {nameof(GetCardCoordinateData)}", correlationId);

                return null;
            }

            // Get Asset Object
            var assetObj = assetData.FirstOrDefault();

            // Get PocType
            var pocType = GetPocType(assetObj, logger, correlationId);

            var cardDate = DateTime.Parse(cardDateString);
            var cardCoordinate = new CardCoordinateModel();

            // card mongo db filter
            var cardFilter = new FilterDefinitionBuilder<MongoCardCollection.Card>()
               .Where(x => (x.AssetId != null && x.AssetId == assetObj.Id) &&
                            x.Date.Day == cardDate.Day &&
                            x.Date.Month == cardDate.Month &&
                            x.Date.Year == cardDate.Year &&
                            x.Date.Hour == cardDate.Hour &&
                            x.Date.Minute == cardDate.Minute &&
                            x.Date.Second == cardDate.Second
                            );

            MongoCardCollection.Card cardData = null;
            var cardDataList = Find<MongoCardCollection.Card>(Constants.MongoDBCollection.CARD_COLLECTION, cardFilter, correlationId);
            if(cardDataList != null)
            {
                cardData = cardDataList.FirstOrDefault();
            }

            if (cardData != null)
            {
                cardCoordinate = new CardCoordinateModel()
                {
                    Area = cardData.Area,
                    AreaLimit = cardData.AreaLimit,
                    DownHoleCardBinary = GetCoordinatesDataBytes(cardData.DownholeCardPoints, logger, correlationId),
                    Fillage = (float?)cardData.Fillage,
                    FillBasePercent = cardData.FillBasePct,
                    HiLoadLimit = cardData.HiLoadLimit,
                    LoadLimit = cardData.LoadLimit,
                    LoadLimit2 = (short?)cardData.LoadLimit2,
                    LowLoadLimit = cardData.LoLoadLimit,
                    MalfunctionLoadLimit = cardData.MalfuncationLoadLimit,
                    MalfunctionPositionLimit = cardData.MalfuncationPositionLimit,
                    PermissibleLoadDownBinary = GetCoordinatesDataBytes(cardData.PermissibleLoadDownPoints, logger, correlationId),
                    PermissibleLoadUpBinary = GetCoordinatesDataBytes(cardData.PermissibleLoadUpPoints, logger, correlationId),
                    POCDownholeCard = cardData.POCDownholeCard,
                    PocDownHoleCardBinary = GetCoordinatesDataBytes(cardData.POCDownholeCardPoints, logger, correlationId),
                    PositionLimit = cardData.PositionLimit,
                    PositionLimit2 = (short?)cardData.PositionLimit2,
                    PredictedCard = cardData.PredictedCard,
                    PredictedCardBinary = GetCoordinatesDataBytes(cardData.PredictedCardPoints, logger, correlationId),
                    SecondaryPumpFillage = (float?)cardData.SecondaryPumpFillage,
                    StrokesPerMinute = (float?)cardData.StrokesPerMinute,
                    StrokeLength = cardData.StrokeLength,
                    SurfaceCardBinary =  GetCoordinatesDataBytes(cardData.SurfaceCardPoints, logger, correlationId),
                    PocType = pocType
                };
            }
            else
            {
                logger.WriteCId(Level.Info, "Missing card data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(CardCoordinateMongoStore)} {nameof(GetCardCoordinateData)}", correlationId);

                return null;
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(CardCoordinateMongoStore)} {nameof(GetCardCoordinateData)}", correlationId);

            return cardCoordinate;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Get Poc type from asset item
        /// </summary>
        /// <param name="assetObj"></param>
        /// <param name="logger"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        private short GetPocType(MongoAssetCollection.Asset assetObj, IThetaLogger logger, string correlationId)
        {
            try
            {
                POCTypes obj = (POCTypes)assetObj.POCType.LookupDocument;
                if (obj != null)
                {
                    return (short)obj.POCType;
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred while getting PocType.", ex, correlationId);
            }

            return 0;
        }

        /// <summary>
        /// Convert float coordinates data to bytes
        /// </summary>
        /// <param name="coordinatesData"></param>
        /// <param name="logger"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        private byte[] GetCoordinatesDataBytes(IList<CoordinatesDataModel<float>> coordinatesData, IThetaLogger logger, string correlationId)
        {
            try
            {
                var load = coordinatesData.Select(pos => pos.Y).ToList();
                var position = coordinatesData.Select(pos => pos.X).ToList();

                var index = 0;
                IList<byte> coordinatesDataBytes = new List<byte>();
                coordinatesDataBytes = BitFunctions.FloatListToByteList(load, ref coordinatesDataBytes, ref index);
                coordinatesDataBytes = BitFunctions.FloatListToByteList(position, ref coordinatesDataBytes, ref index);

                return coordinatesDataBytes.ToArray();
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred while converting coordinates data to bytes.", ex, correlationId);
            }

            // in case exception retun empty byte array.
            return Array.Empty<byte>();
        }

        #endregion

    }
}
