using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses.AssetStatus;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs.AssetStatus;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Mapper to map rod lift asset status domain data to rod lift asset status contract data.
    /// </summary>
    public static class AssetStatusMapper
    {
        /// <summary>
        /// Maps the rod lift asset status domain data to rod lift asset status contract data
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="data">The domain data to map the the contract data.</param>
        /// <returns>
        /// The <seealso cref="AssetStatusDataResponse"/>. Returns null if the <paramref name="data"/> is null.
        /// </returns>
        public static AssetStatusDataResponse Map(string correlationId, RodLiftAssetStatusDataOutput data)
        {
            var result = new AssetStatusDataResponse()
            {
                Id = correlationId,
                DateCreated = DateTime.UtcNow,
            };

            if (data == null)
            {
                return result;
            }

            result.Values = new AssetStatusDataValue()
            {
                Alarms = Map(data.Alarms),
                Exceptions = Map(data.Exceptions),
                ImageOverlayItems = Map(data.ImageOverlayItems),
                LastWellTest = Map(data.LastWellTest),
                RodStrings = Map(data.RodStrings),
                StatusRegisters = Map(data.StatusRegisters),
                WellStatusOverview = Map(data.WellStatusOverview),
                DiagramType = data.DiagramType,
                GLAnalysisData = Map(data.GLAnalysisData),
                ChemicalInjectionInformation = Map(data.ChemicalInjectionInformation),
                PlungerLiftData = Map(data.PlungerLiftData),
                FacilityTagData = MapFacilityData(data.FacilityTagData),
                PIDData = MapPIDData(data.PIDData),
                ValveControlData = MapValveControlData(data.ValveControlData),
                RefreshInterval = data.RefreshInterval,
                SmartenLiveUrl = data.SmartenLiveUrl,
            };

            return result;
        }

        #region Private Static Methods

        private static IList<OverlayStatusDataContract> Map(IList<OverlayStatusDataOutput> data)
        {
            return data?.Select(Map).ToList();
        }

        private static IList<PropertyValueContract> Map(IList<PropertyValueOutput> data)
        {
            return data?.Select(Map).ToList();
        }

        private static PropertyValueContract Map(PropertyValueOutput data)
        {
            if (data == null)
            {
                return null;
            }

            return new PropertyValueContract()
            {
                Value = data.Value,
                Label = data.Label,
                IsVisible = data.IsVisible,
                DisplayState = Enum.IsDefined(typeof(DisplayStatusState), (int)data.DisplayState)
                    ? (DisplayStatusState)data.DisplayState
                    : DisplayStatusState.Normal,
            };
        }

        private static OverlayStatusDataContract Map(OverlayStatusDataOutput data)
        {
            if (data == null)
            {
                return null;
            }

            return new OverlayStatusDataContract()
            {
                Value = data.Value,
                Label = data.Label,
                IsVisible = data.IsVisible,
                DisplayState = Enum.IsDefined(typeof(DisplayStatusState), (int)data.DisplayState)
                    ? (DisplayStatusState)data.DisplayState
                    : DisplayStatusState.Normal,
                OverlayField = Enum.IsDefined(typeof(OverlayFields), (int)data.OverlayField)
                    ? (OverlayFields)data.OverlayField
                    : OverlayFields.Unknown,
            };
        }

        private static IList<Responses.FacilityTagGroupDataOutput> MapFacilityData(
            IList<FacilityTagGroupDataOutput> facilityTagData)
        {
            if (facilityTagData == null)
            {
                return null;
            }

            var data = new List<Responses.FacilityTagGroupDataOutput>();

            data = facilityTagData
            .Select(x => new Responses.FacilityTagGroupDataOutput
            {
                NodeId = x.NodeId,
                TagGroupNodeId = x.TagGroupNodeId,
                TagGroupName = x.TagGroupName,
                AlarmCount = x.AlarmCount,
                TagCount = x.TagCount,
                GroupHostAlarm = x.GroupHostAlarm,
                FacilityTags = MapFacilityTags(x.FacilityTags)
            })
            .OrderBy(x => x.DisplayOrder)
            .ToList();

            return data;
        }

        private static Responses.PIDDataOutput MapPIDData(
           PIDDataOutput pidDataOutput)
        {
            if (pidDataOutput == null)
            {
                return null;
            }

            var data = new Responses.PIDDataOutput
            {
                PrimaryProcessVariable = pidDataOutput.OverrideScaleFactor,
                PrimarySetpoint = pidDataOutput.PrimarySetpoint,
                PrimaryDeadband = pidDataOutput.PrimaryDeadband,
                PrimaryProportionalGain = pidDataOutput.PrimaryProportionalGain,
                PrimaryIntegral = pidDataOutput.PrimaryIntegral,
                PrimaryDerivative = pidDataOutput.PrimarySetpoint,
                PrimaryScaleFactor = pidDataOutput.PrimaryDeadband,
                PrimaryOutput = pidDataOutput.PrimaryProportionalGain,
                OverrideProcessVariable = pidDataOutput.OverrideProcessVariable,
                OverrideSetpoint = pidDataOutput.OverrideSetpoint,
                OverrideDeadband = pidDataOutput.OverrideDeadband,
                OverrideProportionalGain = pidDataOutput.OverrideProportionalGain,
                OverrideIntegral = pidDataOutput.OverrideIntegral,
                OverrideDerivative = pidDataOutput.OverrideDerivative,
                OverrideScaleFactor = pidDataOutput.OverrideDeadband,
                OverrideOutput = pidDataOutput.OverrideOutput,
            };

            return data;
        }

        private static Responses.ValveControlDataResponse MapValveControlData(
                       ValveControlDataOutput valveControlData)
        {
            if (valveControlData == null)
            {
                return null;
            }

            var data = new Responses.ValveControlDataResponse();

            data.ValueDP = valveControlData.ValueDP;
            data.ValueSP = valveControlData.ValueSP;
            data.ValueFlowRate = valveControlData.ValueFlowRate;
            data.SetpointDP = valveControlData.SetpointDP;
            data.SetpointSP = valveControlData.SetpointSP;
            data.SetpointFlowRate = valveControlData.SetpointFlowRate;
            data.HiLimitDP = valveControlData.HiLimitDP;
            data.HiLimitSP = valveControlData.HiLimitSP;
            data.HiLimitFlowRate = valveControlData.HiLimitFlowRate;
            data.LoLimitDP = valveControlData.LoLimitDP;
            data.LoLimitSP = valveControlData.LoLimitSP;
            data.LoLimitFlowRate = valveControlData.LoLimitFlowRate;
            data.DeadBandDP = valveControlData.DeadBandDP;
            data.DeadBandSP = valveControlData.DeadBandSP;
            data.DeadBandFlowRate = valveControlData.DeadBandFlowRate;
            data.GainDP = valveControlData.GainDP;
            data.GainSP = valveControlData.GainSP;
            data.GainFlowRate = valveControlData.GainFlowRate;
            data.ControllerModeValue = valveControlData.ControllerModeValue;
            data.ShutInLeftValue = valveControlData.ShutInLeftValue;
            data.SPOverrideValue = valveControlData.SPOverrideValue;

            return data;
        }

        private static List<Responses.FacilityTagsDataOutput> MapFacilityTags(List<FacilityTagsDataOutput> facilityTags)
        {
            return facilityTags.Select(x => new Responses.FacilityTagsDataOutput
            {
                Address = x.Address,
                Description = x.Description,
                Alarm = MapAlarm(x.Alarm),
                Units = x.Units,
                LastUpdated = x.LastUpdated,
                Value = x.Value,
                HostAlarm = x.HostAlarm
            }).ToList();
        }

        private static PropertyValueContract MapAlarm(PropertyValueOutput alarm)
        {
            return new PropertyValueContract
            {
                Label = alarm.Label,
                Value = alarm.Value,
                DisplayState = Enum.IsDefined(typeof(DisplayStatusState), (int)alarm.DisplayState)
                    ? (DisplayStatusState)alarm.DisplayState
                    : DisplayStatusState.Normal,
                IsVisible = alarm.IsVisible,
            };
        }

        #endregion

    }
}