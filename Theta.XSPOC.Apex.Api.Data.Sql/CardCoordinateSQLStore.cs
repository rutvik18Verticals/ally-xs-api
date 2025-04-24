using Microsoft.EntityFrameworkCore;
using System;
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
    /// The service to retrieve card coordinates.
    /// </summary>
    public class CardCoordinateSQLStore : SQLStoreBase, ICardCoordinate
    {

        #region Private Member

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="CardCoordinateSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// <paramref name="loggerFactory"/> is null.
        /// </exception>
        public CardCoordinateSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        // #region ICardCoordinate Implementation

        /// <summary>
        /// Get the Card Coordinate data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset id/node id.</param>
        /// <param name="cardDateString">The card date.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>The <seealso cref="CardCoordinateModel"/>.</returns>
        public CardCoordinateModel GetCardCoordinateData(Guid assetId, string cardDateString, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(CardCoordinateSQLStore)} {nameof(GetCardCoordinateData)}", correlationId);

            var cardDate = DateTime.Parse(cardDateString);
            var cardCoordinate = new CardCoordinateModel();

            using (var context = _contextFactory.GetContext())
            {
                var cardData = context.CardData.AsNoTracking()
                    .Join(context.NodeMasters, l => l.NodeId, r => r.NodeId,
                        (carddata, nodemaster) => new
                        {
                            carddata,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId &&
                        x.carddata.CardDate.Day == cardDate.Day &&
                        x.carddata.CardDate.Month == cardDate.Month &&
                        x.carddata.CardDate.Year == cardDate.Year &&
                        x.carddata.CardDate.Hour == cardDate.Hour &&
                        x.carddata.CardDate.Minute == cardDate.Minute &&
                        x.carddata.CardDate.Second == cardDate.Second)
                    .Select(x => x.carddata).FirstOrDefault();
                var node = context.NodeMasters.AsNoTracking().FirstOrDefault(x => x.AssetGuid == assetId);

                if (cardData != null)
                {
                    cardCoordinate = new CardCoordinateModel()
                    {
                        Area = cardData.Area,
                        AreaLimit = cardData.AreaLimit,
                        DownHoleCardBinary = cardData.DownHoleCardBinary,
                        Fillage = cardData.Fillage,
                        FillBasePercent = cardData.FillBasePercent,
                        HiLoadLimit = cardData.HiLoadLimit,
                        LoadLimit = cardData.LoadLimit,
                        LoadLimit2 = cardData.LoadLimit2,
                        LowLoadLimit = cardData.LowLoadLimit,
                        MalfunctionLoadLimit = cardData.MalfunctionLoadLimit,
                        MalfunctionPositionLimit = cardData.MalfunctionPositionLimit,
                        PermissibleLoadDownBinary = cardData.PermissibleLoadDownBinary,
                        PermissibleLoadUpBinary = cardData.PermissibleLoadUpBinary,
                        POCDownholeCard = cardData.POCDownholeCard,
                        PocDownHoleCardBinary = cardData.PocDownHoleCardBinary,
                        PositionLimit = cardData.PositionLimit,
                        PositionLimit2 = cardData.PositionLimit2,
                        PredictedCard = cardData.PredictedCard,
                        PredictedCardBinary = cardData.PredictedCardBinary,
                        SecondaryPumpFillage = cardData.SecondaryPumpFillage,
                        StrokesPerMinute = cardData.StrokesPerMinute,
                        StrokeLength = cardData.StrokeLength,
                        SurfaceCardBinary = cardData.SurfaceCardBinary,
                        PocType = node.PocType
                    };
                }
                else
                {
                    logger.WriteCId(Level.Info, "Missing card data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(CardCoordinateSQLStore)} {nameof(GetCardCoordinateData)}", correlationId);

                    return null;
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(CardCoordinateSQLStore)} {nameof(GetCardCoordinateData)}", correlationId);

                return cardCoordinate;
            }
        }

    }
}
