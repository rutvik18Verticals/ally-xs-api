using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.Converters;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of IRodLiftAnalysisProcessingService.
    /// </summary>
    public class RodLiftAnalysisProcessingService : IRodLiftAnalysisProcessingService
    {

        #region Private Members

        private readonly IRodLiftAnalysis _rodLiftAnalysisService;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly ICommonService _commonService;
        private readonly ILocalePhrases _localePhrases;
        private readonly ICardCoordinate _cardCoordinateService;

        private const string NULL_TEXT = "-";

        private enum PhraseIDs
        {

            Runtime = 536, // Runtime
            SPM = 98, // SPM
            StrLength = 99, // Str. Length
            PmpDiam = 100, // Pmp Diam
            TestDate = 102, // Test Date
            TestGas = 103, // Test Gas
            TestOil = 104, // Test Oil
            TestGross = 105, // Test Gross
            SurCap24 = 106, // SurCap@24
            PumpingUnit = 107, // Pumping Unit
            Cycles = 109, // Cycles
            DHStroke = 115, // DH Stroke
            DHCap24 = 116, // DH Cap@24
            DHCapRT = 117, // DH Cap@RT
            DHCapRTFillage = 118, // DH Cap@RT, Fillage
            FluidLoad = 120, // Fluid Load
            BuoyantRodWeight = 121, // Buoyant Rod Weight
            DryRodWeight = 122, // Dry Rod Weight
            PumpFrictionLoad = 123, // Pump Friction Load
            POFluidLoad = 124, // PO Fluid Load 
            AnalysisData = 125, // Analysis Data
            InfPrdToday = 157, // Inf Prd, Today
            InfPrdYest = 158, // Inf Prd, Yest
            PumpDepth = 271, // Pump Depth
            TestWater = 1110, // Test Water
            PumpEfficiency = 1164, // Pump Efficiency
            FillageSetpoint = 1178, // Fillage Setpoint
            PumpFillage = 1980, // Pump Fillage
            SWTOilYesterday = 2091, // SWT Oil Yesterday
            SWTWaterYesterday = 2092, // SWT Water Yesterday
            SWTGasYesterday = 2093, // SWT Gas Yesterday
            IdleTime = 2732, // Idle Time
            Oiluplift = 6819, // An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.
            PeakNetTorque = 6848, // Peak Net Torque
            StructuralLoad =
                6849, // An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.
            XDIAGSPM = 7048, // XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.
            XDOAGPotentialuplift =
                7049, // XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.
            SPMLessthenOilperday = 7117, // Incremental production at {0} SPM is less than {1} bbls of oil per day.
            NA = 200, // N/A

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="RodLiftAnalysisProcessingService"/>.
        /// </summary>
        /// <param name="rodLiftAnalysisService">The <seealso cref="IRodLiftAnalysis"/> service.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="commonService">The <seealso cref="ICommonService"/> service.</param>
        /// <param name="localePhrases">The <seealso cref="ILocalePhrases"/> service.</param>
        /// <param name="cardCoordinateService">The <seealso cref="ICardCoordinate"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rodLiftAnalysisService"/> is null
        /// or
        /// <paramref name="loggerFactory"/> is null
        /// </exception>
        public RodLiftAnalysisProcessingService(IRodLiftAnalysis rodLiftAnalysisService, IThetaLoggerFactory loggerFactory,
            ICommonService commonService, ILocalePhrases localePhrases, ICardCoordinate cardCoordinateService)
        {
            _rodLiftAnalysisService = rodLiftAnalysisService ?? throw new ArgumentNullException(nameof(rodLiftAnalysisService));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _localePhrases = localePhrases ?? throw new ArgumentNullException(nameof(localePhrases));
            _cardCoordinateService = cardCoordinateService ?? throw new ArgumentNullException(nameof(cardCoordinateService));
        }

        #endregion

        #region IRodLiftAnalysisProcessingService Implementation

        /// <summary>
        /// Processes the provided rod lift analysis request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="WithCorrelationId{RodLiftAnalysisInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="RodLiftAnalysisResponse"/></returns>
        public async Task<RodLiftAnalysisDataOutput> GetRodLiftAnalysisResultsAsync(
            WithCorrelationId<RodLiftAnalysisInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.RodLiftAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisProcessingService)} {nameof(GetRodLiftAnalysisResultsAsync)}", data?.CorrelationId);

            RodLiftAnalysisDataOutput response = new RodLiftAnalysisDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (data == null)
            {
                var message = $"{nameof(data)} is null, cannot get rod lift analysis results.";
                logger.Write(Level.Info, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (data?.Value == null)
            {
                var message = $"{nameof(data)} is null, cannot get rod lift analysis results.";
                logger.WriteCId(Level.Info, message, data?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = data?.CorrelationId;
            var request = data.Value;

            if (string.IsNullOrEmpty(request.CardDate) ||
                request.AssetId == Guid.Empty)
            {
                var message = $"{nameof(request.CardDate)} and {nameof(request.AssetId)}" +
                    $" should be provided to get rod lift analysis results.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var rodLiftAnalysisData = _rodLiftAnalysisService.GetRodLiftAnalysisData(request.AssetId,
                request.CardDate, correlationId);

            if (rodLiftAnalysisData == null || rodLiftAnalysisData.NodeMasterData == null
                || rodLiftAnalysisData.CardData == null || rodLiftAnalysisData.WellDetails == null)
            {
                var message = (rodLiftAnalysisData?.NodeMasterData == null)
                    ? $"{nameof(rodLiftAnalysisData.NodeMasterData)} is null"
                    : (rodLiftAnalysisData?.CardData == null)
                        ? $"{nameof(rodLiftAnalysisData.CardData)} is null"
                        : $"{nameof(rodLiftAnalysisData)} is null";

                response.Result.Status = false;
                response.Result.Value = $"{message}, cannot get rod lift analysis results.";
                logger.WriteCId(Level.Info, $"{message}, cannot get rod lift analysis results.", correlationId);
            }
            else
            {
                var rodLiftAnalysisValues = await GetRodLiftAnalysisReponseData(rodLiftAnalysisData, correlationId);
                response.Values = rodLiftAnalysisValues;
                response.Result.Status = true;
                response.Result.Value = string.Empty;
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisProcessingService)} {nameof(GetRodLiftAnalysisResultsAsync)}", data?.CorrelationId);

            return response;
        }

        /// <summary>
        /// Processes the provided card date request and generates card date based on that data.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="CardDateInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="IList{CardDateOutput}"/>.</returns>
        public CardDatesOutput GetCardDate(WithCorrelationId<CardDateInput> requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.RodLiftAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisProcessingService)} {nameof(GetCardDate)}", requestWithCorrelationId?.CorrelationId);

            if (requestWithCorrelationId == null)
            {
                logger.WriteCId(Level.Info, $"{nameof(requestWithCorrelationId)} is null, cannot get card date.",
                    requestWithCorrelationId?.CorrelationId);

                return new CardDatesOutput()
                {
                    Result = new MethodResult<string>(
                        false, $"{nameof(requestWithCorrelationId)} is null, cannot get card date."),
                };
            }

            if (requestWithCorrelationId?.Value == null)
            {
                logger.WriteCId(Level.Info, $"{nameof(requestWithCorrelationId)} is null, cannot get card date.",
                    requestWithCorrelationId?.CorrelationId);

                return new CardDatesOutput()
                {
                    Result = new MethodResult<string>(
                        false, $"{nameof(requestWithCorrelationId)} is null, cannot get card date."),
                };
            }

            var correlationId = requestWithCorrelationId?.CorrelationId;
            var request = requestWithCorrelationId.Value;

            if (request.AssetId == Guid.Empty)
            {
                logger.WriteCId(Level.Info,
                    $"{nameof(request)} should be provided to get card date.",
                    correlationId);

                return new CardDatesOutput()
                {
                    Result = new MethodResult<string>(
                        false, $"{nameof(request)} should be provided to get card date."),
                };
            }

            var result = _rodLiftAnalysisService.GetCardDatesByAssetId(request.AssetId, correlationId);

            var cardDates = new List<CardDateItem>();

            if (result != null)
            {
                IDictionary<int, string> phrases = new Dictionary<int, string>();
                IDictionary<int, string> causeIdPhrases = new Dictionary<int, string>();

                foreach (var item in result)
                {
                    cardDates.Add(new CardDateItem()
                    {
                        CardTypeId = item.CardTypeId,
                        CardTypeName =
                            _commonService.GetCardTypeName(item.CardTypeId, item.CauseId, item.PocType, ref phrases,
                                ref causeIdPhrases, correlationId),
                        Date = item.Date,
                    });
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisProcessingService)} {nameof(GetCardDate)}", requestWithCorrelationId?.CorrelationId);

            return new CardDatesOutput()
            {
                Result = new MethodResult<string>(true, String.Empty),
                Values = cardDates,
            };
        }

        #endregion

        #region ICardCoordinateProcessingService Implementation

        /// <summary>
        /// Processes the provided Card Coordinate request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="WithCorrelationId{CardCoordinateInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="CardCoordinateDataOutput"/>.</returns>
        public CardCoordinateDataOutput GetCardCoordinateResults(WithCorrelationId<CardCoordinateInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.RodLiftAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodLiftAnalysisProcessingService)} " +
                $"{nameof(GetCardCoordinateResults)}", data?.CorrelationId);

            CardCoordinateDataOutput response = new CardCoordinateDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };
            if (data?.Value == null)
            {
                var message = $"Correlation Id is null, error retriving card coordinate results.";
                logger.WriteCId(Level.Info, message, data?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = data?.CorrelationId;
            var request = data.Value;

            if (string.IsNullOrEmpty(request.CardDate) ||
                request.AssetId == Guid.Empty)
            {
                var message = $"{nameof(request.CardDate)} and {nameof(request.AssetId)}" +
                    $" required to get card coordinate results.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var cardData = _cardCoordinateService.GetCardCoordinateData(request.AssetId, request.CardDate, correlationId);
            if (cardData == null)
            {
                response.Result.Status = false;
                response.Result.Value = "Card Coordinates  results is empty.";
            }
            else
            {
                response = CardCoordinateDataMapper.MapToDomainObject(cardData, _commonService, correlationId);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(RodLiftAnalysisProcessingService)} " +
                $"{nameof(GetCardCoordinateResults)}", data?.CorrelationId);

            return response;
        }

        #endregion

        #region Private Methods

        private async Task<RodLiftAnalysisValues> GetRodLiftAnalysisReponseData(RodLiftAnalysisResponse rodLiftAnalysisData, string correlationId)
        {
            await Task.Yield();

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            var cardData = rodLiftAnalysisData.CardData;
            var wellDetail = rodLiftAnalysisData.WellDetails;
            var wellTest = rodLiftAnalysisData.WellTestData;
            var node = rodLiftAnalysisData.NodeMasterData;
            var systemParamter = rodLiftAnalysisData.SystemParameters;
            var xdg = rodLiftAnalysisData.XDiagResults;

            RodLiftAnalysisValues values = new RodLiftAnalysisValues();

            var phraseid = Enum.GetValues<PhraseIDs>().Cast<int>().ToArray();
            var phrases = _localePhrases.GetAll(correlationId, phraseid);

            var inputs = new Dictionary<string, ValueItem>();

            inputs.Add("Runtime", new ValueItem()
            {
                Id = "Runtime",
                DisplayValue = FormatValue(cardData?.Runtime, NULL_TEXT, digits, Duration.Hour.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.Runtime]),
                MeasurementAbbreviation = Duration.Hour.Symbol,
                Value = cardData?.Runtime == null ? null : Duration.FromHours((double)(cardData?.Runtime)).Amount,
            });

            inputs.Add("StrokesPerMinute", new ValueItem()
            {
                Id = "StrokesPerMinute",
                DisplayValue = FormatValue(cardData?.StrokesPerMinute.Value, NULL_TEXT, digits),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SPM]),
                Value = cardData?.StrokesPerMinute,
            });

            inputs.Add("StrokeLength", new ValueItem()
            {
                Id = "StrokeLength",
                DisplayValue = FormatValue(cardData?.StrokeLength, NULL_TEXT, digits, Length.Inch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.StrLength]),
                MeasurementAbbreviation = Length.Inch.Symbol,
                Value = cardData?.StrokeLength == null
                    ? null
                    : Length.FromInches((double)cardData?.StrokeLength).Amount,
            });

            var calculatedSurfaceCapacity24 =
                Convert.ToSingle(CalculateCapacity24(cardData?.StrokesPerMinute, cardData?.StrokeLength,
                    wellDetail.PlungerDiameter) + 0.5);

            var calculatedSurfaceCapacity24Quantity = ((float?)calculatedSurfaceCapacity24).HasValue
                ? LiquidFlowRate.FromBarrelsPerDay(calculatedSurfaceCapacity24)
                : null;

            inputs.Add("SurfaceCapacity24", new ValueItem()
            {
                Id = "SurfaceCapacity24",
                DisplayValue = FormatValue(calculatedSurfaceCapacity24Quantity?.Amount, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SurCap24]),
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                Value = calculatedSurfaceCapacity24Quantity.Amount,
            });

            inputs.Add("PumpDiameter", new ValueItem()
            {
                Id = "PumpDiameter",
                DisplayValue = FormatValue(wellDetail?.PlungerDiameter, NULL_TEXT, digits, Length.Inch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PmpDiam]),
                MeasurementAbbreviation = Length.Inch.Symbol,
                Value = wellDetail.PlungerDiameter == null
                    ? null
                    : Length.FromInches((double)wellDetail.PlungerDiameter).Amount,
            });

            inputs.Add("PumpDepth", new ValueItem()
            {
                Id = "PumpDepth",
                DisplayValue = FormatValue(wellDetail?.PumpDepth, NULL_TEXT, digits, Length.Foot.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpDepth]),
                MeasurementAbbreviation = Length.Foot.Symbol,
                Value = wellDetail.PumpDepth == null ? null : Length.FromFeet((double)wellDetail.PumpDepth).Amount,
            });

            float grossRate = 0;
            if (wellTest == null)
            {
                inputs.Add("WellTestDate", new ValueItem()
                {
                    Id = "WellTestDate",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestDate]),
                });

                inputs.Add("WellTestGas", new ValueItem()
                {
                    Id = "WellTestGas",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestGas]),
                });

                inputs.Add("WellTestOil", new ValueItem()
                {
                    Id = "WellTestOil",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestOil]),
                });

                inputs.Add("WellTestWater", new ValueItem()
                {
                    Id = "WellTestWater",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestWater]),
                });

                inputs.Add("WellTestGross", new ValueItem()
                {
                    Id = "WellTestGross",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestGross]),
                });
            }
            else
            {
                inputs.Add("WellTestDate", new ValueItem()
                {
                    Id = "WellTestDate",
                    DisplayValue = wellTest.TestDate.ToShortDateString(),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestDate]),
                });

                inputs.Add("WellTestGas", new ValueItem()
                {
                    Id = "WellTestGas",
                    DisplayValue = FormatValue(wellTest.GasRate, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestGas]),
                    MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
                    Value = wellTest.GasRate == null
                        ? null
                        : GasFlowRate.FromThousandStandardCubicFeetPerDay((double)wellTest.GasRate).Amount,
                });

                inputs.Add("WellTestOil", new ValueItem()
                {
                    Id = "WellTestOil",
                    DisplayValue = FormatValue(wellTest.OilRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestOil]),
                    MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                    Value = wellTest.OilRate == null
                        ? null
                        : LiquidFlowRate.FromBarrelsPerDay((double)wellTest.OilRate).Amount,
                });

                inputs.Add("WellTestWater", new ValueItem()
                {
                    Id = "WellTestWater",
                    DisplayValue = FormatValue(wellTest.WaterRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestWater]),
                    MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                    Value = wellTest.WaterRate == null
                        ? null
                        : LiquidFlowRate.FromBarrelsPerDay((double)wellTest.WaterRate).Amount,
                });

                grossRate = (wellTest.OilRate ?? 0) + (wellTest.WaterRate ?? 0);

                inputs.Add("WellTestGross", new ValueItem()
                {
                    Id = "WellTestGross",
                    DisplayValue = FormatValue(grossRate, string.Empty, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestGross]),
                    MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                    Value = LiquidFlowRate.FromBarrelsPerDay(grossRate).Amount,
                });
            }

            inputs.Add("CyclesYesterday", new ValueItem()
            {
                Id = "CyclesYesterday",
                DisplayValue = FormatValue(wellDetail.Cycles, NULL_TEXT, digits),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.Cycles]),
                Value = wellDetail.Cycles,
            });

            inputs.Add("IdleTime", new ValueItem()
            {
                Id = "IdleTime",
                DisplayValue = FormatValue(wellDetail.IdleTime, NULL_TEXT, digits, Duration.Minute.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.IdleTime]),
                MeasurementAbbreviation = Duration.Minute.Symbol,
                Value = wellDetail.IdleTime == null ? null : Duration.FromMinutes((double)wellDetail.IdleTime).Amount,
            });

            inputs.Add("PumpingUnitManufacturer", new ValueItem()
            {
                Id = "PumpingUnitManufacturer",
                DisplayValue = rodLiftAnalysisData.PumpingUnitManufacturer ?? string.Empty,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpingUnit]),
                Value = rodLiftAnalysisData.PumpingUnitManufacturer,
            });

            inputs.Add("PumpingUnitAPIDesignation", new ValueItem()
            {
                Id = "PumpingUnitAPIDesignation",
                DisplayValue = rodLiftAnalysisData.PumpingUnitAPIDesignation ?? string.Empty,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpingUnit]),
                Value = rodLiftAnalysisData.PumpingUnitAPIDesignation,
            });

            // Output

            IDictionary<int, string> phrasesCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhrases = new Dictionary<int, string>();

            var outputs = new Dictionary<string, ValueItem>();

            if (xdg != null)
            {
                outputs.Add("DownholeStroke", new ValueItem()
                {
                    Id = "DownholeStroke",
                    DisplayValue = FormatValue(xdg.GrossPumpStroke, NULL_TEXT, digits, Length.Inch.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DHStroke]),
                    MeasurementAbbreviation = Length.Inch.Symbol,
                    Value = xdg.GrossPumpStroke == null ? null : Length.FromInches((double)xdg.GrossPumpStroke).Amount,
                });

                outputs.Add("FluidLoad", new ValueItem()
                {
                    Id = "FluidLoad",
                    DisplayValue = FormatValue(xdg.FluidLoadonPump, NULL_TEXT, digits, Weight.Pound.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FluidLoad]),
                    MeasurementAbbreviation = Weight.Pound.Symbol,
                    Value = xdg.FluidLoadonPump == null ? null : Weight.FromPounds((double)xdg.FluidLoadonPump).Amount,
                });

                outputs.Add("BuoyantRodWeight", new ValueItem()
                {
                    Id = "BuoyantRodWeight",
                    DisplayValue = FormatValue(xdg.BouyRodWeight, NULL_TEXT, digits, Weight.Pound.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.BuoyantRodWeight]),
                    MeasurementAbbreviation = Weight.Pound.Symbol,
                    Value = xdg.BouyRodWeight == null ? null : Weight.FromPounds((double)xdg.BouyRodWeight).Amount,
                });

                outputs.Add("DryRodWeight", new ValueItem()
                {
                    Id = "DryRodWeight",
                    DisplayValue = FormatValue(xdg.DryRodWeight, NULL_TEXT, digits, Weight.Pound.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DryRodWeight]),
                    MeasurementAbbreviation = Weight.Pound.Symbol,
                    Value = xdg.DryRodWeight == null ? null : Weight.FromPounds((double)xdg.DryRodWeight).Amount,
                });

                outputs.Add("PumpFriction", new ValueItem()
                {
                    Id = "PumpFriction",
                    DisplayValue = xdg.PumpFriction == null
                        ? NULL_TEXT
                        : xdg.PumpFriction < 0
                            ? "N/A"
                            : FormatValue(xdg.PumpFriction, NULL_TEXT, digits, Weight.Pound.Symbol),
                    DisplayName = phrases[(int)PhraseIDs.PumpFrictionLoad],
                    MeasurementAbbreviation = Weight.Pound.Symbol,
                    Value = xdg.PumpFriction == null
                        ? null
                        : xdg.PumpFriction < 0
                            ? "N/A"
                            : Weight.FromPounds((double)xdg.PumpFriction).Amount,
                });

                outputs.Add("POFluidLoad", new ValueItem()
                {
                    Id = "POFluidLoad",
                    DisplayValue = FormatValue(xdg.PofluidLoad, NULL_TEXT, digits, Weight.Pound.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.POFluidLoad]),
                    MeasurementAbbreviation = Weight.Pound.Symbol,
                    Value = xdg.PofluidLoad == null ? null : Weight.FromPounds((double)xdg.PofluidLoad).Amount,
                });

                if (xdg.GrossPumpStroke.HasValue && xdg.PumpSize.HasValue)
                {
                    outputs.Add("DownholeCapacity24", new ValueItem()
                    {
                        Id = "DownholeCapacity24",
                        DisplayValue = FormatValue(xdg.DownholeCapacity24, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DHCap24]),
                        MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                        Value = xdg.DownholeCapacity24 == null
                            ? null
                            : LiquidFlowRate.FromBarrelsPerDay((double)xdg.DownholeCapacity24).Amount,
                    });

                    outputs.Add("DownholeCapacityRuntime", new ValueItem()
                    {
                        Id = "DownholeCapacityRuntime",
                        DisplayValue = FormatValue(xdg.DownholeCapacityRuntime, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DHCapRT]),
                        MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                        Value = xdg.DownholeCapacityRuntime == null
                            ? null
                            : LiquidFlowRate.FromBarrelsPerDay((double)xdg.DownholeCapacityRuntime).Amount,
                    });

                    outputs.Add("DownholeCapacityRuntimeFillage", new ValueItem()
                    {
                        Id = "DownholeCapacityRuntimeFillage",
                        DisplayValue = FormatValue(xdg.DownholeCapacityRuntimeFillage, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DHCapRTFillage]),
                        MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                        Value = xdg.DownholeCapacityRuntimeFillage == null
                            ? null
                            : LiquidFlowRate.FromBarrelsPerDay((double)xdg.DownholeCapacityRuntimeFillage).Amount
                    });
                }

                var additionalUpliftDisplayValue = FormatValue(xdg.AdditionalUplift, "N/A", digits, LiquidFlowRate.BarrelPerDay.Symbol);
                var additionalUpliftGrossDisplayValue = FormatValue(xdg.AdditionalUpliftGross, "N/A", digits, LiquidFlowRate.BarrelPerDay.Symbol);

                outputs.Add("AdditionalUplift", new ValueItem()
                {
                    Id = "AdditionalUplift",
                    DisplayValue = additionalUpliftDisplayValue,
                    DisplayName = "Incremental Oil", //todo: add display name from tblLocalePhrases
                    MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                    Value = (xdg.AdditionalUplift == null)
                        ? null
                        : LiquidFlowRate.FromBarrelsPerDay((double)xdg.AdditionalUplift).Amount,
                });

                outputs.Add("AdditionalUpliftGross", new ValueItem()
                {
                    Id = "AdditionalUpliftGross",
                    DisplayValue = additionalUpliftGrossDisplayValue,
                    DisplayName = "Incremental Gross", //todo: add display name from tblLocalePhrases
                    MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                    Value = (xdg.AdditionalUpliftGross == null)
                        ? null
                        : LiquidFlowRate.FromBarrelsPerDay((double)xdg.AdditionalUpliftGross).Amount,
                });

                outputs.Add("PumpEfficiency", new ValueItem()
                {
                    Id = "PumpEfficiency",
                    DisplayValue = FormatValue(xdg.PumpEfficiency == null ? null : xdg.PumpEfficiency, "N/A", digits, Fraction.Percentage.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpEfficiency]),
                    MeasurementAbbreviation = Fraction.Percentage.Symbol,
                    Value = (xdg.PumpEfficiency == null) ? null : Fraction.FromPercentage((double)xdg.PumpEfficiency / 100).Amount,
                });

                var upliftResult = string.IsNullOrWhiteSpace(xdg.UpliftCalculationMissingRequirements)
                    ? string.Empty
                    : xdg.UpliftCalculationMissingRequirements;
                var maximumSPMMessage = string.Empty;

                if (xdg.MaximumSpm.HasValue && xdg.ProductionAtMaximumSpm.HasValue && xdg.OilProductionAtMaximumSpm.HasValue)
                {
                    if (grossRate > 0 && wellTest?.OilRate > 0)
                    {
                        var deltaGrossRate = xdg.ProductionAtMaximumSpm.Value - grossRate;
                        var deltaOilRate = xdg.OilProductionAtMaximumSpm.Value - wellTest.OilRate.Value;
                        var displayGrossRate = string.Empty;
                        var displayOilRate = string.Empty;
                        var displayMaximumSPM = string.Empty;
                        var minimumProductionThreshold = string.Empty;
                        var displaySPM = string.Empty;
                        var displayMinimumProductionThreshold = string.Empty;

                        minimumProductionThreshold = systemParamter;

                        if (deltaOilRate >= Convert.ToSingle(minimumProductionThreshold))
                        {
                            displayGrossRate = FormatValue<float>(deltaGrossRate, string.Empty, digits);
                            displayOilRate = FormatValue<float>(deltaOilRate, string.Empty, digits);
                            displayMaximumSPM = FormatValue(xdg.MaximumSpm, NULL_TEXT, digits);

                            maximumSPMMessage =
                                $" {string.Format(phrases[(int)PhraseIDs.XDIAGSPM], displayMaximumSPM, displayGrossRate, displayOilRate)}";
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(xdg.MaximumSpmoverloadMessage))
                            {
                                maximumSPMMessage = $" {string.Format(phrases[(int)PhraseIDs.XDOAGPotentialuplift])}";
                            }
                            else
                            {
                                displaySPM = FormatValue(cardData.StrokesPerMinute, NULL_TEXT, digits);
                                displayMinimumProductionThreshold =
                                    FormatValue<float>(Convert.ToSingle(minimumProductionThreshold), string.Empty, digits);

                                maximumSPMMessage = " " + phrases[(int)PhraseIDs.XDOAGPotentialuplift] + (char)13 + (char)10 +
                                    string.Format(phrases[(int)PhraseIDs.SPMLessthenOilperday], displaySPM,
                                        displayMinimumProductionThreshold) +
                                    (char)13 + (char)10 + xdg.MaximumSpmoverloadMessage;
                            }
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(xdg.MaximumSpmoverloadMessage) == false)
                {
                    maximumSPMMessage = (char)13 + (char)10 + xdg.MaximumSpmoverloadMessage;
                }

                var bpdMeasuringUnit = LiquidFlowRate.BarrelPerDay.Symbol;
                outputs.Add("XDAnalysis", new ValueItem()
                {
                    Id = "XDAnalysis",
                    DisplayValue = _commonService.GetCardTypeName(rodLiftAnalysisData.CardType, rodLiftAnalysisData.CauseId,
                            rodLiftAnalysisData.PocType, ref phrasesCache, ref causeIdPhrases, correlationId) + ": " +
                        cardData.CardDate.ToShortDateString() + " " + cardData.CardDate.ToShortTimeString() +
                        (char)13 + (char)10 + (char)13 + (char)10 + xdg.DownholeAnalysis +
                        (char)13 + (char)10 + (char)13 + (char)10 + xdg.InputAnalysis +
                        (char)13 + (char)10 + (char)13 + (char)10 + xdg.RodAnalysis +
                        (char)13 + (char)10 + (char)13 + (char)10 + xdg.SurfaceAnalysis +
                        (string.IsNullOrWhiteSpace(upliftResult)
                            ? string.Empty
                            : (char)13 + (char)10 + upliftResult) +
                        (additionalUpliftDisplayValue == "N/A" || xdg.AdditionalUplift <= 0
                            ? string.Empty
                            : (char)13 + (char)10 + (char)13 + (char)10 +
                            string.Format(phrases[(int)PhraseIDs.Oiluplift], additionalUpliftGrossDisplayValue, bpdMeasuringUnit,
                                additionalUpliftDisplayValue,
                                bpdMeasuringUnit) +
                            maximumSPMMessage),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.AnalysisData]),
                });
            }
            else
            {
                outputs.Add("DownholeStroke", new ValueItem()
                {
                    Id = "DownholeStroke",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DHStroke]),
                });

                outputs.Add("FluidLoad", new ValueItem()
                {
                    Id = "FluidLoad",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FluidLoad]),
                });

                outputs.Add("BuoyantRodWeight", new ValueItem()
                {
                    Id = "BuoyantRodWeight",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.BuoyantRodWeight]),
                });

                outputs.Add("DryRodWeight", new ValueItem()
                {
                    Id = "DryRodWeight",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DryRodWeight]),
                });

                outputs.Add("PumpFriction", new ValueItem()
                {
                    Id = "PumpFriction",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpFrictionLoad]),
                });

                outputs.Add("POFluidLoad", new ValueItem()
                {
                    Id = "POFluidLoad",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.POFluidLoad]),
                });

                outputs.Add("DownholeCapacity24", new ValueItem()
                {
                    Id = "DownholeCapacity24",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DHCap24]),
                });

                outputs.Add("DownholeCapacityRuntime", new ValueItem()
                {
                    Id = "DownholeCapacityRuntime",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DHCapRT]),
                });

                outputs.Add("DownholeCapacityRuntimeFillage", new ValueItem()
                {
                    Id = "DownholeCapacityRuntimeFillage",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.DHCapRTFillage]),
                });

                outputs.Add("PumpEfficiency", new ValueItem()
                {
                    Id = "PumpEfficiency",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpEfficiency]),
                });

                outputs.Add("XDAnalysis", new ValueItem()
                {
                    Id = "XDAnalysis",
                    DisplayValue = _commonService.GetCardTypeName(rodLiftAnalysisData.CardType, rodLiftAnalysisData.CauseId,
                            rodLiftAnalysisData.PocType, ref phrasesCache, ref causeIdPhrases, correlationId) + ": " +
                        cardData.CardDate.ToShortDateString() + " " + cardData.CardDate.ToShortTimeString()
                        + (char)13 + (char)10 + phrases[(int)PhraseIDs.NA],
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.AnalysisData]),
                });
            }

            outputs.Add("SAMFillage", new ValueItem()
            {
                Id = "SAMFillage",
                DisplayValue = FormatValue(cardData.Fillage == null ? null : cardData.Fillage, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpFillage]),
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
                Value = (cardData.Fillage == null)
                    ? NULL_TEXT
                    : Fraction.FromPercentage((double)cardData.Fillage / 100).Amount,
            });

            outputs.Add("SAMFillageSetpoint", new ValueItem()
            {
                Id = "SAMFillageSetpoint",
                DisplayValue = FormatValue(cardData.AreaLimit, string.Empty, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FillageSetpoint]),
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
                Value = Fraction.FromPercentage((double)cardData.AreaLimit / 100).Amount,
            });

            if (cardData.SecondaryPumpFillage.HasValue)
            {
                outputs.Add("SecondaryPumpFillage", new ValueItem()
                {
                    Id = "SecondaryPumpFillage",
                    DisplayValue = FormatValue(cardData.SecondaryPumpFillage, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower("Sec Pump Fillage"), //todo: add display name from tblLocalePhrases
                    MeasurementAbbreviation = Fraction.Percentage.Symbol,
                    Value = (cardData.SecondaryPumpFillage == null)
                        ? null
                        : Fraction.FromPercentage((double)cardData.SecondaryPumpFillage / 100).Amount,
                });
            }

            outputs.Add("InferredProductionToday", new ValueItem()
            {
                Id = "InferredProductionToday",
                DisplayValue = FormatValue(node.InferredProd, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InfPrdToday]),
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                Value = (node.InferredProd == null)
                    ? null
                    : LiquidFlowRate.FromBarrelsPerDay((double)node.InferredProd).Amount,
            });

            outputs.Add("InferredProductionYesterday", new ValueItem()
            {
                Id = "InferredProductionYesterday",
                DisplayValue = FormatValue(wellDetail.POCGrossRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InfPrdYest]),
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                Value = (wellDetail.POCGrossRate == null)
                    ? null
                    : LiquidFlowRate.FromBarrelsPerDay((double)wellDetail.POCGrossRate).Amount,
            });

            outputs.Add("PeakNetTorque", new ValueItem()
            {
                Id = "PeakNetTorque",
                DisplayValue = FormatValue<int>(cardData.AreaLimit, NULL_TEXT, digits),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PeakNetTorque]),
                Value = cardData.AreaLimit,
            });

            outputs.Add("StructuralLoad", new ValueItem()
            {
                Id = "StructuralLoad",
                DisplayValue = FormatValue(cardData.LoadSpanLimit, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.StructuralLoad]),
                Value = (cardData.LoadSpanLimit == null)
                    ? null
                    : Fraction.FromPercentage((double)cardData.LoadSpanLimit / 100).Amount,
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
            });

            if (node.PocType == (short)Models.DeviceType.RPC_Lufkin_SAM ||
                node.PocType == (short)Models.DeviceType.RPC_Rockwell_OptiLift)
            {
                var wellTestOilAddress = 39748;
                var wellTestWaterAddress = 39749;
                var wellTestGasAddress = 39750;

                if (node.PocType == (short)Models.DeviceType.RPC_Rockwell_OptiLift)
                {
                    wellTestOilAddress = 35063;
                    wellTestWaterAddress = 35065;
                    wellTestGasAddress = 35069;
                }

                var currentRawScanData = rodLiftAnalysisData.CurrentRawScanData;

                var oil = currentRawScanData.FirstOrDefault(x => x.Address == wellTestOilAddress);

                if (oil != null)
                {
                    outputs.Add("SWTOil", new ValueItem()
                    {
                        Id = "SWTOil",
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SWTOilYesterday]),
                        DisplayValue = oil.Value.ToString(),
                        Value = oil.Value,
                    });
                }

                var water = currentRawScanData.FirstOrDefault(x => x.Address == wellTestWaterAddress);

                if (water != null)
                {
                    outputs.Add("SWTWater", new ValueItem()
                    {
                        Id = "SWTWater",
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SWTWaterYesterday]),
                        DisplayValue = water.Value.ToString(),
                        Value = water.Value,
                    });
                }

                var gas = currentRawScanData.FirstOrDefault(x => x.Address == wellTestGasAddress);

                if (gas != null)
                {
                    outputs.Add("SWTGas", new ValueItem()
                    {
                        Id = "SWTGas",
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SWTGasYesterday]),
                        DisplayValue = gas.Value.ToString(),
                        Value = gas.Value,
                    });
                }
            }

            values.Input = inputs.Values.ToList();
            values.Output = outputs.Values.ToList();

            return values;
        }

        private float CalculateCapacity24(float? spm, float? grossPumpStroke, float? plungerDiameter)
        {
            if (spm.HasValue && grossPumpStroke.HasValue && plungerDiameter.HasValue)
            {
                return Convert.ToSingle(0.1166 * spm.Value * grossPumpStroke.Value * Math.Pow(plungerDiameter.Value, 2));
            }

            return 0;
        }

        private string FormatValue(double? value, string emptyText, int significantDigits, string measurementUnit)
        {
            if (value.HasValue == false)
            {
                return emptyText;
            }

            if (string.IsNullOrWhiteSpace(measurementUnit))
            {
                return $"{MathUtility.RoundToSignificantDigits(value, significantDigits)}";
            }

            return $"{MathUtility.RoundToSignificantDigits(value, significantDigits)} {measurementUnit}";
        }

        private string FormatValue<T>(T? value, string emptyText, int significantDigits)
            where T : struct
        {
            if (value.HasValue == false)
            {
                return emptyText;
            }

            if (typeof(T) == typeof(float?) || typeof(T) == typeof(float))
            {
                var doubleValue = MathUtility.RoundToSignificantDigits(Convert.ToSingle(value.Value), significantDigits);

                return doubleValue.ToString();
            }

            if (typeof(T) == typeof(int?) || typeof(T) == typeof(int))
            {
                var doubleValue = MathUtility.RoundToSignificantDigits(Convert.ToInt32(value.Value), significantDigits);

                return doubleValue.ToString();
            }

            if (typeof(T) == typeof(short?) || typeof(T) == typeof(short))
            {
                var doubleValue = MathUtility.RoundToSignificantDigits(Convert.ToInt16(value.Value), significantDigits);

                return doubleValue.ToString();
            }

            if (typeof(T) == typeof(double?) || typeof(T) == typeof(double))
            {
                var doubleValue = MathUtility.RoundToSignificantDigits(Convert.ToDouble(value), significantDigits);

                return doubleValue.ToString();
            }

            return value.ToString();
        }

        #endregion

    }
}
