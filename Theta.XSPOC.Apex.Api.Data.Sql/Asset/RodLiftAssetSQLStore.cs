using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Asset;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.Asset;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Asset
{
    /// <summary>
    /// This is the SQL implementation that defines the methods for the rod lift asset store.
    /// </summary>
    public class RodLiftAssetSQLStore : IAssetStore
    {

        #region Private Fields

        private readonly IThetaDbContextFactory<XspocDbContext> _thetaDbContextFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="RodLiftAssetSQLStore"/> using the provided
        /// <paramref name="thetaDbContextFactory"/>.
        /// </summary>
        /// <param name="thetaDbContextFactory">The theta db context factory used to get a db context.</param>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="thetaDbContextFactory"/> is null.
        /// </exception>
        public RodLiftAssetSQLStore(IThetaDbContextFactory<XspocDbContext> thetaDbContextFactory)
        {
            _thetaDbContextFactory =
                thetaDbContextFactory ?? throw new ArgumentNullException(nameof(thetaDbContextFactory));
        }

        #endregion

        #region IAssetRepository Implementation

        /// <summary>
        /// Gets the asset status data for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the status data.</param>
        /// <returns>
        /// A <seealso cref="RodLiftAssetStatusCoreData"/> that contains the asset status data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is the default GUID then <c>null</c> is returned.
        /// </returns>
        public async Task<RodLiftAssetStatusCoreData> GetAssetStatusDataAsync(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {

                var node = await context.NodeMasters
                    .GroupJoin(context.Company, n => n.CompanyId, c => c.Id,
                    (nodemaster, company) => new
                    {
                        NodeMaster = nodemaster,
                        Company = company,
                    })
                    .SelectMany(sp => sp.Company.DefaultIfEmpty(),
                    (l, r) => new
                    {
                        l.NodeMaster,
                        Company = r,
                    })
                    .Select(m => new NodeProjected()
                    {
                        AssetGUID = m.NodeMaster.AssetGuid,
                        NodeId = m.NodeMaster.NodeId,
                        PocType = m.NodeMaster.PocType,
                        CustomerGUID = m.Company.CustomerGUID,
                    }).FirstOrDefaultAsync(x => x.AssetGUID == assetId);

                if (node == null)
                {
                    return null;
                }

                var rodMotorSettings = context.RodMotorSettings;

                var coreRecordData = context.NodeMasters.GroupJoin(context.PocType, l => l.PocType, r => r.PocType,
                        (node, poctype) => new
                        {
                            node,
                            poctype,
                        })
                    .SelectMany(m => m.poctype.DefaultIfEmpty(), (node, poctype) => new
                    {
                        node.node,
                        poctype,
                    })
                    .GroupJoin(context.WellDetails, l => l.node.NodeId, r => r.NodeId,
                        (leftWithWellDetails, wellDetails) => new
                        {
                            leftWithWellDetails,
                            wellDetails,
                        })
                    .SelectMany(m => m.wellDetails.DefaultIfEmpty(), (leftWithWellDetails, wellDetails) => new
                    {
                        leftWithWellDetails.leftWithWellDetails,
                        wellDetails,
                    })
                    .GroupJoin(context.XDIAGResultLast, l => l.leftWithWellDetails.node.NodeId, r => r.NodeId,
                        (leftWithXDIAGResultLast, xdiagResultLast) => new
                        {
                            leftWithXDIAGResultLast,
                            xdiagResultLast,
                        })
                    .SelectMany(m => m.xdiagResultLast.DefaultIfEmpty(), (leftWithXDIAGResultLast, xdiagResultLast) =>
                        new
                        {
                            leftWithXDIAGResultLast.leftWithXDIAGResultLast,
                            xdiagResultLast,
                        })
                    .GroupJoin(context.PumpingUnits, l => l.leftWithXDIAGResultLast.wellDetails.PumpingUnitId,
                        r => r.UnitId,
                        (leftWithPumpingUnit, pumpingUnit) => new
                        {
                            leftWithPumpingUnit,
                            pumpingUnit,
                        })
                    .SelectMany(m => m.pumpingUnit.DefaultIfEmpty(), (leftWithPumpingUnit, pumpingUnit) => new
                    {
                        leftWithPumpingUnit.leftWithPumpingUnit,
                        pumpingUnit,
                    })
                    .GroupJoin(context.PumpingUnitManufacturer, l => l.pumpingUnit.ManufacturerId,
                        r => r.ManufacturerAbbreviation,
                        (leftWithManufacturer, manufacturer) => new
                        {
                            leftWithManufacturer,
                            manufacturer,
                        })
                    .SelectMany(m => m.manufacturer.DefaultIfEmpty(), (leftWithManufacturer, manufacturer) => new
                    {
                        leftWithManufacturer.leftWithManufacturer,
                        manufacturer,
                    })
                    .GroupJoin(context.CustomPumpingUnits,
                        l => l.leftWithManufacturer.leftWithPumpingUnit.leftWithXDIAGResultLast.wellDetails
                            .PumpingUnitId, r => r.Id,
                        (leftWithCustomPumpingUnit, customPumpingUnit) => new
                        {
                            leftWithCustomPumpingUnit,
                            customPumpingUnit,
                        }).SelectMany(m => m.customPumpingUnit.DefaultIfEmpty(),
                        (leftWithCustomPumpingUnit, customPumpingUnit) => new
                        {
                            leftWithCustomPumpingUnit.leftWithCustomPumpingUnit,
                            customPumpingUnit,
                        })
                    .GroupJoin(rodMotorSettings, l => l.leftWithCustomPumpingUnit.leftWithManufacturer
                            .leftWithPumpingUnit
                            .leftWithXDIAGResultLast.wellDetails.MotorSettingId, r => r.Id,
                        (leftWithMotorSetting, motorSetting) => new
                        {
                            leftWithMotorSetting,
                            motorSetting,
                        }).SelectMany(m => m.motorSetting.DefaultIfEmpty(), (leftWithMotorSetting, motorSetting) => new
                        {
                            leftWithMotorSetting.leftWithMotorSetting,
                            motorSetting,
                        })
                    .GroupJoin(context.RodMotorKinds, l => l.leftWithMotorSetting.leftWithCustomPumpingUnit
                            .leftWithManufacturer
                            .leftWithPumpingUnit.leftWithXDIAGResultLast.wellDetails.MotorTypeId, r => r.Id,
                        (leftWithMotorKind, motorKind) => new
                        {
                            leftWithMotorKind,
                            motorKind,
                        }).SelectMany(m => m.motorKind.DefaultIfEmpty(), (leftWithMotorKind, motorKind) => new
                        {
                            leftWithMotorKind.leftWithMotorKind,
                            motorKind,
                        })
                    .GroupJoin(context.ESPAnalysisResults.OrderByDescending(x => x.TestDate)
                            .Where(y => y.NodeId == node.NodeId), l => l.leftWithMotorKind
                            .leftWithMotorSetting.leftWithCustomPumpingUnit
                            .leftWithManufacturer.leftWithPumpingUnit.leftWithXDIAGResultLast.wellDetails.NodeId,
                        r => r.NodeId,
                        (leftWithESPResult, espResult) => new
                        {
                            leftWithESPResult,
                            espResult
                        })
                    .SelectMany(m => m.espResult.DefaultIfEmpty(), (leftWithESPResult, espResult) => new
                    {
                        leftWithESPResult.leftWithESPResult,
                        espResult,
                    })
                    .Select(m => new
                    {
                        Node = m.leftWithESPResult.leftWithMotorKind.leftWithMotorSetting
                            .leftWithCustomPumpingUnit.leftWithManufacturer.leftWithPumpingUnit.leftWithXDIAGResultLast
                            .leftWithWellDetails.node,
                        WellDetails = m.leftWithESPResult.leftWithMotorKind.leftWithMotorSetting
                            .leftWithCustomPumpingUnit.leftWithManufacturer.leftWithPumpingUnit
                            .leftWithXDIAGResultLast.wellDetails,
                        PocType = m.leftWithESPResult.leftWithMotorKind.leftWithMotorSetting
                            .leftWithCustomPumpingUnit.leftWithManufacturer.leftWithPumpingUnit
                            .leftWithXDIAGResultLast.leftWithWellDetails.poctype,
                        XDiagResultsLast = m.leftWithESPResult.leftWithMotorKind.leftWithMotorSetting
                            .leftWithCustomPumpingUnit.leftWithManufacturer.leftWithPumpingUnit
                            .xdiagResultLast,
                        PumpingUnit = m.leftWithESPResult.leftWithMotorKind.leftWithMotorSetting.leftWithCustomPumpingUnit
                            .leftWithManufacturer.pumpingUnit,
                        CustomPumpingUnit = m.leftWithESPResult
                            .leftWithMotorKind.leftWithMotorSetting.customPumpingUnit,
                        Manufacturer = m.leftWithESPResult.leftWithMotorKind.leftWithMotorSetting.leftWithCustomPumpingUnit
                            .manufacturer,
                        MotorSetting = m.leftWithESPResult.leftWithMotorKind.motorSetting,
                        ESPResult = m.espResult,
                        MotorKind = m.leftWithESPResult.motorKind,

                    })
                    .Select(m => new RodLiftAssetStatusCoreRecordData()
                    {
                        CommunicationPercentageYesterday = m.Node.PercentCommunicationsYesterday,
                        YesterdayRuntimePercentage = m.Node.YesterdayRuntimePercent,
                        TodayRuntimePercentage = m.Node.TodayRuntimePercent,
                        NodeAddress = m.Node.Node,
                        IsNodeEnabled = m.Node.Enabled,
                        CommunicationStatus = m.Node.CommStatus,
                        RunStatus = m.Node.RunStatus,
                        TimeInState = m.Node.TimeInState,
                        LastGoodScan = m.Node.LastGoodScanTime,
                        TzOffset = m.Node.Tzoffset,
                        TzDaylight = m.Node.Tzdaylight,
                        FirmwareVersion = m.Node.FirmwareVersion,
                        ApiPort = m.Node.Apiport,
                        PortId = m.Node.PortId,
                        PumpDepth = m.WellDetails != null ? m.WellDetails.PumpDepth : null,
                        TubingPressure = m.WellDetails != null ? m.WellDetails.TubingPressure : null,
                        CasingPressure = m.WellDetails != null ? m.WellDetails.CasingPressure : null,
                        RateAtTest = m.WellDetails != null ? m.WellDetails.GrossRateAtTest : null,
                        StrokesPerMinute = m.WellDetails != null ? m.WellDetails.StrokesPerMinute : null,
                        PlungerDiameter = m.WellDetails != null ? m.WellDetails.PlungerDiameter : null,
                        PrimeMoverType = m.WellDetails != null ? m.WellDetails.PrimeMoverType : null,
                        PocTypeDescription = m.PocType != null ? m.PocType.Description : null,
                        PocType = m.Node.PocType,
                        PumpEfficiencyPercentage = m.XDiagResultsLast != null ? m.XDiagResultsLast.PumpEfficiencyPercentage : null,
                        GearBoxLoadPercentage = m.XDiagResultsLast != null ? m.XDiagResultsLast.GearBoxLoadPercentage : null,
                        StructuralLoading = m.XDiagResultsLast != null ? m.XDiagResultsLast.UnitStructuralLoad : null,
                        MaxRodLoading = m.XDiagResultsLast != null ? m.XDiagResultsLast.MaxRodLoad : null,
                        PumpEfficiency = m.XDiagResultsLast != null ? m.XDiagResultsLast.PumpEfficiency : null,
                        StrokeLength = m.WellDetails != null ? m.WellDetails.StrokeLength : null,
                        FluidLevel = m.WellDetails != null ? m.WellDetails.FluidLevel : null,
                        APIDesignation = m.PumpingUnit != null
                                            ? m.PumpingUnit.APIDesignation
                                            : (m.CustomPumpingUnit != null ? m.CustomPumpingUnit.APIDesignation : null),
                        PumpingUnitName = m.PumpingUnit != null ? m.PumpingUnit.UnitName : null,
                        LinePressure = context.PlungerLiftDataHistory
                            .Where(x => x.NodeId == node.NodeId)
                            .Select(x => x.LinePressure)
                            .SingleOrDefault(),
                        PumpingUnitManufacturer = m.Manufacturer != null
                                            ? m.Manufacturer.ManufacturerAbbreviation
                                            : (m.CustomPumpingUnit != null ? m.CustomPumpingUnit.Manufacturer : null),
                        RatedHorsePower = m.WellDetails != null && m.WellDetails.MotorSettingId != null
                            ? (m.MotorSetting != null ? m.MotorSetting.RatedHP : null)
                            : (m.WellDetails != null && m.WellDetails.MotorSizeId == null
                                ? null
                                : rodMotorSettings.Count(s => s.MotorSizeId == m.WellDetails.MotorSizeId) > 1
                                    ? null
                                    : rodMotorSettings.FirstOrDefault(s => s.MotorSizeId == m.WellDetails.MotorSizeId) != null ? rodMotorSettings.FirstOrDefault(s => s.MotorSizeId == m.WellDetails.MotorSizeId).RatedHP : null),
                        PumpFillage = m.Node.PumpFillage != null
                                        ? m.Node.PumpFillage
                                        : (m.XDiagResultsLast != null ? m.XDiagResultsLast.FillagePercentage : null),
                        CalculatedFluidLevelAbovePump = m.ESPResult != null ? m.ESPResult.CalculatedFluidLevelAbovePump : null,
                        MotorTypeId = m.WellDetails != null ? m.WellDetails.MotorTypeId : null,
                        MotorKindName = m.MotorKind != null ? m.MotorKind.Name : null,
                        MotorLoad = m.XDiagResultsLast != null ? m.XDiagResultsLast.MotorLoad : null,
                        LastWellTestDate = m.WellDetails != null ? m.WellDetails.LastWellTestDate : null,
                        GasRate = m.WellDetails != null ? m.WellDetails.GasRate : null,
                        GrossRate = m.WellDetails != null ? m.WellDetails.GrossRate : null,
                        WaterCut = m.WellDetails != null ? m.WellDetails.WaterCut : null,
                        NodeId = m.Node.NodeId,
                        AssetGUID = m.Node.AssetGuid,
                        ESPResultTestDate = m.ESPResult != null ? m.ESPResult.TestDate : null,
                        ApplicationId = m.Node.ApplicationId,
                        CustomerGUID = node.CustomerGUID != null ? node.CustomerGUID : Guid.Empty,
                        PumpingUnitTypeId = m.Manufacturer != null
                                            ? m.Manufacturer.UnitTypeId.ToString()
                                            : (m.CustomPumpingUnit != null ? m.CustomPumpingUnit.Type.ToString() : null),
                        PumpType = m.WellDetails != null && m.WellDetails.PumpType != null ? m.WellDetails.PumpType.ToString() : null,
                    }).FirstOrDefault(m => m.AssetGUID == assetId);

                var result = Map(coreRecordData);

                return result;
            }

        }

        /// <summary>
        /// Gets the list of rod string for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the rod strings.</param>
        /// <returns>
        /// A <seealso cref="IList{RodStrings}"/> that contains the rod strings for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        public async Task<IList<RodStringData>> GetRodStringAsync(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var rodStrings = context.RodStrings.Join(context.RodStringGrades, l => l.RodGradeId, r => r.RodGradeId,
                        (rodString, rodStringGrade) => new
                        {
                            rodString,
                            rodStringGrade,
                        }).Join(context.RodStringSizes, l => l.rodString.RodSizeId, r => r.RodSizeId,
                        (rodString, rodStringSize) => new
                        {
                            rodString,
                            rodStringSize,
                        })
                    .Join(context.NodeMasters, l => l.rodString.rodString.NodeId, r => r.NodeId, (rods, node) => new
                    {
                        rods,
                        node,
                    })
                    .Where(m => m.node.AssetGuid == assetId)
                    .OrderBy(m => m.rods.rodString.rodString.RodNum).Select(m => new RodStringRecordData()
                    {
                        RodStringPositionNumber = m.rods.rodString.rodString.RodNum,
                        RodStringGradeName = m.rods.rodString.rodString.Grade,
                        Diameter = m.rods.rodString.rodString.Diameter,
                        Length = m.rods.rodString.rodString.Length,
                        RodStringSizeDisplayName = m.rods.rodStringSize.DisplayName,
                    }).Select(m => new RodStringData()
                    {
                        Length = m.Length == null ? null : Length.FromFeet(m.Length.Value),
                        Diameter = m.Diameter,
                        RodStringGradeName = m.RodStringGradeName,
                        RodStringPositionNumber = m.RodStringPositionNumber,
                        RodStringSizeDisplayName = m.RodStringSizeDisplayName,
                        UnitString = Length.Foot.Symbol,
                    });

                return rodStrings.ToList();
            }
        }

        /// <summary>
        /// Gets the list of esp motor infomation for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the esp motors.</param>
        /// <returns>
        /// A <seealso cref="IList{RodStrings}"/> that contains the ESP Motors for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        public async Task<ESPMotorInformationModel> GetESPMotorInformation(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var node = await context.NodeMasters
                    .Select(m => new NodeProjected()
                    {
                        AssetGUID = m.AssetGuid,
                        NodeId = m.NodeId,
                    }).FirstOrDefaultAsync(x => x.AssetGUID == assetId);

                if (node == null)
                {
                    return null;
                }

                var espMotorsInfo = context.ESPWellDetails
                         .Where(wd => wd.NodeId == node.NodeId)
                         .GroupJoin(context.ESPCables, wd => wd.CableId, sc => sc.CableId, (wd, cableGroup) => new { wd, cableGroup })
                         .SelectMany(x => x.cableGroup.DefaultIfEmpty(), (x, sc) => new { x.wd, sc })
                         .GroupJoin(context.ESPMotorLeads, x => x.wd.MotorLeadId, ml => ml.MotorLeadId, (x, motorLeadGroup) => new { x.wd, x.sc, motorLeadGroup })
                         .SelectMany(x => x.motorLeadGroup.DefaultIfEmpty(), (x, ml) => new { x.wd, x.sc, ml })
                         .GroupJoin(context.ESPSeals, x => x.wd.SealId, ss => ss.SealId, (x, sealGroup) => new { x.wd, x.sc, x.ml, sealGroup })
                         .SelectMany(x => x.sealGroup.DefaultIfEmpty(), (x, ss) => new { x.wd, x.sc, x.ml, ss })
                         .GroupJoin(context.ESPMotors, x => x.wd.MotorId, sm => sm.MotorId, (x, motorGroup) => new { x.wd, x.sc, x.ml, x.ss, motorGroup })
                         .SelectMany(x => x.motorGroup.DefaultIfEmpty(), (x, sm) => new { x.wd, x.sc, x.ml, x.ss, sm })
                         .GroupJoin(context.ESPManufacturers, x => x.sm.ManufacturerId, smf => smf.ManufacturerId, (x, manufacturerGroup) => new { x.wd, x.sc, x.ml, x.ss, x.sm, manufacturerGroup })
                         .SelectMany(x => x.manufacturerGroup.DefaultIfEmpty(), (x, smf) => new { x.wd, x.sc, x.ml, x.ss, x.sm, smf })
                         .Select(x => new ESPMotorInformationModel
                         {
                             Cable = x.sc.Model + (!string.IsNullOrEmpty(x.sc.Description) ? " (" + x.sc.Description + ")" : string.Empty),
                             CableDescription = x.sc.CableDescription,
                             CableType = x.sc.CableType,
                             CableSeries = x.sc.Series,
                             MotorLead = x.ml.Model + (!string.IsNullOrEmpty(x.ml.Description) ? " (" + x.ml.Description + ")" : string.Empty),
                             Seal = x.ss.Model + (!string.IsNullOrEmpty(x.ss.Description) ? " (" + x.ss.Description + ")" : string.Empty),
                             SealSeries = x.ss.Series,
                             SealModel = x.ss.SealModel,
                             Motor = x.sm.Model + (!string.IsNullOrEmpty(x.sm.Description) ? " (" + x.sm.Description + ")" : string.Empty),
                             MotorSeries = x.sm.Series,
                             MotorLeadSeries = x.ml.Series,
                             MotorLeadType = x.ml.MotorLeadType,
                             MotorLeadDescription = x.ml.MotorLeadDescription,
                             MotorModel = x.sm.MotorModel
                         }).FirstOrDefault();

                if (espMotorsInfo == null)
                {
                    return null;
                }

                var espPumpConfigs = from wp in context.ESPWellPumps
                                     join p in context.ESPPumps
                                     on wp.ESPPumpId equals p.ESPPumpId
                                     where wp.ESPWellId == node.NodeId
                                     orderby wp.OrderNumber
                                     select new { wp };

                var pumps = context.ESPWellPumps
                    .Join(context.ESPPumps, p => p.ESPPumpId, pump => pump.ESPPumpId,
                    (p, pump) => new { p, pump })
                    .Join(context.ESPManufacturers, p => p.pump.ManufacturerId, manufacture => manufacture.ManufacturerId,
                    (p, manufacturer) => new { p, manufacturer })
                    .Where(x => x.p.p.ESPWellId == node.NodeId)
                    .OrderBy(x => x.p.p.OrderNumber)
                    .Select((x) => new PumpConfigurationModel
                    {
                        Pump = x.manufacturer.Manufacturer + " " + x.p.pump.Series + " " + x.p.pump.PumpModel
                        + " - " + x.p.p.NumberOfStages.ToString() + " " + "Stages",
                    }).ToList();

                espMotorsInfo.PumpConfigurations = pumps;

                var cables = context.ESPWellCables
                    .Join(context.ESPCables, c => c.ESPCableId, cable => cable.CableId,
                    (c, cable) => new { c, cable })
                    .Where(x => x.c.NodeId == node.NodeId)
                    .OrderBy(x => x.c.OrderNumber)
                    .Select((x) => new CableModel
                    {
                        Name = x.cable.Model + " " + "(" +
                        (x.cable.CableDescription != null ? string.Format("{0} {1} {2}", x.cable.Series, x.cable.CableType, x.cable.CableDescription)
                        : x.cable.Description) + ")",
                    }).ToList();

                espMotorsInfo.Cables = cables;

                var motorLeads = context.ESPWellMotorLead
                    .Join(context.ESPMotorLeads, c => c.ESPMotorLeadId, motorleads => motorleads.MotorLeadId,
                    (ml, motorleads) => new { ml, motorleads })
                    .Where(x => x.ml.NodeId == node.NodeId)
                    .OrderBy(x => x.ml.OrderNumber)
                    .Select((x) => new MotorLeadModel
                    {
                        Name = x.motorleads.Model + " " + "(" +
                        (x.motorleads.MotorLeadType != null ? string.Format("{0} {1} {2}", x.motorleads.Series, x.motorleads.MotorLeadType, x.motorleads.MotorLeadDescription)
                        : x.motorleads.Description) + ")",
                    }).ToList();

                espMotorsInfo.MotorLeads = motorLeads;

                var motors = context.ESPWellMotors
                    .Join(context.ESPMotors, c => c.ESPMotorId, motors => motors.MotorId,
                    (m, motors) => new { m, motors })
                    .Where(x => x.m.NodeId == node.NodeId)
                    .OrderBy(x => x.m.OrderNumber)
                    .Select((x) => new
                    {
                        Current = Current.FromAmperes((float)x.motors.Amps),
                        Power = Power.FromHorsepower((float)x.motors.HP),
                        Voltage = Voltage.FromVolts((float)x.motors.Volts),
                        x.motors.Model,
                        x.motors.Series,
                        x.motors.MotorModel,
                        RatedTemperature = x.motors.RatedTemperature != null ?
                                Temperature.FromDegreesFahrenheit((float)x.motors.RatedTemperature.Value) : null,
                        x.motors.Description
                    }).ToList();

                var espwellMotors = new List<MotorModel>();

                foreach (var motor in motors)
                {
                    var espMotor = new MotorModel();

                    string hp = motor.Power?.ToString()?.ToUpper();
                    string amp = motor.Current?.ToString();
                    string voltage = motor.Voltage?.ToString();
                    string series = motor?.Series;

                    if (string.IsNullOrWhiteSpace(hp) || string.IsNullOrWhiteSpace(amp) || string.IsNullOrWhiteSpace(voltage)
                            || string.IsNullOrWhiteSpace(series))
                    {
                        espMotor.Name = motor.Series != null &&
                            motor.MotorModel != null ? string.Format("{0} {1}", motor.Series, motor.MotorModel) : motor.Description;
                    }
                    else
                    {
                        hp = hp.Replace(" ", string.Empty);
                        amp = amp.Replace(" ", string.Empty);
                        voltage = voltage.Replace(" ", string.Empty);

                        espMotor.Name = $"{series} {hp} {amp} {voltage}";
                    }
                    espMotor.Name = motor.Model == null ? espMotor.Name : motor.Model + " (" + espMotor.Name + ")";
                    espwellMotors.Add(espMotor);
                }

                espMotorsInfo.Motors = espwellMotors;

                var seals = context.ESPWellSeals
                    .Join(context.ESPSeals, s => s.ESPSealId, seal => seal.SealId,
                    (s, seal) => new { s, seal })
                    .Where(x => x.s.NodeId == node.NodeId)
                    .OrderBy(x => x.s.OrderNumber)
                    .Select((x) => new SealModel
                    {
                        Name = x.seal.Model + " " + "(" +
                        (x.seal.SealModel != null ? string.Format("{0} {1}", x.seal.Series, x.seal.SealModel)
                                               : x.seal.Description) + ")",
                    }).ToList();

                espMotorsInfo.Seals = seals;

                return espMotorsInfo;
            }
        }

        /// <summary>
        /// Gets the list of esp pump infomation for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the esp motors.</param>
        /// <returns>
        /// A <seealso cref="ESPPumpInformationModel"/> that contains the ESP Motors for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        public async Task<ESPPumpInformationModel> GetESPPumpInformation(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var node = await context.NodeMasters
                     .Select(m => new NodeProjected()
                     {
                         AssetGUID = m.AssetGuid,
                         NodeId = m.NodeId,
                     }).FirstOrDefaultAsync(x => x.AssetGUID == assetId);

                if (node == null)
                {
                    return null;
                }

                var espPump = (from WD in context.ESPWellDetails
                               where WD.NodeId == node.NodeId
                               join P in context.ESPPumps on WD.ESPPumpId equals P.ESPPumpId into pumpGroup
                               from P in pumpGroup.DefaultIfEmpty()
                               join M in context.ESPManufacturers on P.ManufacturerId equals M.ManufacturerId
                               into manufacturerGroup
                               from M in manufacturerGroup.DefaultIfEmpty()
                               select new ESPPumpInformationModel
                               {
                                   Pump = (!string.IsNullOrEmpty(P.Series) && !string.IsNullOrEmpty(P.PumpModel))
                                       ? string.Format("{0} {1}", P.Series.ToString(), P.PumpModel.ToString())
                                       : P.Pump,
                                   Manufacturer = M.Manufacturer
                               }).FirstOrDefault();

                return espPump;
            }
        }

        /// <summary>
        /// Gets the gl analysis infomation for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the esp motors.</param>
        /// <returns>
        /// A <seealso cref="GLAnalysisInformationModel"/> that contains the GL Analysis Details for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        public async Task<GLAnalysisInformationModel> GetGasLiftAnalysisInformation(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var node = await context.NodeMasters
                     .Select(m => new NodeProjected()
                     {
                         AssetGUID = m.AssetGuid,
                         NodeId = m.NodeId,
                     }).FirstOrDefaultAsync(x => x.AssetGUID == assetId);

                if (node == null)
                {
                    return null;
                }

                var glAnalysisInfomation = (from ar in context.GLAnalysisResults
                                            join wt in context.WellTest
                                            on new { ar.NodeId, ar.TestDate } equals new { wt.NodeId, wt.TestDate }
                                            join vs in context.GLValveStatus
                                            on ar.Id equals vs.GLAnalysisResultId
                                            join wv in context.GLWellValve
                                            on vs.GLWellValveId equals wv.Id
                                            where ar.NodeId == node.NodeId &&
                                                  ar.Success && vs.IsInjectingGas == true
                                            orderby ar.TestDate descending
                                            select new GLAnalysisInformationModel
                                            {
                                                GasInjectionRate = ar.InjectedGasRate.ToString() ?? string.Empty,
                                                WellheadTemperature = ar.WellheadTemperature.ToString() ?? string.Empty,
                                                BottomholeTemperature = ar.BottomholeTemperature.ToString() ?? string.Empty,
                                                ReservoirPressure = ar.ReservoirPressure.ToString() ?? string.Empty,
                                                FlowingBottomholePressure = ar.FlowingBhp.ToString() ?? string.Empty,
                                                ValveDepth = wv.MeasuredDepth.ToString() ?? string.Empty,
                                                InjectionRateForTubingCriticalVelocity =
                                                vs.InjectionRateForTubingCriticalVelocity.ToString() ?? string.Empty
                                            }).FirstOrDefault();

                return glAnalysisInfomation;
            }
        }

        /// <summary>
        /// Gets the chemical injection infomation for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to get the esp motors.</param>
        /// <returns>
        /// A <seealso cref="ChemicalInjectionInformationModel"/> that contains the Chemical Injection Details for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        public async Task<ChemicalInjectionInformationModel> GetChemicalInjectionInformation(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var node = await context.NodeMasters
                     .Select(m => new NodeProjected()
                     {
                         AssetGUID = m.AssetGuid,
                         NodeId = m.NodeId,
                     }).FirstOrDefaultAsync(x => x.AssetGUID == assetId);

                if (node == null)
                {
                    return null;
                }

                var query = from d in context.CurrentRawScans
                            join p in context.Parameters
                            on new { d.Address, POCType = 390 } equals new { p.Address, POCType = 390 } into ps
                            from p in ps.DefaultIfEmpty()
                            join s in context.States
                            on new { StateId = (int)p.StateId, Value = (float)d.Value }
                            equals new { s.StateId, Value = (float)s.Value } into ss
                            from s in ss.DefaultIfEmpty()
                            where d.NodeId == node.NodeId &&
                                  new[] { 10000, 10002, 40015, 40025, 40102, 40106, 40108, 40110, 40112, 40130, 40132, 40138, 40140 }
                                  .Contains(d.Address)
                            select new
                            {
                                d.Address,
                                FloatValue = d.Value,
                                StringValue = s.Text ?? d.Value.ToString()
                            };
                var result = query.ToList();

                var chemicalInjectionInfo = new ChemicalInjectionInformationModel();
                float? cycleCountdown = null, cycleDuration = null;

                foreach (var data in result)
                {
                    switch (data.Address)
                    {
                        case 10000:
                            chemicalInjectionInfo.CurrentRunMode = data.StringValue.ToString();
                            break;
                        case 10002:
                            chemicalInjectionInfo.PumpStatus = data.StringValue.ToString();
                            break;
                        case 40015:
                            chemicalInjectionInfo.CycleFrequency = data.StringValue.ToString();
                            break;
                        case 40025:
                            chemicalInjectionInfo.CurrentCycle = data.StringValue.ToString();
                            break;
                        case 40102:
                            chemicalInjectionInfo.DailyVolume = data.StringValue.ToString();
                            break;
                        case 40106:
                            chemicalInjectionInfo.InjectionRate = data.StringValue.ToString();
                            break;
                        case 40108:
                            chemicalInjectionInfo.AccumulatedCycleVolume = data.StringValue.ToString();
                            break;
                        case 40110:
                            chemicalInjectionInfo.TodaysVolume = data.StringValue.ToString();
                            break;
                        case 40112:
                            chemicalInjectionInfo.CurrentTankLevel = data.StringValue.ToString();
                            break;
                        case 40130:
                            cycleCountdown = data.FloatValue;
                            break;
                        case 40132:
                            cycleDuration = data.FloatValue;
                            break;
                        case 40138:
                            chemicalInjectionInfo.TargetCycleVolume = data.StringValue.ToString();
                            break;
                        case 40140:
                            chemicalInjectionInfo.CurrentBatteryVolts = data.StringValue.ToString();
                            break;
                        default:
                            break;
                    }

                    if (cycleDuration.HasValue)
                    {
                        chemicalInjectionInfo.IsPumpOn = true;
                        chemicalInjectionInfo.CycleTime = cycleDuration.ToString();
                    }
                    else if (cycleCountdown.HasValue)
                    {
                        chemicalInjectionInfo.IsPumpOn = false;
                        chemicalInjectionInfo.CycleTime = cycleCountdown.ToString();
                    }
                    else
                    {
                        chemicalInjectionInfo.IsPumpOn = false;
                        chemicalInjectionInfo.CycleTime = string.Empty;
                    }
                }

                return chemicalInjectionInfo;
            }
        }

        /// <summary>
        /// Gets the curr raw scan data information for the provided <paramref name="nodeId"/>.
        /// </summary>
        /// <param name="nodeId">The asset id used to get the curr raw scan data information.</param>
        /// <param name="registerList">The register list.</param>
        /// <returns>
        /// A dictionary containing the address and values for the <paramref name="registerList"/>.
        /// If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        public Dictionary<string, float?> GetCurrRawScanDataItems(string nodeId, List<int> registerList)
        {
            using (var context = _thetaDbContextFactory.GetContext())
            {
                var query = from d in context.CurrentRawScans
                            where d.NodeId == nodeId &&
                                   registerList.Contains(d.Address)
                            select new
                            {
                                d.Address,
                                d.Value
                            };
                var result = query.ToList();

                return result.ToDictionary(c => c.Address.ToString(), c => c.Value);
            }
        }

        /// <summary>
        /// Gets the curr raw scan data information for the provided <paramref name="nodeId"/>.
        /// </summary>
        /// <param name="nodeId">The asset id used to get the curr raw scan data information.</param>
        /// <param name="tagList">The Parameter Tag.</param>
        /// <returns>
        /// A dictionary containing the address and StringValue for the <paramref name="tagList"/>.
        /// If no data or the asset id is not found then an empty list is returned.
        /// </returns>
        public Dictionary<string, string> GetCurrRawScanDataItemsStringValue(string nodeId, List<string> tagList)
        {
            using (var context = _thetaDbContextFactory.GetContext())
            {
                var result = context.CurrentRawScans.Join(context.Parameters, scanData => scanData.Address,
                param => param.Address, (scanData, param) => new { scanData, param })
            .Where(joined => tagList.Contains(joined.param.Tag)
                && joined.param.Poctype == context.NodeMasters.
                Where(node => node.NodeId == nodeId)
                .Select(node => node.PocType).FirstOrDefault()
                && joined.scanData.NodeId == nodeId);
                if (result != null)
                {
                    var output = result.ToList();

                    return output.ToDictionary(c => c.param.Tag.ToString(), c => c.scanData.StringValue);
                }
                else
                {
                    return new Dictionary<string, string>();
                }
            }
        }

        /// <summary>
        /// Gets the ip host name from port master by a node's port id.
        /// </summary>
        /// <param name="portId">The port id.</param>
        /// <returns>The ip host name of the port master entry.</returns>
        public async Task<string> GetIpHostNameByPortId(short? portId)
        {
            if (portId == null)
            {
                return null;
            }

            using (var context = _thetaDbContextFactory.GetContext())
            {
                var ipHostName = await context.PortConfigurations
                    .Where(x => x.PortId == portId)
                    .Select(x => x.IpHostname)
                    .FirstOrDefaultAsync();

                return ipHostName;
            }
        }

        #endregion

        #region Private Records

        private class RodStringRecordData
        {

            /// <summary>
            /// Gets or sets the rod string position number.
            /// </summary>
            public short? RodStringPositionNumber { get; set; }

            /// <summary>
            /// Gets or sets the rod string grade name.
            /// </summary>
            public string RodStringGradeName { get; set; }

            /// <summary>
            /// Gets or sets the diameter.
            /// </summary>
            public double? Diameter { get; set; }

            /// <summary>
            /// Gets or sets the length.
            /// </summary>
            public int? Length { get; set; }

            /// <summary>
            /// Gets or sets the rod string size display name.
            /// </summary>
            public string RodStringSizeDisplayName { get; set; }

        }

        private class RodLiftAssetStatusCoreRecordData
        {

            /// <summary>
            /// Gets or sets the api designation.
            /// </summary>
            public string APIDesignation { get; set; }

            /// <summary>
            /// Gets or sets the api port.
            /// </summary>
            public int? ApiPort { get; set; }

            /// <summary>
            /// Gets or sets the port id.
            /// </summary>
            public short? PortId { get; set; }

            /// <summary>
            /// Gets or sets the asset GUID.
            /// </summary>
            public Guid AssetGUID { get; set; }

            /// <summary>
            /// Gets or sets the calculated fluid level above the pump from esp result.
            /// </summary>
            public float? CalculatedFluidLevelAbovePump { get; set; }

            /// <summary>
            /// Gets or sets the casing pressure.
            /// </summary>
            public short? CasingPressure { get; set; }

            /// <summary>
            /// Gets or sets the communication percentage for yesterday.
            /// </summary>
            public int? CommunicationPercentageYesterday { get; set; }

            /// <summary>
            /// Gets or sets the communication status.
            /// </summary>
            public string CommunicationStatus { get; set; }

            /// <summary>
            /// Gets or sets the ESP result test date.
            /// </summary>
            public DateTime? ESPResultTestDate { get; set; }

            /// <summary>
            /// Gets or sets the firmware version.
            /// </summary>
            public float? FirmwareVersion { get; set; }

            /// <summary>
            /// Gets or sets the fluid level.
            /// </summary>
            public short? FluidLevel { get; set; }

            /// <summary>
            /// Gets or sets the gas rate.
            /// </summary>
            public float? GasRate { get; set; }

            /// <summary>
            /// Gets or sets the rate at test.
            /// </summary>
            public float? RateAtTest { get; set; }

            /// <summary>
            /// Gets or sets the gear box load percentage.
            /// </summary>
            public short? GearBoxLoadPercentage { get; set; }

            /// <summary>
            /// Gets or sets the gross rate.
            /// </summary>
            public float? GrossRate { get; set; }

            /// <summary>
            /// Gets or sets if the node is enabled.
            /// </summary>
            public bool IsNodeEnabled { get; set; }

            /// <summary>
            /// Gets or sets the last good scan.
            /// </summary>
            public DateTime? LastGoodScan { get; set; }

            /// <summary>
            /// The timezone offset of the node.
            /// </summary>
            public float TzOffset { get; set; }

            /// <summary>
            /// The honour daylight saving.
            /// </summary>
            public bool TzDaylight { get; set; }

            /// <summary>
            /// Gets or sets the last well test date.
            /// </summary>
            public DateTime? LastWellTestDate { get; set; }

            /// <summary>
            /// Gets or sets the max rod loading.
            /// </summary>
            public short? MaxRodLoading { get; set; }

            /// <summary>
            /// Gets or sets the motor kind name.
            /// </summary>
            public string MotorKindName { get; set; }

            /// <summary>
            /// Gets or sets the motor load.
            /// </summary>
            public short? MotorLoad { get; set; }

            /// <summary>
            /// Gets or sets the motor type id.
            /// </summary>
            public int? MotorTypeId { get; set; }

            /// <summary>
            /// Gets or sets the plunger diameter.
            /// </summary>
            public float? PlungerDiameter { get; set; }

            /// <summary>
            /// Gets or sets the poc type description.
            /// </summary>
            public string PocTypeDescription { get; set; }

            /// <summary>
            /// Gets or sets the prime motor type.
            /// </summary>
            public string PrimeMoverType { get; set; }

            /// <summary>
            /// Gets or sets the pump depth.
            /// </summary>
            public short? PumpDepth { get; set; }

            /// <summary>
            /// Gets or sets the pump efficiency.
            /// </summary>
            /// 
            public int? PumpEfficiency { get; set; }

            /// <summary>
            /// Gets or sets the pump efficiency percentage.
            /// </summary>
            public short? PumpEfficiencyPercentage { get; set; }

            /// <summary>
            /// Gets or sets the pump fillage.
            /// </summary>
            public int? PumpFillage { get; set; }

            /// <summary>
            /// Gets or sets pumping unit manufacturer.
            /// </summary>
            public string PumpingUnitManufacturer { get; set; }

            /// <summary>
            /// Gets or sets the pumping unit name.
            /// </summary>
            public string PumpingUnitName { get; set; }

            /// <summary>
            /// Gets or sets the node address that represents how to communicate to a device.
            /// </summary>
            public string NodeAddress { get; set; }

            /// <summary>
            /// Gets or sets the node id.
            /// </summary>
            public string NodeId { get; set; }

            /// <summary>
            /// Gets or sets the rated horse power.
            /// </summary>
            public float? RatedHorsePower { get; set; }

            /// <summary>
            /// Gets or sets the run status.
            /// </summary>
            public string RunStatus { get; set; }

            /// <summary>
            /// Gets or sets the stroke length.
            /// </summary>
            public float? StrokeLength { get; set; }

            /// <summary>
            /// Gets or sets the strokes per minute.
            /// </summary>
            public float? StrokesPerMinute { get; set; }

            /// <summary>
            /// Gets or sets the structural loading.
            /// </summary>
            public short? StructuralLoading { get; set; }

            /// <summary>
            /// Gets or sets the time in state.
            /// </summary>
            public int? TimeInState { get; set; }

            /// <summary>
            /// Gets or sets the today runtime percentage.
            /// </summary>
            public float? TodayRuntimePercentage { get; set; }

            /// <summary>
            /// Gets or sets the tubing pressure.
            /// </summary>
            public short? TubingPressure { get; set; }

            /// <summary>
            /// Gets or sets the water cut.
            /// </summary>
            public float? WaterCut { get; set; }

            /// <summary>
            /// Gets or sets the yesterday runtime percentage.
            /// </summary>
            public float? YesterdayRuntimePercentage { get; set; }

            /// <summary>
            /// Gets or sets the company guid.
            /// </summary>
            public Guid? CustomerGUID { get; set; }

            /// <summary>
            /// Gets or sets the application id.
            /// </summary>
            public int? ApplicationId { get; set; }

            /// <summary>
            /// Gets or sets the pumping unit type id.
            /// </summary>
            public string PumpingUnitTypeId { get; set; }

            /// <summary>
            /// Gets or sets the poc type.
            /// </summary>
            public int PocType { get; set; }

            /// <summary>
            /// Gets or sets the pump type.
            /// </summary>
            public string PumpType { get; set; }

            /// <summary>
            /// Gets or sets Line Pressure.
            /// </summary>
            public float? LinePressure { get; set; }

        }

        #endregion

        #region Private Methods

        private RodLiftAssetStatusCoreData Map(RodLiftAssetStatusCoreRecordData coreRecordData)
        {
            if (coreRecordData == null)
            {
                return new RodLiftAssetStatusCoreData();
            }

            return new RodLiftAssetStatusCoreData()
            {
                NodeId = coreRecordData.NodeId,
                AssetGUID = coreRecordData.AssetGUID,
                GrossRate = coreRecordData.GrossRate == null
                    ? null
                    : LiquidFlowRate.FromBarrelsPerDay(coreRecordData.GrossRate.Value),
                StrokeLength = coreRecordData.StrokeLength == null
                    ? null
                    : Length.FromInches(coreRecordData.StrokeLength.Value),
                LastGoodScan = coreRecordData.LastGoodScan,
                TzOffset = coreRecordData.TzOffset,
                TzDaylight = coreRecordData.TzDaylight,
                RunStatus = coreRecordData.RunStatus,
                StrokesPerMinute = coreRecordData.StrokesPerMinute,
                TimeInState = coreRecordData.TimeInState,
                TubingPressure = coreRecordData.TubingPressure == null
                    ? null
                    : Pressure.FromPoundsPerSquareInch(coreRecordData.TubingPressure.Value),
                MotorTypeId = coreRecordData.MotorTypeId,
                PumpFillage = coreRecordData.PumpFillage,
                StructuralLoading = coreRecordData.StructuralLoading,
                LastWellTestDate = coreRecordData.LastWellTestDate,
                APIDesignation = coreRecordData.APIDesignation,
                WaterCut = coreRecordData.WaterCut == null
                    ? null
                    : Fraction.FromPercentage(coreRecordData.WaterCut.Value),
                GasRate = coreRecordData.GasRate == null
                    ? null
                    : GasFlowRate.FromThousandStandardCubicFeetPerDay(coreRecordData.GasRate.Value),
                MaxRodLoading = coreRecordData.MaxRodLoading,
                YesterdayRuntimePercentage = coreRecordData.YesterdayRuntimePercentage,
                CasingPressure = coreRecordData.CasingPressure == null
                    ? null
                    : Pressure.FromPoundsPerSquareInch(coreRecordData.CasingPressure.Value),
                RateAtTest = coreRecordData.RateAtTest == null
                    ? null
                    : GasFlowRate.FromThousandStandardCubicFeetPerDay(coreRecordData.RateAtTest.Value),
                CommunicationPercentageYesterday = coreRecordData.CommunicationPercentageYesterday,
                MotorLoad = coreRecordData.MotorLoad,
                PumpEfficiencyPercentage = coreRecordData.PumpEfficiencyPercentage,
                RatedHorsePower = coreRecordData.RatedHorsePower,
                TodayRuntimePercentage = coreRecordData.TodayRuntimePercentage,
                PrimeMoverType = coreRecordData.PrimeMoverType,
                CommunicationStatus = coreRecordData.CommunicationStatus,
                CalculatedFluidLevelAbovePump = coreRecordData.CalculatedFluidLevelAbovePump,
                FirmwareVersion = coreRecordData.FirmwareVersion,
                ApiPort = coreRecordData.ApiPort,
                PortId = coreRecordData.PortId,
                FluidLevel =
                    coreRecordData.FluidLevel == null ? null : Length.FromFeet(coreRecordData.FluidLevel.Value),
                GearBoxLoadPercentage = coreRecordData.GearBoxLoadPercentage,
                PlungerDiameter = coreRecordData.PlungerDiameter == null
                    ? null
                    : Length.FromInches(coreRecordData.PlungerDiameter.Value),
                PumpDepth = coreRecordData.PumpDepth == null ? null : Length.FromFeet(coreRecordData.PumpDepth.Value),
                PumpEfficiency = coreRecordData.PumpEfficiency,
                ESPResultTestDate = coreRecordData.ESPResultTestDate,
                IsNodeEnabled = coreRecordData.IsNodeEnabled,
                MotorKindName = coreRecordData.MotorKindName,
                NodeAddress = coreRecordData.NodeAddress,
                PocTypeDescription = coreRecordData.PocTypeDescription,
                PumpingUnitManufacturer = coreRecordData.PumpingUnitManufacturer,
                PumpingUnitName = coreRecordData.PumpingUnitName,
                CustomerGUID = coreRecordData.CustomerGUID,
                ApplicationId = coreRecordData.ApplicationId,
                PumpingUnitTypeId = coreRecordData.PumpingUnitTypeId,
                PocType = coreRecordData.PocType,
                PumpType = coreRecordData.PumpType,
                LinePressure = (short?)coreRecordData.LinePressure,
            };
        }

        #endregion

    }
}