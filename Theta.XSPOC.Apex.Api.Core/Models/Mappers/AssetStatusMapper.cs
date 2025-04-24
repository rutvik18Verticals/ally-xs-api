using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs.AssetStatus;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.Asset;
using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;
using Theta.XSPOC.Apex.Kernel.UnitConversion.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Mappers
{
    /// <summary>
    /// Mapper to map rod lift asset status domain data to rod lift asset status Output data.
    /// </summary>
    public static class AssetStatusMapper
    {

        private static int SignificantDigitCount { get; set; } = 3;
        private static int CONTROLLER_VERSION { get; } = 40211;
        private static int REVISION_VERSION { get; } = 40212;
        private static int COUNTDOWN_HOURS { get; } = 30008;
        private static int COUNTDOWN_SECONDS { get; } = 30009;
        private static int WELL_OUTPUT_TO_CONTROL_VALVE_MA { get; } = 432061;
        private static Single MA_MINIMUM { get; } = 4.0F;
        private static Single MA_MAXIMUM { get; } = 20.0F;
        private static Single MA_RANGE { get; } = MA_MAXIMUM - MA_MINIMUM;

        private static readonly string TEXT_PCNT_OPEN = "     {0} % Open";

        /// <summary>
        /// Maps the rod lift asset status domain data to rod lift asset status Output data.
        /// </summary>
        /// <param name="data">The domain data to map the the Output data.</param>
        /// <returns>
        /// The <seealso cref="RodLiftAssetStatusDataOutput"/>. Returns null if the <paramref name="data"/> is null.
        /// </returns>
        public static RodLiftAssetStatusDataOutput Map(RodLiftAssetStatusData data)
        {
            if (data?.CoreData == null)
            {
                return null;
            }

            return new RodLiftAssetStatusDataOutput()
            {
                ImageOverlayItems = MapImageOverlayItems(data),
                Exceptions = MapExceptionData(data.ExceptionData),
                Alarms = MapAlarmData(data.AlarmData),
                StatusRegisters = MapRegisterData(data),
                WellStatusOverview = new List<PropertyValueOutput>()
                {
                    new PropertyValueOutput()
                    {
                        Label = "Communication yesterday",
                        Value = data.CoreData.CommunicationPercentageYesterday == null ? "" : data.CoreData.CommunicationPercentageYesterday.ToString(),
                        IsVisible = true,
                    },
                    new PropertyValueOutput()
                    {
                        Label = "Runtime today",
                        Value = data.CoreData.TodayRuntimePercentage == null ? "" : MathUtility.RoundToSignificantDigits(data.CoreData.TodayRuntimePercentage, SignificantDigitCount).ToString(),
                        IsVisible = true,
                    },
                    new PropertyValueOutput()
                    {
                        Label = "Runtime yesterday",
                        Value = data.CoreData.YesterdayRuntimePercentage == null ? "" : MathUtility.RoundToSignificantDigits(data.CoreData.YesterdayRuntimePercentage, SignificantDigitCount).ToString(),
                        IsVisible = true,
                    },
                },
                LastWellTest = MapWellTestData(data),
                RodStrings = MapRodString(data),
                DiagramType = data.DiagramType,
                GLAnalysisData = MapGLAnalysisData(data.GLAnalysisData),
                ChemicalInjectionInformation = MapChemicalInjectionData(data.ChemicalInjectionInformation),
                PlungerLiftData = MapPlungerLiftData(data.PlungerLiftData),
                FacilityTagData = MapFacilityTagData(data.FacilityTagData),
                PIDData = MapPIDData(data.PIDDataModel),
                ValveControlData = MapValveControlData(data.ValveControlData),
            };
        }

        /// <summary>
        /// Maps the rod lift asset status domain data to rod lift asset status Output data with significant digit override.
        /// </summary>
        /// <param name="data">The domain data to map the the Output data.</param>
        /// <param name="significantDigitCount">The significant digit count.</param>
        /// <returns>
        /// The <seealso cref="RodLiftAssetStatusDataOutput"/>. Returns null if the <paramref name="data"/> is null.
        /// </returns>
        public static RodLiftAssetStatusDataOutput Map(RodLiftAssetStatusData data, int significantDigitCount)
        {
            SignificantDigitCount = significantDigitCount;

            if (data?.CoreData == null)
            {
                return null;
            }

            return new RodLiftAssetStatusDataOutput()
            {
                ImageOverlayItems = MapImageOverlayItems(data),
                Exceptions = MapExceptionData(data.ExceptionData),
                Alarms = MapAlarmData(data.AlarmData),
                StatusRegisters = MapRegisterData(data),
                WellStatusOverview = new List<PropertyValueOutput>()
                {
                    new PropertyValueOutput()
                    {
                        Label = "Communication yesterday",
                        Value = data.CoreData.CommunicationPercentageYesterday == null ? "" : data.CoreData.CommunicationPercentageYesterday.ToString(),
                        IsVisible = true,
                    },
                    new PropertyValueOutput()
                    {
                        Label = "Runtime today",
                        Value = data.CoreData.TodayRuntimePercentage == null ? "" : MathUtility.RoundToSignificantDigits(data.CoreData.TodayRuntimePercentage, SignificantDigitCount).ToString(),
                        IsVisible = true,
                    },
                    new PropertyValueOutput()
                    {
                        Label = "Runtime yesterday",
                        Value = data.CoreData.YesterdayRuntimePercentage == null ? "" : MathUtility.RoundToSignificantDigits(data.CoreData.YesterdayRuntimePercentage, SignificantDigitCount).ToString(),
                        IsVisible = true,
                    },
                },
                LastWellTest = MapWellTestData(data),
                RodStrings = MapRodString(data),
                DiagramType = data.DiagramType,
                GLAnalysisData = MapGLAnalysisData(data.GLAnalysisData),
                ChemicalInjectionInformation = MapChemicalInjectionData(data.ChemicalInjectionInformation),
                PlungerLiftData = MapPlungerLiftData(data.PlungerLiftData),
                FacilityTagData = MapFacilityTagData(data.FacilityTagData),
                PIDData = MapPIDData(data.PIDDataModel),
                ValveControlData = MapValveControlData(data.ValveControlData),
                RefreshInterval = data.RefreshInterval,
                SmartenLiveUrl = data.SmartenLiveUrl,
            };
        }

        #region Private Static Methods

        private static IList<OverlayStatusDataOutput> MapImageOverlayItems(RodLiftAssetStatusData data)
        {
            if (data?.CoreData == null)
            {
                return new List<OverlayStatusDataOutput>();
            }

            var timeInStateSpan = new TimeSpan(0, data.CoreData.TimeInState ?? 0, 0);

            var result = new List<OverlayStatusDataOutput>()
            {
                new OverlayStatusDataOutput()
                {
                    Label = "API designation",
                    Value = data.CoreData.APIDesignation,
                    IsVisible = true,
                    OverlayField = OverlayFields.ApiDesignation,
                    DisplayState = DisplayState.Emphasis,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Controller",
                    Value = data.CoreData.PocTypeDescription,
                    IsVisible = true,
                    OverlayField = OverlayFields.ControllerType,
                },
                new OverlayStatusDataOutput()
                {
                    Label = $"{data.CoreData.PocTypeDescription}",
                    Value = data.CoreData.NodeAddress,
                    IsVisible = true,
                    OverlayField = OverlayFields.ControllerInformation
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Comms status",
                    Value = data.CoreData.CommunicationStatus,
                    IsVisible = true,
                    DisplayState = CommunicationDisplayState(data.CoreData.CommunicationStatus),
                    OverlayField = OverlayFields.CurrentCommunicationStatus,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Last good scan",
                    Value = data.CoreData.LastGoodScan?.ToString("M/d/yyyy, h:mm:ss tt"),
                    IsVisible = true,
                    OverlayField = OverlayFields.LastGoodScan,
                    DisplayState = DisplayState.Emphasis,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Motor loading",
                    Value = data.CoreData.MotorLoad?.ToString() + (data.CoreData.MotorLoad != null ? " %" : ""),
                    IsVisible = true,
                    OverlayField = OverlayFields.MotorLoading,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Motor type",
                    Value = $"{data.CoreData.PrimeMoverType} [Size (HP): {MathUtility.RoundToSignificantDigits(data.CoreData.RatedHorsePower, SignificantDigitCount)}]",
                    IsVisible = true,
                    OverlayField = OverlayFields.MotorType,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Pump fillage",
                    Value = data.CoreData.PumpFillage?.ToString() + (data.CoreData.PumpFillage != null ? " %" : ""),
                    IsVisible = true,
                    OverlayField = OverlayFields.PumpFillage,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Rod loading",
                    Value = data.CoreData.MaxRodLoading?.ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.RodLoading,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Run status",
                    Value = data.CoreData.RunStatus,
                    IsVisible = true,
                    OverlayField = OverlayFields.RunStatus,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Stroke length",
                    Value = data.CoreData.StrokeLength == null
                        ? ""
                        : $"{MathUtility.RoundToSignificantDigits((((Quantity)data.CoreData.StrokeLength).Amount), SignificantDigitCount)}" +
                          $" {data.CoreData.StrokeLengthUnitString}",
                    IsVisible = true,
                    OverlayField = OverlayFields.StrokeLength,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "SPM",
                    Value = MathUtility.RoundToSignificantDigits(data.CoreData.StrokesPerMinute, SignificantDigitCount)?.ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.StrokesPerMinute,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Structural loading",
                    Value = data.CoreData.StructuralLoading?.ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.StructuralLoading,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Time in state",
                    Value = timeInStateSpan.TotalMinutes < 60 ? $"{timeInStateSpan.TotalMinutes:N1} m" :
                        timeInStateSpan.TotalMinutes < 1440 ? $"{timeInStateSpan.TotalHours:N1} h" :
                        $"{timeInStateSpan.TotalDays:N1} d",
                    IsVisible = true,
                    OverlayField = OverlayFields.TimeInState,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Gearbox loading (%)",
                    Value = data.CoreData.GearBoxLoadPercentage?.ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.GearboxLoading,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Pump depth",
                    Value = data.CoreData.PumpDepth == null ? "" : MathUtility.RoundToSignificantDigits(((Quantity<Length>)data.CoreData.PumpDepth).Amount, SignificantDigitCount).ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.PumpDepth,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Pump type",
                    Value = data.CoreData.PumpType?.ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.PumpType,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Pumping unit",
                    Value = data.CoreData.PumpingUnitName?.ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.PumpingUnit,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Pump efficiency (%)",
                    Value = data.CoreData.PumpEfficiencyPercentage?.ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.PumpEfficiencyPercentage,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Current load factor",
                    Value = LoadFactorCalculation(
                        data.PlungerLiftData?.CasingPressure,
                        data.PlungerLiftData?.TubingPressure,
                        data.PlungerLiftData?.LinePressure),
                    IsVisible = true,
                    OverlayField = OverlayFields.CurrentLoadFactor,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Intermitten/Real time",
                    Value = "",
                    IsVisible = true,
                    OverlayField = OverlayFields.IntermittenRealTime,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "First head",
                    Value = "",
                    IsVisible = true,
                    OverlayField = OverlayFields.FirstHead,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Second head",
                    Value = "",
                    IsVisible = true,
                    OverlayField = OverlayFields.SecondHead,
                },
                new OverlayStatusDataOutput()
                {
                    Label = "Device ID",
                    Value = "",
                    IsVisible = true,
                    OverlayField = OverlayFields.DeviceID,
                },
            };

            if (!String.IsNullOrEmpty((data?.CoreData.NodeAddress)))
            {
                bool isNodeAddressParsed = TryParseNodeAddress(data?.CoreData.NodeAddress,
                    out Protocol protocol, out string hostname, out string opcTypeName,
                    out ushort? port, out _, out int? offset);

                if (isNodeAddressParsed)
                {
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Protocol",
                        Value = protocol.ToString(),
                        IsVisible = true,
                        OverlayField = OverlayFields.Protocol,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Host name",
                        Value = hostname?.ToString(),
                        IsVisible = true,
                        OverlayField = OverlayFields.HostName,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "OPC",
                        Value = opcTypeName?.ToString(),
                        IsVisible = true,
                        OverlayField = OverlayFields.OPCType,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Port",
                        Value = port?.ToString(),
                        IsVisible = true,
                        OverlayField = OverlayFields.Port,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Address",
                        Value = data?.CoreData.NodeAddress ?? "",
                        IsVisible = true,
                        OverlayField = OverlayFields.RTUAddress,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Offset",
                        Value = offset?.ToString(),
                        IsVisible = true,
                        OverlayField = OverlayFields.Offset,
                    });
                }
            }

            var cyclesToday = MapParamStandard(data.ParamStandardData, OverlayFields.CyclesToday);
            if (cyclesToday != null)
            {
                result.Add(cyclesToday);
            }

            var cyclesYesterday = MapParamStandard(data.ParamStandardData, OverlayFields.CyclesYesterday, "Cycles yesterday");

            if (cyclesYesterday != null)
            {
                result.Add(cyclesYesterday);
            }

            var flowlinePressure = MapParamStandard(data.ParamStandardData, OverlayFields.FlowlinePressure);

            if (flowlinePressure != null)
            {
                result.Add(flowlinePressure);
            }

            AddOverlayFieldsByDiagramType(ref result, data);

            return result;
        }

        private static void AddOverlayFieldsByDiagramType(ref List<OverlayStatusDataOutput> result, RodLiftAssetStatusData data)
        {
            var diagramType = data.DiagramType;

            if (diagramType == (int)DiagramType.ReverseMark)
            {
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Tubing pressure",
                    Value = MapParamStandard(data.ParamStandardData, OverlayFields.TubingPressure).Value == string.Empty ?
                                data.CoreData.TubingPressure == null ? "" :
                                    $"{MathUtility.RoundToSignificantDigits(((Quantity)data.CoreData.TubingPressure).Amount, SignificantDigitCount)}"
                                    + $" {data.CoreData.TubingPressureUnitString}"
                                : $"{MapParamStandard(data.ParamStandardData, OverlayFields.TubingPressure).Value}",
                    IsVisible = true,
                    OverlayField = OverlayFields.TubingPressure
                });

                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Casing pressure",
                    Value = MapParamStandard(data.ParamStandardData, OverlayFields.CasingPressure).Value == string.Empty ?
                                data.CoreData.CasingPressure == null ? "" :
                                    $"{MathUtility.RoundToSignificantDigits(((Quantity)data.CoreData.CasingPressure).Amount, SignificantDigitCount)}"
                                    + $" {data.CoreData.CasingPressureUnitString}"
                                : $"{MapParamStandard(data.ParamStandardData, OverlayFields.CasingPressure).Value}",
                    IsVisible = true,
                    OverlayField = OverlayFields.CasingPressure
                });
            }
            else
            {
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Tubing pressure",
                    Value = data.CoreData.TubingPressure == null
                        ? ""
                        : $"{MathUtility.RoundToSignificantDigits(((Quantity)data.CoreData.TubingPressure).Amount, SignificantDigitCount)}" +
                          $" {data.CoreData.TubingPressureUnitString}",
                    IsVisible = true,
                    OverlayField = OverlayFields.TubingPressure
                });

                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Casing pressure",
                    Value = data.CoreData.CasingPressure == null
                        ? ""
                        : $"{MathUtility.RoundToSignificantDigits(((Quantity)data.CoreData.CasingPressure).Amount, SignificantDigitCount)}" +
                          $" {data.CoreData.CasingPressureUnitString}",
                    IsVisible = true,
                    OverlayField = OverlayFields.CasingPressure
                });
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Gas flow rate",
                    Value = data.CoreData.RateAtTest == null
                        ? ""
                        : $"{MathUtility.RoundToSignificantDigits(((Quantity)data.CoreData.RateAtTest).Amount, SignificantDigitCount)}" +
                          $" {data.CoreData.RateAtTestString}",
                    IsVisible = true,
                    OverlayField = OverlayFields.GasRate
                });
            }

            if (diagramType == (int)DiagramType.GasFlowMonitor)
            {
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.MeterId, "Meter"));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowRate, "Volume flow rate"));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.TodayContractVolume));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.YestContractVolume));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.AccumulatedVolume));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.LastCalcPeriodVol));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.DiffPress));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.StaticPressure));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.TodaysMass));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.YesterdaysMass));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.AccumulatedMass));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.PulseCount));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.BatteryVoltage));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.SolarVoltage));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.EnergyRate));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.TodaysEnergy));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.YesterdaysEnergy));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionTodayVolume, "Todays volume"));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionYesterdayVolume, "Yesterdays volume"));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.Meter, "Meter"));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.Temperature));
                result.Add(MapParamStandardTypeStringValue(data.ParamStandardData, OverlayFields.TubeID, "Tube ID"));
                result.Add(MapParamStandardTypeStringValue(data.ParamStandardData, OverlayFields.TubeDescription, "Tube Description"));

                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Location",
                    Value = "",
                    IsVisible = true,
                    OverlayField = OverlayFields.Location,
                });
            }
            else if (diagramType == (int)DiagramType.LiquidFlowMeter)
            {
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowRate, "Volume flow rate"));
            }
            else if (diagramType == (int)DiagramType.GasLiftManager)
            {
                var pocType = data.CoreData.PocType;

                //IsPCSFGasLif
                if (pocType == (short)DeviceType.PCS_Ferguson_GLM_Well
                  || pocType == (short)DeviceType.PCS_Ferguson_AutoLift_Well)
                {
                    PopulatePCSFGasLift(ref result, data);
                }
                //IsTotalFlowGasLift
                else if (pocType == (short)DeviceType.TF_GasLift_Well)
                {
                    PopulateTotalFlowGasLift(ref result, data);
                }
                else
                {
                    PopulateCustomGasLift(ref result, data);
                }
            }
            else if (diagramType == (int)DiagramType.GasLift)
            {
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Current mode",
                    Value = data.CoreData.RunStatus.ToString(),
                    IsVisible = true,
                    OverlayField = OverlayFields.CurrentMode,
                });

                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Injection flow",
                    Value = string.Empty, //todo
                    IsVisible = true,
                    OverlayField = OverlayFields.CurrentMode,
                });
                string countdownTimer = string.Empty;
                var countdownSeconds = data.RegisterData.FirstOrDefault(r => r.Address == COUNTDOWN_SECONDS);
                var countdownHours = data.RegisterData.FirstOrDefault(r => r.Address == COUNTDOWN_HOURS);
                if (countdownSeconds != null && countdownHours != null)
                {
                    var cHours = countdownHours.Value as AnalogValue;
                    var cSeconds = countdownSeconds.Value as AnalogValue;

                    var sCountdownMinutes = cSeconds.Amount / 60;
                    var sCountdownSeconds = cSeconds.Amount % 60;
                    countdownTimer = cHours.Amount + ":" + (sCountdownMinutes) + ":" + (sCountdownSeconds);
                }

                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Countdown timer",
                    Value = countdownTimer,
                    IsVisible = true,
                    OverlayField = OverlayFields.CurrentMode,
                });

                string firmwareVersion = string.Empty;
                if (!string.IsNullOrEmpty(data.CoreData.FirmwareVersion.ToString()))
                {
                    firmwareVersion = data.CoreData.FirmwareVersion.ToString();
                }
                else
                {
                    var controllerVersion = data.RegisterData.FirstOrDefault(r => r.Address == CONTROLLER_VERSION);
                    var revisionVersion = data.RegisterData.FirstOrDefault(r => r.Address == REVISION_VERSION);

                    if (controllerVersion != null && revisionVersion != null)
                    {
                        var cVersion = controllerVersion.Value as AnalogValue;
                        var rVersion = revisionVersion.Value as AnalogValue;
                        firmwareVersion = cVersion.Amount.ToString() + " - " + cVersion.Amount.ToString();
                    }
                }

                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Firmware version",
                    Value = firmwareVersion,
                    IsVisible = true,
                    OverlayField = OverlayFields.FirmwareVersion,
                });

                var fieldCasingPressure = MapParamStandard(data.ParamStandardData, OverlayFields.CasingPressure);
                if (fieldCasingPressure.Value == string.Empty)
                {
                    fieldCasingPressure.Value = data.CoreData.CasingPressure.ToString();
                }

                var fieldTubingPressure = MapParamStandard(data.ParamStandardData, OverlayFields.TubingPressure);
                if (fieldTubingPressure.Value == string.Empty)
                {
                    fieldTubingPressure.Value = data.CoreData.TubingPressure.ToString();
                }

                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.InjectionPressure));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.TodaysFlow));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionTemp));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.BatteryVoltage));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.SolarVoltage));
            }
            else if (diagramType == (int)DiagramType.Facility || diagramType == (int)DiagramType.RegisterView)
            {
                if (data.FacilityTagData != null)
                {
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Comms status",
                        Value = data.FacilityTagData.CommStatus ?? string.Empty,
                        IsVisible = true,
                        OverlayField = OverlayFields.FacilityComms,
                        DisplayState = CommunicationDisplayState(data.FacilityTagData.CommStatus),
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Enabled",
                        Value = data.FacilityTagData.Enabled ? "True" : "False",
                        IsVisible = true,
                        OverlayField = OverlayFields.FacilityEnabled,
                        DisplayState = data.FacilityTagData.Enabled ? DisplayState.Ok : DisplayState.Error,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Tag count",
                        Value = data.FacilityTagData.TagCount.ToString() ?? string.Empty,
                        IsVisible = true,
                        OverlayField = OverlayFields.FacilityTagCount,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Alarm Count",
                        Value = data.FacilityTagData.AlarmCount.ToString() ?? string.Empty,
                        IsVisible = true,
                        OverlayField = OverlayFields.FacilityAlarmCount,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Host alarm",
                        Value = data.FacilityTagData.NodeHostAlarm ?? string.Empty,
                        IsVisible = true,
                        OverlayField = OverlayFields.FacilityHostAlarm,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Comment",
                        Value = data.FacilityTagData.Comment ?? string.Empty,
                        IsVisible = true,
                        OverlayField = OverlayFields.FacilityComment,
                    });
                }
            }
            else if (diagramType == (int)DiagramType.Esp)
            {
                if (data.ESPMotorInformation != null)
                {
                    var espMotorInformation = data.ESPMotorInformation;

                    var fluidLevel = string.Empty;
                    if (data.CoreData.CalculatedFluidLevelAbovePump != null)
                    {
                        fluidLevel = $"{MathUtility.RoundToSignificantDigits((double)data.CoreData.CalculatedFluidLevelAbovePump, SignificantDigitCount)}" + " " + $"{data.CoreData.FluidLevelUnitString}";
                    }
                    else if (data.CoreData.FluidLevel != null)
                    {
                        fluidLevel = $"{MathUtility.RoundToSignificantDigits(((Quantity)data.CoreData.FluidLevel).Amount, SignificantDigitCount)} " +
                            $"{data.CoreData.FluidLevelUnitString}";
                    }

                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = $"{data.CoreData.PocTypeDescription}\n{data.CoreData.NodeAddress}",
                        Value = null,
                        IsVisible = true,
                        OverlayField = OverlayFields.ControllerAndNode,
                    });

                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Fluid level",
                        Value = fluidLevel,
                        IsVisible = true,
                        OverlayField = OverlayFields.FluidLevel,
                    });
                    if (!string.IsNullOrEmpty(espMotorInformation.Seal))
                    {
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Seal",
                            Value = espMotorInformation.Seal,
                            IsVisible = true,
                            OverlayField = OverlayFields.Seal,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Seal series",
                            Value = espMotorInformation.SealSeries,
                            IsVisible = true,
                            OverlayField = OverlayFields.SealSeries,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Seal model",
                            Value = espMotorInformation.SealModel,
                            IsVisible = true,
                            OverlayField = OverlayFields.SealModel,
                        });
                    }
                    else
                    {
                        if (espMotorInformation.Seals != null)
                        {
                            var counter = 1;
                            foreach (var seal in espMotorInformation.Seals)
                            {
                                result.Add(new OverlayStatusDataOutput()
                                {
                                    Label = "Seal",
                                    Value = espMotorInformation.Seals.Count > 1 ? counter + ": " + seal.Name : seal.Name,
                                    IsVisible = true,
                                    OverlayField = OverlayFields.Seal,
                                });
                                counter++;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(espMotorInformation.Cable))
                    {
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Cable",
                            Value = espMotorInformation.Cable,
                            IsVisible = true,
                            OverlayField = OverlayFields.Cable,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Cable series",
                            Value = espMotorInformation.CableSeries,
                            IsVisible = true,
                            OverlayField = OverlayFields.CableSeries,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Cable description",
                            Value = espMotorInformation.CableDescription,
                            IsVisible = true,
                            OverlayField = OverlayFields.CableDescription,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Cable type",
                            Value = espMotorInformation.CableType,
                            IsVisible = true,
                            OverlayField = OverlayFields.CableType,
                        });
                    }
                    else
                    {
                        if (espMotorInformation.Cables != null)
                        {
                            var counter = 1;
                            foreach (var cable in espMotorInformation.Cables)
                            {
                                result.Add(new OverlayStatusDataOutput()
                                {
                                    Label = "Cable",
                                    Value = espMotorInformation.Cables.Count > 1 ? counter + ": " + cable.Name : cable.Name,
                                    IsVisible = true,
                                    OverlayField = OverlayFields.Cable,
                                });
                                counter++;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(espMotorInformation.Motor))
                    {
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Motor",
                            Value = espMotorInformation.Motor,
                            IsVisible = true,
                            OverlayField = OverlayFields.Motor,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Motor model",
                            Value = espMotorInformation.MotorModel,
                            IsVisible = true,
                            OverlayField = OverlayFields.MotorModel,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Motor series",
                            Value = espMotorInformation.MotorSeries,
                            IsVisible = true,
                            OverlayField = OverlayFields.MotorSeries,
                        });
                    }
                    else
                    {
                        if (espMotorInformation.Motors != null)
                        {
                            var counter = 1;
                            foreach (var motor in espMotorInformation.Motors)
                            {
                                result.Add(new OverlayStatusDataOutput()
                                {
                                    Label = "Motor",
                                    Value = espMotorInformation.Motors.Count > 1 ? counter + ": " + motor.Name : motor.Name,
                                    IsVisible = true,
                                    OverlayField = OverlayFields.Motor,
                                });
                                counter++;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(espMotorInformation.MotorLead))
                    {
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Motor lead",
                            Value = espMotorInformation.MotorLead,
                            IsVisible = true,
                            OverlayField = OverlayFields.MotorLead,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Motor lead description",
                            Value = espMotorInformation.MotorLeadDescription,
                            IsVisible = true,
                            OverlayField = OverlayFields.MotorLeadDescription,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Motor lead series",
                            Value = espMotorInformation.MotorLeadSeries,
                            IsVisible = true,
                            OverlayField = OverlayFields.MotorLeadSeries,
                        });
                        result.Add(new OverlayStatusDataOutput()
                        {
                            Label = "Motor lead type",
                            Value = espMotorInformation.MotorLeadType,
                            IsVisible = true,
                            OverlayField = OverlayFields.MotorLeadType,
                        });
                    }
                    else
                    {
                        if (espMotorInformation.MotorLeads != null)
                        {
                            var counter = 1;
                            foreach (var motorleads in espMotorInformation.MotorLeads)
                            {
                                result.Add(new OverlayStatusDataOutput()
                                {
                                    Label = "Motor lead",
                                    Value = espMotorInformation.MotorLeads.Count > 1 ? counter + ": " + motorleads.Name : motorleads.Name,
                                    IsVisible = true,
                                    OverlayField = OverlayFields.MotorLead,
                                });
                                counter++;
                            }
                        }
                    }
                }

                if (data.ESPPumpInformation != null && string.IsNullOrWhiteSpace(data.ESPPumpInformation.Pump) == false
                    && string.IsNullOrWhiteSpace((data.ESPPumpInformation.Manufacturer)) == false)
                {
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Pump model",
                        Value = data.ESPPumpInformation.Pump,
                        IsVisible = true,
                        OverlayField = OverlayFields.Pump,
                    });
                    result.Add(new OverlayStatusDataOutput()
                    {
                        Label = "Manufacturer",
                        Value = data.ESPPumpInformation.Manufacturer,
                        IsVisible = true,
                        OverlayField = OverlayFields.Manufacturer,
                    });
                }
                else
                {
                    var espPumpConfig = data.ESPMotorInformation?.PumpConfigurations;

                    if (espPumpConfig != null && espPumpConfig.Count > 0)
                    {
                        var counter = 1;
                        foreach (var pump in espPumpConfig)
                        {
                            result.Add(new OverlayStatusDataOutput()
                            {
                                Label = "Pump",
                                Value = espPumpConfig.Count > 1 ? counter + ": " + pump.Pump : pump.Pump,
                                IsVisible = true,
                                OverlayField = OverlayFields.Pump,
                            });
                            counter++;
                        }
                    }
                }
            }
            else if (diagramType == (int)DiagramType.TotalFlowTank)
            {
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.LastSample));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.Temperature));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.Volume));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.TankLevel));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.InterfaceLevel));
            }
            else if (diagramType == (int)DiagramType.Efm)
            {
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowingTemperature));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowRate, "Gas flow rate"));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.DiffPress));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.StaticPressure));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.BatteryVoltage));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionTodayVolume, "Today's gas production"));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionYesterdayVolume, "Yesterdays gas production"));
            }
            else if (diagramType == (int)DiagramType.PulseAccumulator)
            {
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowRate, "Volume flow rate"));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.AccumulatedVolume));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.PulseCount));
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionYesterdayVolume, "Yesterday's volume"));
            }
            else if (diagramType == (int)DiagramType.PID)
            {
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Mode",
                    Value = data.PIDDataModel.ControllerModeValue ?? "",
                    IsVisible = true,
                    OverlayField = OverlayFields.PIDMode,
                });
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Station ID",
                    Value = data.ValveControlData is null ? "" : data.ValveControlData.StationId,
                    IsVisible = true,
                    OverlayField = OverlayFields.StationID,
                });
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Location",
                    Value = data.ValveControlData is null ? "" : data.ValveControlData.Location,
                    IsVisible = true,
                    OverlayField = OverlayFields.Location,
                });
            }
            else if (diagramType == (int)DiagramType.ValveControl)
            {
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Station ID",
                    Value = data.ValveControlData is null ? "" : data.ValveControlData.StationId,
                    IsVisible = true,
                    OverlayField = OverlayFields.StationID,
                });
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Location",
                    Value = data.ValveControlData is null ? "" : data.ValveControlData.Location,
                    IsVisible = true,
                    OverlayField = OverlayFields.Location,
                });
            }
            else if (diagramType == (int)DiagramType.MultiWellPlungerLift)
            {
                result.Add(new OverlayStatusDataOutput()
                {
                    Label = "Firmware version",
                    Value = data.PlungerLiftData.ControllerFirmwareVersion,
                    IsVisible = true,
                    OverlayField = OverlayFields.FirmwareVersion,
                });
            }
        }

        private static void PopulateCustomGasLift(ref List<OverlayStatusDataOutput> result, RodLiftAssetStatusData data)
        {
            var controlValue = MapParamStandard(data.ParamStandardData, OverlayFields.ValvePercentOpen, "Control valve");

            var controlValueData = string.Format(TEXT_PCNT_OPEN, BoundsCheck_Percent(controlValue.Value));

            controlValue.Value = controlValueData;

            result.Add(controlValue);
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionFlowRate, "Gas flow rate"));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowlinePressure));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionTodayVolume));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionYesterdayVolume));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowRate));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.InjectionPress));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.GasInjectionRate, "Injection rate"));
        }

        private static void PopulateTotalFlowGasLift(ref List<OverlayStatusDataOutput> result, RodLiftAssetStatusData data)
        {
            var controlValue = MapParamStandard(data.ParamStandardData, OverlayFields.ControlValue, "Control valve");

            var controlValueData = string.Format(TEXT_PCNT_OPEN, BoundsCheck_Percent(controlValue.Value));

            controlValue.Value = controlValueData;

            result.Add(controlValue);
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionFlowRate, "Gas flow rate"));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowlinePressure));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionTodayVolume));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionYesterdayVolume));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowRate));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.InjectionPress));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.GasInjectionRate, "Injection rate"));
        }

        private static void PopulatePCSFGasLift(ref List<OverlayStatusDataOutput> result, RodLiftAssetStatusData data)
        {
            string tempValveVal = null;
            string controlValueData = string.Empty;
            string countdownTimer = string.Empty;

            var controlValue = data.RegisterData.FirstOrDefault(r => r.Address == WELL_OUTPUT_TO_CONTROL_VALVE_MA);

            if ((controlValue?.Value) is AnalogValue outputValveMa)
            {
                float outputValvePcntOpen = ValvePcntOpenFromMa(outputValveMa.Amount);
                tempValveVal = (outputValvePcntOpen * 100.0F).ToString();
            }

            if (tempValveVal == null)
            {
                controlValueData = "";
            }
            else
            {
                if (tempValveVal == "Invalid Data")
                {
                    controlValueData = string.Format(TEXT_PCNT_OPEN, BoundsCheck_Percent(tempValveVal));
                }
                else
                {
                    controlValueData = string.Format(TEXT_PCNT_OPEN,
                                                           BoundsCheck_Percent(((int.Parse(tempValveVal)) / 100).ToString()));
                }
            }

            result.Add(new OverlayStatusDataOutput()
            {
                Label = "Control valve",
                Value = controlValueData,
                IsVisible = true,
                OverlayField = OverlayFields.ControlValue,
            });

            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.ProductionFlowRate, "Gas flow rate"));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.FlowlinePressure));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.TodayContractVolume));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.YestContractVolume));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.InjectionPressure));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.InjectionPress));
            result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.GasInjectionRate, "Injection rate"));

            //update casing pressure and tubing pressure, production flow values for PCSF GasLift
            if (result.Any(x => x.OverlayField == OverlayFields.CasingPressure))
            {
                var casingPressureField = MapParamStandard(data.ParamStandardData, OverlayFields.CasingPressure);
                result.FirstOrDefault(x => x.OverlayField == OverlayFields.CasingPressure).Value = casingPressureField.Value;
            }
            else
            {
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.CasingPressure));
            }

            if (result.Any(x => x.OverlayField == OverlayFields.TubingPressure))
            {
                var tubingPressureField = MapParamStandard(data.ParamStandardData, OverlayFields.TubingPressure);
                result.FirstOrDefault(x => x.OverlayField == OverlayFields.TubingPressure).Value = tubingPressureField.Value;
            }
            else
            {
                result.Add(MapParamStandard(data.ParamStandardData, OverlayFields.TubingPressure));
            }

        }

        private static IList<PropertyValueOutput> MapRodString(RodLiftAssetStatusData data)
        {
            return data?.RodStrings == null
                ? new List<PropertyValueOutput>()
                : data.RodStrings.Where(m => m != null).OrderBy(r => r.RodStringPositionNumber).Select(MapRodString)
                    .ToList();
        }

        private static PropertyValueOutput MapRodString(RodStringData item)
        {
            if (item == null)
            {
                return null;
            }

            return new PropertyValueOutput()
            {
                Label = $"{item.RodStringPositionNumber} {item.RodStringSizeDisplayName}" +
                        $" {item.RodStringGradeName}",
                Value = $"{Math.Round(((Quantity)item.Length).Amount)} {item.UnitString}",
                IsVisible = true,
            };
        }

        private static IList<PropertyValueOutput> MapWellTestData(
            RodLiftAssetStatusData data)
        {
            var rodLiftAssetStatusCoreData = data.CoreData;

            if (rodLiftAssetStatusCoreData.LastWellTestDate == null)
            {
                return new List<PropertyValueOutput>();
            }

            string waterRate = string.Empty;
            string oilRate = string.Empty;
            string grossRate = string.Empty;
            string gasRate = string.Empty;

            if (rodLiftAssetStatusCoreData.GasRate != null)
            {
                gasRate = MathUtility
                    .RoundToSignificantDigits(((Quantity)rodLiftAssetStatusCoreData.GasRate).Amount, SignificantDigitCount)
                    .ToString();
            }
            if (rodLiftAssetStatusCoreData.GrossRate != null)
            {
                if (rodLiftAssetStatusCoreData.WaterCut != null)
                {
                    var calculatedWaterRate = ((Quantity)rodLiftAssetStatusCoreData.GrossRate).Amount *
                                              ((Quantity)rodLiftAssetStatusCoreData.WaterCut).Amount /
                                              100;
                    var calculatedOilRate =
                        ((Quantity)rodLiftAssetStatusCoreData.GrossRate).Amount - calculatedWaterRate;

                    waterRate = MathUtility
                        .RoundToSignificantDigits(calculatedWaterRate, SignificantDigitCount).ToString();
                    oilRate = MathUtility
                        .RoundToSignificantDigits(calculatedOilRate, SignificantDigitCount).ToString();
                }

                grossRate = MathUtility
                    .RoundToSignificantDigits(((Quantity)rodLiftAssetStatusCoreData.GrossRate).Amount,
                        SignificantDigitCount).ToString();
            }

            return new List<PropertyValueOutput>()
            {
                new PropertyValueOutput()
                {
                    Label = "Test date",
                    Value = rodLiftAssetStatusCoreData.LastWellTestDate.ToString() ?? "",
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Test gas",
                    Value = gasRate,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Test oil",
                    Value = oilRate,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Test water",
                    Value = waterRate,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Test gross",
                    Value = grossRate,
                    IsVisible = true,
                }
            };
        }

        private static OverlayStatusDataOutput MapParamStandardTypeStringValue(IList<ParamStandardData> dataParamStandardData,
            OverlayFields field, string label = null)
        {

#pragma warning disable IDE0072
            var lookUpKey = field switch
            {
                OverlayFields.TubeID => StandardMeasurement.EFMTubeID.Key,
                OverlayFields.TubeDescription => StandardMeasurement.MeterName.Key,
                _ => (int?)null
            };
#pragma warning restore IDE0072

            if (lookUpKey == null)
            {
                return null;
            }

            var result = dataParamStandardData?.Where(p => p != null)
                .FirstOrDefault(m => m.ParamStandardType == lookUpKey);

            if (result?.ParamStandardType == null)
            {
                return new OverlayStatusDataOutput
                {
                    OverlayField = field,
                    Value = string.Empty,
                    Label = label ?? ToSentenceCase(StandardMeasurement.GetValue((int)lookUpKey).Name.ToString()),
                    IsVisible = true,
                };
            }

            return new OverlayStatusDataOutput()
            {
                OverlayField = field,
                Value = result.StringValue,
                Label = label ?? ToSentenceCase(StandardMeasurement.GetValue(result.ParamStandardType.Value).Name.ToString()),
                IsVisible = true,
            };
        }

        private static OverlayStatusDataOutput MapParamStandard(IList<ParamStandardData> dataParamStandardData,
            OverlayFields field, string label = null)
        {
#pragma warning disable IDE0072
            var lookUpKey = field switch
            {
                OverlayFields.CyclesToday => StandardMeasurement.CyclesToday.Key,
                OverlayFields.CyclesYesterday => StandardMeasurement.Cycles.Key,
                OverlayFields.FlowlinePressure => StandardMeasurement.FlowlinePressure.Key,
                OverlayFields.MeterId => StandardMeasurement.EFMTubeID.Key,
                OverlayFields.FlowRate => StandardMeasurement.FlowRate.Key,
                OverlayFields.TodayContractVolume => StandardMeasurement.TodayContractVolume.Key,
                OverlayFields.YestContractVolume => StandardMeasurement.YestContractVolume.Key,
                OverlayFields.AccumulatedVolume => StandardMeasurement.AccumulatedVolume.Key,
                OverlayFields.LastCalcPeriodVol => StandardMeasurement.LastCalcPeriodVol.Key,
                OverlayFields.DiffPress => StandardMeasurement.DiffPress.Key,
                OverlayFields.StaticPressure => StandardMeasurement.StaticPressure.Key,
                OverlayFields.TodaysMass => StandardMeasurement.TodaysMass.Key,
                OverlayFields.YesterdaysMass => StandardMeasurement.YesterdaysMass.Key,
                OverlayFields.AccumulatedMass => StandardMeasurement.AccumulatedMass.Key,
                OverlayFields.PulseCount => StandardMeasurement.PulseCount.Key,
                OverlayFields.BatteryVoltage => StandardMeasurement.BatteryVoltage.Key,
                OverlayFields.SolarVoltage => StandardMeasurement.SolarVoltage.Key,
                OverlayFields.EnergyRate => StandardMeasurement.EnergyRate.Key,
                OverlayFields.TodaysEnergy => StandardMeasurement.TodaysEnergy.Key,
                OverlayFields.YesterdaysEnergy => StandardMeasurement.YesterdaysEnergy.Key,
                OverlayFields.CasingPressure => StandardMeasurement.CasingPress.Key,
                OverlayFields.TubingPressure => StandardMeasurement.TubingPress.Key,
                OverlayFields.InjectionPressure => StandardMeasurement.FlowAccumMcfYDay.Key,
                OverlayFields.TodaysFlow => StandardMeasurement.FlowAccumMcfToday.Key,
                OverlayFields.ProductionTemp => StandardMeasurement.FlowlinePressure.Key,
                OverlayFields.ProductionFlowRate => StandardMeasurement.ProductionFlowRate.Key,
                OverlayFields.ProductionPressure => StandardMeasurement.FlowlinePressure.Key,
                OverlayFields.InjectionPress => StandardMeasurement.InjectionPressure.Key,
                OverlayFields.ControlValue => StandardMeasurement.PrevOpenTubingPsig.Key,
                OverlayFields.FlowingTemperature => StandardMeasurement.FlowingTemperature.Key,
                OverlayFields.ProductionTodayVolume => StandardMeasurement.TodayContractVolume.Key,
                OverlayFields.ProductionYesterdayVolume => StandardMeasurement.YestContractVolume.Key,
                OverlayFields.GasInjectionRate => StandardMeasurement.GasInjectionRate.Key,
                OverlayFields.ValvePercentOpen => StandardMeasurement.ValvePercentOpen.Key,
                OverlayFields.LastSample => StandardMeasurement.LastSample.Key,
                OverlayFields.Temperature => StandardMeasurement.Temperature.Key,
                OverlayFields.Volume => StandardMeasurement.Volume.Key,
                OverlayFields.TankLevel => StandardMeasurement.TankLevel.Key,
                OverlayFields.InterfaceLevel => StandardMeasurement.InterfaceLevel.Key,
                OverlayFields.Meter => StandardMeasurement.EFMTubeID.Key,
                OverlayFields.GasRate => StandardMeasurement.GasRate.Key,
                OverlayFields.TubeID => StandardMeasurement.EFMTubeID.Key,
                OverlayFields.TubeDescription => StandardMeasurement.MeterName.Key,
                _ => (int?)null
            };
#pragma warning restore IDE0072

            if (lookUpKey == null)
            {
                return null;
            }

            var overlayDataOutput = new OverlayStatusDataOutput
            {
                OverlayField = field,
                Value = string.Empty,
                Label = label ?? ToSentenceCase(StandardMeasurement.GetValue((int)lookUpKey).Name.ToString()),
                IsVisible = true,
            };

            var result = dataParamStandardData?.Where(p => p != null)
                .FirstOrDefault(m => m.ParamStandardType == lookUpKey);

            if (result?.ParamStandardType == null)
            {
                return overlayDataOutput;
            }

            var trueValue = result.Value switch
            {
                AnalogValue analogValue => analogValue.Amount,
                Quantity quantityValue => (float)quantityValue.Amount,
                _ => 0f,
            };

            var unitType = result.Value switch
            {
                AnalogValue analogValue => string.Empty,
                Quantity quantityValue => result.Units ?? quantityValue.Unit.Symbol,
                _ => string.Empty,
            };

            if (string.IsNullOrWhiteSpace(unitType) == false)
            {
                unitType = " " + unitType;
            }

            if (result.Decimals != null && result.Value != null)
            {
                trueValue = (float)MathUtility.RoundToSignificantDigits(trueValue, SignificantDigitCount);
            }

            return new OverlayStatusDataOutput()
            {
                OverlayField = field,
                Value = $"{trueValue}{unitType}",
                Label = label ?? ToSentenceCase(StandardMeasurement.GetValue(result.ParamStandardType.Value).Name.ToString()),
                IsVisible = true,
            };
        }

        private static IList<PropertyValueOutput> MapExceptionData(IList<ExceptionData> items)
        {
            return items == null
                ? new List<PropertyValueOutput>()
                : items.Where(m => m != null).Select(MapExceptionData).ToList();
        }

        private static PropertyValueOutput MapExceptionData(ExceptionData item)
        {
            if (item == null)
            {
                return null;
            }

            return new PropertyValueOutput()
            {
                Label = string.Empty,
                Value = item.Description,
                IsVisible = true,
            };
        }

        private static IList<PropertyValueOutput> MapAlarmData(IList<AlarmData> items)
        {
            return items == null
                ? new List<PropertyValueOutput>()
                : items.Where(m => m != null).Select(MapAlarmData).ToList();
        }

        private static PropertyValueOutput MapAlarmData(AlarmData item)
        {
            if (item == null)
            {
                return null;
            }

            return new PropertyValueOutput()
            {
                Label = item.AlarmType,
                Value = item.AlarmDescription,
                IsVisible = true,
                DisplayState = DisplayState.Error,
            };
        }

        private static IList<PropertyValueOutput> MapRegisterData(RodLiftAssetStatusData data)
        {
            return data?.RegisterData == null
                ? new List<PropertyValueOutput>()
                : data.RegisterData.Where(m => m != null).Select(MapRegisterData).ToList();
        }

        private static PropertyValueOutput MapRegisterData(RegisterData item)
        {
            if (item == null)
            {
                return null;
            }

            var amount = item.Value switch
            {
                AnalogValue analogValue => analogValue.Amount,
                Quantity quantityValue => (float)quantityValue.Amount,
                _ => 0f,
            };

            var unitType = item.Value switch
            {
                Quantity quantityValue => item.Units ?? quantityValue.Unit.Symbol,
                _ => string.Empty,
            };

            if (string.IsNullOrWhiteSpace(unitType) == false)
            {
                unitType = " " + unitType;
            }

            return new PropertyValueOutput()
            {
                Label = ToSentenceCase(item.Description),
                Value = string.IsNullOrWhiteSpace(item.StringValue)
                    ? $"{MathUtility.RoundToSignificantDigits(amount, SignificantDigitCount)}{unitType}"
                    : item.StringValue,
                IsVisible = true,
            };
        }

        private static DisplayState CommunicationDisplayState(string communicationStatus)
        {
            bool IsErrorStatus = GetCommunicationErrorStatuses(communicationStatus);

            if (communicationStatus == "OK" || communicationStatus?.Contains("Running") == true)
            {
                return DisplayState.Ok;
            }

            if (communicationStatus?.Contains("Timeout") == true)
            {
                return DisplayState.Warning;
            }

            return IsErrorStatus ? DisplayState.Error : DisplayState.Normal;
        }

        private static bool GetCommunicationErrorStatuses(string communicationStatus)
        {
            if (communicationStatus?.Contains("Error", StringComparison.OrdinalIgnoreCase) == true
                || communicationStatus?.Contains("fail", StringComparison.OrdinalIgnoreCase) == true
                || communicationStatus?.Contains("Bad Item", StringComparison.OrdinalIgnoreCase) == true
                || communicationStatus?.Contains("EmptyData", StringComparison.OrdinalIgnoreCase) == true
                || communicationStatus?.Contains("illegal data address", StringComparison.OrdinalIgnoreCase) == true
                || communicationStatus?.Contains("exception", StringComparison.OrdinalIgnoreCase) == true
                || communicationStatus?.Contains("RPCServUnvl", StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static IList<PropertyValueOutput> MapGLAnalysisData(GLAnalysisInformationModel glAnalysisData)
        {
            if (glAnalysisData == null)
            {
                return new List<PropertyValueOutput>();
            }

            var result = new List<PropertyValueOutput>()
            {
                new PropertyValueOutput()
                {
                    Label = "Bottomhole temperature",
                    Value = glAnalysisData.BottomholeTemperature,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Gas injection rate",
                    Value = glAnalysisData.GasInjectionRate,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Reservoir pressure",
                    Value = glAnalysisData.ReservoirPressure,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Wellhead temperature",
                    Value = glAnalysisData.WellheadTemperature,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Valve depth",
                    Value = glAnalysisData.ValveDepth,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Flowing bottomhole pressure",
                    Value = glAnalysisData.FlowingBottomholePressure,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Injection rate for tubing critical velocity",
                    Value = glAnalysisData.InjectionRateForTubingCriticalVelocity,
                    IsVisible = true,
                },
            };

            return result;
        }

        private static IList<PropertyValueOutput> MapChemicalInjectionData(ChemicalInjectionInformationModel chemicalInjectionInformationData)
        {
            if (chemicalInjectionInformationData == null)
            {
                return new List<PropertyValueOutput>();
            }

            var result = new List<PropertyValueOutput>()
            {
                new PropertyValueOutput()
                {
                    Label = "Accumulated cycle volume",
                    Value = chemicalInjectionInformationData.AccumulatedCycleVolume,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Current battery volts",
                    Value = chemicalInjectionInformationData.CurrentBatteryVolts,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Current cycle",
                    Value = chemicalInjectionInformationData.CurrentCycle,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Current runmode",
                    Value = chemicalInjectionInformationData.CurrentRunMode,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Current tank level",
                    Value = chemicalInjectionInformationData.CurrentTankLevel,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Cycle frequency",
                    Value = chemicalInjectionInformationData.CycleFrequency,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Daily volume",
                    Value = chemicalInjectionInformationData.DailyVolume,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Injection rate",
                    Value = chemicalInjectionInformationData.InjectionRate,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Pump status",
                    Value = chemicalInjectionInformationData.PumpStatus,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Todays volume",
                    Value = chemicalInjectionInformationData.TodaysVolume,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Target cycle volume",
                    Value = chemicalInjectionInformationData.TargetCycleVolume,
                    IsVisible = true,
                },
            };

            return result;
        }

        private static IList<PropertyValueOutput> MapPlungerLiftData(PlungerLiftDataModel plungerLiftData)
        {
            if (plungerLiftData == null)
            {
                return new List<PropertyValueOutput>();
            }

            var result = new List<PropertyValueOutput>()
            {
                new PropertyValueOutput()
                {
                    Label = "Current mode",
                    Value = plungerLiftData.CurrentMode,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Casing pressure",
                    Value = plungerLiftData.CasingPressure,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Tubing pressure",
                    Value = plungerLiftData.TubingPressure,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Line pressure",
                    Value = plungerLiftData.LinePressure,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Flow rate",
                    Value = plungerLiftData.FlowRate,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Gas temperature",
                    Value = plungerLiftData.GasTemperature,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Gas today accumulation",
                    Value = plungerLiftData.GasTodayAccumulation,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Gas yesterday accumulation",
                    Value = plungerLiftData.GasYesterdayAccumulation,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Oil today accumulation",
                    Value = plungerLiftData.OilTodayAccumulation,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Oil yesterday accumulation",
                    Value = plungerLiftData.OilYesterdayAccumulation,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Water today accumulation",
                    Value = plungerLiftData.WaterTodayAccumulation,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Water yesterday accumulation",
                    Value = plungerLiftData.WaterYesterdayAccumulation,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Efm lifetime accumulation",
                    Value = plungerLiftData.EfmLifetimeAccumulation,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Solar voltage",
                    Value = plungerLiftData.SolarVoltage,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Battery voltage",
                    Value = plungerLiftData.BatteryVoltage,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Controller firmware revision",
                    Value = plungerLiftData.ControllerFirmwareRevision,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Controller firmware version",
                    Value = plungerLiftData.ControllerFirmwareVersion,
                    IsVisible = true,
                },
                new PropertyValueOutput()
                {
                    Label = "Current Load Factor",
                    Value = plungerLiftData.CurrentLoadFactor,
                    IsVisible = true,
                },
            };

            return result;
        }

        private static IList<FacilityTagGroupDataOutput> MapFacilityTagData(FacilityDataModel facilityTagData)
        {
            if (facilityTagData == null)
            {
                return null;
            }

            var data = new List<FacilityTagGroupDataOutput>();

            data = facilityTagData.TagGroups
            .Select(x => new FacilityTagGroupDataOutput
            {
                NodeId = x.NodeId,
                TagGroupNodeId = x.TagGroupNodeId,
                TagGroupName = x.TagGroupName,
                AlarmCount = Convert.ToInt32(x.AlarmCount),
                TagCount = Convert.ToInt32(x.TagCount),
                GroupHostAlarm = x.GroupHostAlarm,
                FacilityTags = MapFacilityTags(x.FacilityTags)
            })
            .OrderBy(x => x.DisplayOrder)
            .ToList();

            return data;
        }

        private static PIDDataOutput MapPIDData(PIDDataModel pIDDataModel)
        {
            if (pIDDataModel == null)
            {
                return null;
            }

            var data = new PIDDataOutput
            {
                PrimaryProcessVariable = pIDDataModel.OverrideScaleFactor,
                PrimarySetpoint = pIDDataModel.PrimarySetpoint,
                PrimaryDeadband = pIDDataModel.PrimaryDeadband,
                PrimaryProportionalGain = pIDDataModel.PrimaryProportionalGain,
                PrimaryIntegral = pIDDataModel.PrimaryIntegral,
                PrimaryDerivative = pIDDataModel.PrimarySetpoint,
                PrimaryScaleFactor = pIDDataModel.PrimaryDeadband,
                PrimaryOutput = pIDDataModel.PrimaryProportionalGain,
                OverrideProcessVariable = pIDDataModel.OverrideProcessVariable,
                OverrideSetpoint = pIDDataModel.OverrideSetpoint,
                OverrideDeadband = pIDDataModel.OverrideDeadband,
                OverrideProportionalGain = pIDDataModel.OverrideProportionalGain,
                OverrideIntegral = pIDDataModel.OverrideIntegral,
                OverrideDerivative = pIDDataModel.OverrideDerivative,
                OverrideScaleFactor = pIDDataModel.OverrideDeadband,
                OverrideOutput = pIDDataModel.OverrideOutput,
            };

            return data;
        }

        private static ValveControlDataOutput MapValveControlData(ValveControlDataModel valveControlData)
        {
            if (valveControlData == null)
            {
                return null;
            }

            return new ValveControlDataOutput
            {
                ValueDP = valveControlData.ValueDP,
                ValueSP = valveControlData.ValueSP,
                ValueFlowRate = valveControlData.ValueFlowRate,
                SetpointDP = valveControlData.SetpointDP,
                SetpointSP = valveControlData.SetpointSP,
                SetpointFlowRate = valveControlData.SetpointFlowRate,
                HiLimitDP = valveControlData.HiLimitDP,
                HiLimitSP = valveControlData.HiLimitSP,
                HiLimitFlowRate = valveControlData.HiLimitFlowRate,
                LoLimitDP = valveControlData.LoLimitDP,
                LoLimitSP = valveControlData.LoLimitSP,
                LoLimitFlowRate = valveControlData.LoLimitFlowRate,
                DeadBandDP = valveControlData.DeadBandDP,
                DeadBandSP = valveControlData.DeadBandSP,
                DeadBandFlowRate = valveControlData.DeadBandFlowRate,
                GainDP = valveControlData.GainDP,
                GainSP = valveControlData.GainSP,
                GainFlowRate = valveControlData.GainFlowRate,
                ControllerModeValue = valveControlData.ControllerModeValue,
                ShutInLeftValue = valveControlData.ShutInLeftValue,
                SPOverrideValue = valveControlData.SPOverrideValue,
            };
        }

        private static List<FacilityTagsDataOutput> MapFacilityTags(List<FacilityTagsModel> facilityTags)
        {
            var result = new List<PropertyValueOutput>();

            return facilityTags.Select(x => new FacilityTagsDataOutput
            {
                Description = x.Description,
                Value = float.TryParse(x.StringValue, out var value) ? MathUtility.RoundToSignificantDigits(value, SignificantDigitCount).ToString() : x.StringValue,
                Units = x.EngUnits,
                Alarm = MapAlarmState(x.AlarmState, x.HostAlarm),
                HostAlarm = string.Empty, // TODO Populate host alarm, x.HostAlarm, above, is Alarm.
                LastUpdated = x.UpdateDate,
                Address = x.Address,
            }).ToList();
        }

        private static PropertyValueOutput MapAlarmState(int alarmState, string hostAlarm)
        {
            string label;
            DisplayState state;

            if (string.IsNullOrEmpty(hostAlarm))
            {
                switch (alarmState)
                {
                    case 1:
                        label = "HI";
                        state = DisplayState.Error;
                        break;
                    case 2:
                        label = "LO";
                        state = DisplayState.Error;
                        break;
                    default:
                        label = "OK";
                        state = DisplayState.Ok;
                        break;
                }
            }
            else
            {
                label = hostAlarm;
                state = DisplayState.Error;
            }

            return new PropertyValueOutput
            {
                Label = label,
                IsVisible = true,
                Value = label,
                DisplayState = state,
            };
        }

        private static bool TryParseNodeAddress(string address,
                                               out Protocol protocol,
                                               out string hostname,
                                               out string opcTypeName,
                                               out ushort? port,
                                               out string rtuAddress,
                                               out int? offset)
        {
            const string KEY_PROTOCOL = "protocol";
            const string KEY_HOST = "host";
            const string KEY_OPC = "opc";
            const string KEY_PORT = "port";
            const string KEY_RTU_A = "rtuA";
            const string KEY_RTU_B = "rtuB";
            const string KEY_OFFSET = "offset";
            const string DELIMITERS = @"\|\+\*";
            const string FORMAT = @"(((?<{0}>[a-zA-Z])" +
                                        @"(?<{1}>[^{7}]*)" +
                                        @"(\*(?<{2}>[^{7}]*))?" +
                                        @"(\|(?<{3}>[^{7}]*))?" +
                                        @"\|(?<{4}>[^{7}]*)?)" +
                                    @"|(?<{5}>[^{7}]*))" +
                                    @"(\+(?<{6}>[^{7}]*))?";

            const string PROTOCOL_MODBUS = "i";
            const string PROTOCOL_MODBUS_TCP = "m";
            const string PROTOCOL_MODBUS_ETHERNET = "e";
            const string PROTOCOL_MODBUS_OPC = "o";

            // Set defaults
            protocol = Protocol.Modbus;
            hostname = null;
            opcTypeName = null;
            port = null;
            rtuAddress = null;
            offset = null;

            if (address == null)
            {
                return false;
            }

            string pattern = string.Format(FORMAT, KEY_PROTOCOL, KEY_HOST, KEY_OPC, KEY_PORT, KEY_RTU_A, KEY_RTU_B, KEY_OFFSET, DELIMITERS);
            Match match = Regex.Match(address, pattern);
            bool success = match.Success;

            if (success)
            {
                Group group = match.Groups[KEY_RTU_B];
                if (group.Success)
                {
                    rtuAddress = group.Value;
                }
                else
                {
                    group = match.Groups[KEY_PROTOCOL];
                    if (group.Success)
                    {
                        switch (group.Value)
                        {
                            case PROTOCOL_MODBUS:
                                protocol = Protocol.Modbus;
                                break;
                            case PROTOCOL_MODBUS_TCP:
                                protocol = Protocol.ModbusTCP;
                                break;
                            case PROTOCOL_MODBUS_ETHERNET:
                                protocol = Protocol.ModbusEthernet;
                                break;
                            case PROTOCOL_MODBUS_OPC:
                                protocol = Protocol.OPC;
                                break;
                            default:
                                success = false;
                                break;
                        }
                    }

                    group = match.Groups[KEY_HOST];
                    if (group.Success)
                    {
                        hostname = group.Value;
                    }

                    group = match.Groups[KEY_OPC];
                    if (group.Success)
                    {
                        opcTypeName = group.Value;
                    }

                    group = match.Groups[KEY_PORT];
                    if (group.Success)
                    {
                        if (ushort.TryParse(group.Value, out var temp))
                        {
                            port = temp;
                        }
                        else
                        {
                            success = false;
                        }
                    }

                    group = match.Groups[KEY_RTU_A];
                    if (group.Success)
                    {
                        rtuAddress = group.Value;
                    }
                }

                group = match.Groups[KEY_OFFSET];
                if (group.Success)
                {
                    if (int.TryParse(group.Value, out int temp))
                    {
                        offset = temp;
                    }
                    else
                    {
                        success = false;
                    }
                }
            }

            return success;
        }

        private static float ValvePcntOpenFromMa(float outputValveMa)
        {
            if (outputValveMa == MA_MINIMUM)
            {
                return 0.0F;
            }
            float returnValue = ((outputValveMa - MA_MINIMUM) / MA_RANGE) * 100.0F;
            return returnValue;
        }

        // Takes a Percent value, performs a bounds check and returns the formatted value.
        private static string BoundsCheck_Percent(string sPercentValue)
        {
            return BoundsCheckSingle(sPercentValue, 0.0F, 100.0F, "F1");
        }

        private static string BoundsCheckSingle(string sValue, float fFloor, float fCeiling, string sFormat)
        {
            if (float.TryParse(sValue, out var fNewValue))
            {
                if (fNewValue < fFloor)
                {
                    return fFloor.ToString(sFormat);
                }
                else if (fNewValue > fCeiling)
                {
                    return fCeiling.ToString(sFormat);
                }
                return MathUtility.RoundToSignificantDigits(fNewValue, SignificantDigitCount).ToString(sFormat);
            }
            return sValue; // Return original value if parsing fails
        }

        private static string ToSentenceCase(string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s[1..].ToLower();
        }

        private static string LoadFactorCalculation(string casing, string tubing, string line)
        {
            string missingValues = string.Empty;

            if (string.IsNullOrEmpty(casing))
            {
                missingValues = "Casing";
            }
            if (string.IsNullOrEmpty(tubing))
            {
                missingValues += ", Tubing";
            }
            if (string.IsNullOrEmpty(line))
            {
                missingValues += ", Line";
            }
            if (!string.IsNullOrEmpty(missingValues))
            {
                if (missingValues[..2] == ", ")
                {
                    missingValues = missingValues.Remove(0, 2);
                }

                missingValues = "Invalid: " + missingValues + " Pressure";
                return missingValues;
            }
            else
            {
                int casingValue = ConvertToInteger(casing.ToLower().Replace("psi", ""));
                int tubingValue = ConvertToInteger(tubing.ToLower().Replace("psi", ""));
                int lineValue = ConvertToInteger(line);

                // 100 * (casing – tubing) / (casing - line)
                int numerator = 100 * (casingValue - tubingValue);
                if (numerator < 0)
                {
                    return "0";
                }

                int denominator = casingValue - lineValue;
                if (denominator <= 0)
                {
                    return "Invalid";
                }

                return (numerator / denominator).ToString("N0");
            }
        }

        private static int ConvertToInteger(string stringValue)
        {
            if (int.TryParse(stringValue, out var integerValue))
            {
                return integerValue;
            }

            return integerValue;
        }

        #endregion

    }

    /// <summary>
    /// Represents a protocol that XSPOC can use to communicate with a node.
    /// </summary>
    public enum Protocol
    {

        /// <summary>
        /// Indicates that the node communicates using the Modbus protocol.
        /// </summary>
        Modbus = 1,

        /// <summary>
        /// Indicates that the node communicates using the Modbus TCP protocol.
        /// </summary>
        ModbusTCP = 2,

        /// <summary>
        /// Indicates that the node communicates using the Modbus Ethernet protocol.
        /// </summary>
        ModbusEthernet = 3,

        /// <summary>
        /// Indicates that the node communicates using the OPC protocol.
        /// </summary>
        OPC = 4,

        /// <summary>
        /// Indicates that the node communicates using the OPC UA protocol.
        /// </summary>
        OPCUA = 5,

        /// <summary>
        /// Indicates that the node communicates using the Weatherford Baker protocol.
        /// </summary>
        Baker = 6,

        /// <summary>
        /// Indicates that the node communicates using the ABB Totalflow G3 protocol.
        /// </summary>
        TotalflowG3 = 7,

        /// <summary>
        /// Indicates that the node communicates using the ABB Totalflow G4 protocol.
        /// </summary>
        TotalflowG4 = 8,

        /// <summary>
        /// Indicates that the node communicates using the EFM Fisher ROC protocol.
        /// </summary>
        EFMFisherROC = 9,

        /// <summary>
        /// Indicates that the node communicates using the EFM Fisher ROC Plus protocol.
        /// </summary>
        EFMFisherROCPlus = 10,

    }

}