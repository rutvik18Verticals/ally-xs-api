using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class CardCoordinateTest
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullContextFactoryTest()
        {
            _ = new CardCoordinateSQLStore(null, null);
        }

        [TestMethod]
        public void CardCoordinateDataTest()
        {
            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "well1",
                    PocType = 8,
                    AssetGuid = new Guid("f3eb743c-f890-44f3-80e5-6a46df7ce2b7"),
                }
            }.AsQueryable();
            var cardData = GetCardData().AsQueryable();
            var correlationId = Guid.NewGuid().ToString();

            var mockContext = SetupMockContext();

            var mockNodeMasterDbSet = SetupNodeMaster(nodeData);
            var mockCardDataDbSet = SetupCardData(cardData);

            var mockContextFactory = SetupMockContext();
            mockContextFactory.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContextFactory.Setup(x => x.CardData).Returns(mockCardDataDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContextFactory.Object);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var cardCoordinate = new CardCoordinateSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response =
                cardCoordinate.GetCardCoordinateData(new Guid("f3eb743c-f890-44f3-80e5-6a46df7ce2b7"), "2022-11-09 11:48:13.000", "correlationId");
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CardCoordinateNullCardDataTest()
        {
            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();
            var correlationId = Guid.NewGuid().ToString();

            var cardData = GetCardData().AsQueryable();

            var mockContext = SetupMockContext();

            var mockNodeMasterDbSet = SetupNodeMaster(nodeData);
            var mockCardDataDbSet = SetupCardData(cardData);

            var mockContextFactory = SetupMockContext();
            mockContextFactory.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContextFactory.Setup(x => x.CardData).Returns(mockCardDataDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContextFactory.Object);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var cardCoordinate = new CardCoordinateSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response =
                cardCoordinate.GetCardCoordinateData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2022-11-09 11:48:13.000", "correlationId");
            Assert.IsNull(response);
        }

        #endregion

        #region Private Methods

        private IList<CardDataEntity> GetCardData()
        {
            var cardDates = new List<CardDataEntity>()
            {
                new CardDataEntity()
                {
                    AnalysisDate = new DateTime(),
                    Area = 1,
                    AreaLimit = 70,
                    CardArea = 1,
                    CardType = "P",
                    CauseId = 99,
                    CorrectedCard = "1",
                    CardDate = new DateTime(2022, 11, 9, 11, 48, 13),
                    DownHoleCard = "1",
                    DownHoleCardBinary = null,
                    ElectrogramCardBinary = null,
                    Fillage = 1,
                    FillBasePercent = 1,
                    HiLoadLimit = 1,
                    LoadLimit = 1,
                    LoadLimit2 = 1,
                    LoadSpanLimit = 1,
                    LowLoadLimit = 1,
                    MalfunctionLoadLimit = 1,
                    MalfunctionPositionLimit = 1,
                    NodeId = "well1",
                    PermissibleLoadDownBinary = null,
                    PermissibleLoadUpBinary = null,
                    PocDHCard = null,
                    POCDownholeCard = null,
                    PocDownHoleCardBinary = null,
                    PositionLimit = 1,
                    PositionLimit2 = 1,
                    PredictedCard = null,
                    PredictedCardBinary = null,
                    ProcessCard = 1,
                    Runtime = 24,
                    Saved = true,
                    SecondaryPumpFillage = 40,
                    StrokesPerMinute = 8,
                    StrokeLength = 35,
                    SurfaceCard = "1",
                    SurfaceCardBinary = null,
                    TorquePlotCurrent = "1",
                    TorquePlotCurrentBinary = null,
                    TorquePlotMinimumEnergy = "1",
                    TorquePlotMinimumEnergyBinary = null,
                    TorquePlotMinimumTorque = "1",
                    TorquePlotMinimumTorqueBinary = null,
                }
            };

            return cardDates;
        }

        //private CardData GetCardData()
        //{

        //      var cardData=  new CardData()
        //        {
        //            StrokesPerMinute = 8,
        //            StrokeLength = 35,
        //            SecondaryPumpFillage = 40,
        //            AreaLimit = 0,
        //            Area = 3800,
        //            //DownHoleCardBinary = cardData.DownHoleCardBinary,
        //            Fillage = 96,
        //            FillBasePercent =45,
        //            HiLoadLimit = 50000,
        //            LoadLimit =456 ,
        //            LoadLimit2 = 0,
        //            LowLoadLimit = 0,
        //            MalfunctionLoadLimit = 0,
        //            MalfunctionPositionLimit = 28,
        //            //PermissibleLoadDownBinary = cardData.PermissibleLoadDownBinary,
        //            //PermissibleLoadUpBinary = cardData.PermissibleLoadUpBinary,
        //            //POCDownholeCard = cardData.POCDownholeCard,
        //            //PocDownHoleCardBinary = cardData.PocDownHoleCardBinary,
        //            PositionLimit = 55,
        //            PositionLimit2 = 0,
        //            //PredictedCard = cardData.PredictedCard,
        //            //PredictedCardBinary = cardData.PredictedCardBinary,
        //            //SurfaceCardBinary = cardData.SurfaceCardBinary,
        //            NodeId = "well1"
        //    };

        //    return cardData;

        //}

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<NodeMasterEntity>> SetupNodeMaster(IQueryable<NodeMasterEntity> data)
        {
            var mockDbSet = new Mock<DbSet<NodeMasterEntity>>();
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<CardDataEntity>> SetupCardData(IQueryable<CardDataEntity> data)
        {
            var mockDbSet = new Mock<DbSet<CardDataEntity>>();
            mockDbSet.As<IQueryable<CardDataEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<CardDataEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<CardDataEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<CardDataEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<NoLockXspocDbContext> SetupMockContext()
        {
            var contextOptions = new Mock<DbContextOptions<NoLockXspocDbContext>>();
            contextOptions.Setup(m => m.ContextType).Returns(typeof(NoLockXspocDbContext));
            contextOptions.Setup(m => m.Extensions).Returns(new List<IDbContextOptionsExtension>());

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();
            var mockInterceptor = new Mock<IDbConnectionInterceptor>();
            var mockContext = new Mock<NoLockXspocDbContext>(contextOptions.Object, mockInterceptor.Object, mockDateTimeConverter.Object);

            return mockContext;
        }

        #endregion

    }
}
