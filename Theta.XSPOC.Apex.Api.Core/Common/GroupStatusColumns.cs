using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the group status columns.
    /// </summary>
    public class GroupStatusColumns
    {

        #region Private Fields

        private int _sourceId;
        private string _measure;
        private Hashtable _colStd;
        private readonly IList<GroupStatusTableModel> _viewTables;
        private readonly SortedDictionary<int, string> _stdColList = new SortedDictionary<int, string>();
        private int _align;
        private int _unitMeasureParameter;
        private int _unitMeasureFacilityTag;
        private int _unitMeasure;
        private readonly Hashtable _groupStatusColumns;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupStatusColumns"/> class.
        /// </summary>
        /// <param name="groupStatusTables"></param>
        /// <param name="groupStatusColumns"></param>
        public GroupStatusColumns(IList<GroupStatusTableModel> groupStatusTables, Hashtable groupStatusColumns)
        {

            _measure = string.Empty;

            _stdColList.Clear();
            _stdColList.Add((int)StdCol.PctCom, "%COM");
            _viewTables = groupStatusTables ?? new List<GroupStatusTableModel>();
            _groupStatusColumns = groupStatusColumns ?? new Hashtable();
        }

        #endregion

        #region Public Enums

        /// <summary>
        /// Gets or sets the DataType.
        /// </summary>
        public enum DataType
        {

            /// <summary>
            /// Gets or sets the Alpha.
            /// </summary>
            Alpha = 0,

            /// <summary>
            /// Gets or sets the Numeric.
            /// </summary>
            Numeric = 1,

            /// <summary>
            /// Gets or sets the DateTime.
            /// </summary>
            DateTime = 2,

            /// <summary>
            /// Gets or sets the HoursMinutesSeconds.
            /// </summary>
            HoursMinutesSeconds = 3,

            /// <summary>
            /// Gets or sets the MinutesHoursDays.
            /// </summary>
            MinutesHoursDays = 4,

        }

        /// <summary>
        /// Enum for SourceType.
        /// </summary>
        public enum SourceType
        {

            /// <summary>
            /// Gets and sets the Formula.
            /// </summary>
            Formula = -2, // 0xFFFFFFFE

            /// <summary>
            /// Gets and sets the ParamStandard.
            /// </summary>
            ParamStandard = -1, // 0xFFFFFFFF

            /// <summary>
            /// Gets and sets the Common.
            /// </summary>
            Common = 0,

            /// <summary>
            /// Gets and sets the Parameter.
            /// </summary>
            Parameter = 2,

            /// <summary>
            /// Gets and sets the Facility.
            /// </summary>
            Facility = 4,

        }

        #endregion

        #region Private Enums

        /// <summary>
        /// Gets and sets the Infragistics Align.
        /// </summary>
        private enum InfragisticsAlign
        {

            /// <summary>
            /// Gets and sets the DefaultAlign.
            /// </summary>
            DefaultAlign = 0,

            /// <summary>
            /// Gets and sets the Left.
            /// </summary>
            Left = 1,

            /// <summary>
            /// Gets and sets the Center.
            /// </summary>
            Center = 2,

            /// <summary>
            /// Gets and sets the Right.
            /// </summary>
            Right = 3,

            /// <summary>
            /// Gets and sets the LeftDateTimeMMDD.
            /// </summary>
            LeftDateTimeMMDD = 10,

            /// <summary>
            /// Gets and sets the LeftDateTimeMMDDYY.
            /// </summary>
            LeftDateTimeMMDDYY = 11,

            /// <summary>
            /// Gets and sets the LeftDateMMDD.
            /// </summary>
            LeftDateMMDD = 12,

            /// <summary>
            /// Gets and sets the LeftDateMMDDYY.
            /// </summary>
            LeftDateMMDDYY = 13,

        }

        private enum StdCol
        {

            PctCom = 0,
            PctRty = 1,

        }

        /// <summary>
        /// Gets or sets the MinutesHoursDays.
        /// </summary>
        private enum VB6Align
        {

            /// <summary>
            /// Gets or sets the DefaultAlign.
            /// </summary>
            DefaultAlign = 0,

            /// <summary>
            /// Gets or sets the Left.
            /// </summary>
            Left = 1,

            /// <summary>
            /// Gets or sets the Center.
            /// </summary>
            Center = 4,

            /// <summary>
            /// Gets or sets the Right.
            /// </summary>
            Right = 7,

            /// <summary>
            /// Gets or sets the LeftDateTimeMMDD.
            /// </summary>
            LeftDateTimeMMDD = 10, // 0x0000000A

            /// <summary>
            /// Gets or sets the LeftDateTimeMMDDYY.
            /// </summary>
            LeftDateTimeMMDDYY = 11, // 0x0000000B

            /// <summary>
            /// Gets or sets the LeftDateMMDD.
            /// </summary>
            LeftDateMMDD = 12, // 0x0000000C

            /// <summary>
            /// Gets or sets the LeftDateMMDDYY.
            /// </summary>
            LeftDateMMDDYY = 13, // 0x0000000D

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the FieldName.
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Gets or sets the FieldHeading.
        /// </summary>
        public string FieldHeading { get; set; }

        /// <summary>
        /// Gets or sets the FieldSql.
        /// </summary>
        public string FieldSql { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SupplementalFields.
        /// </summary>
        public string SupplementalFields { get; private set; }

        /// <summary>
        /// Gets or sets the AliasTable.
        /// </summary>
        public string AliasTable { get; private set; }

        /// <summary>
        /// Gets or sets the HasAliasTable.
        /// </summary>
        public bool HasAliasTable => ParentTable == "tblWellTests";

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ParentTable.
        /// </summary>
        public string ParentTable { get; set; }

        /// <summary>
        /// Gets a value indicating whether the column has conditional formatting.
        /// </summary>
        public bool HasConditionalFormat => (ConditionalFormats != null && ConditionalFormats.Count > 0);

        /// <summary>
        /// Gets or sets the SourceId.
        /// </summary>
        public int SourceId
        {
            get => _sourceId;
            set
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    throw new ArgumentException(string.Format("{0} - {1}", "Set Name first prior to setting SourceID", 484));
                }

                _sourceId = value;

                switch (_sourceId)
                {
                    case (int)SourceType.Formula:
                        FieldName = Name;
                        FieldHeading = "Formula." + Name;
                        ParentTable = "Formula";
                        FieldSql = "'' AS [" + FieldHeading + "]";

                        break;
                    case (int)SourceType.ParamStandard:
                        FieldName = Name;
                        FieldHeading = "pst." + Name;
                        ParentTable = "Param Standard Type";
                        FieldSql = "'' AS [pst." + Name + "]";
                        SupplementalFields = "'' AS [pstText." + Name + "], 0 AS [formula.pst." + Name + "]";

                        break;
                    case (int)SourceType.Common:
                        LoadCommonCollection();

                        FieldName = _colStd[Name.Trim().ToUpper()]?.ToString();
                        FieldHeading = _colStd[Name.Trim().ToUpper()]?.ToString();

                        try
                        {
                            if (FieldName == string.Empty)
                            {
                                throw new ArgumentException(string.Format("{0}: {1} - {2}", "This field is required", Name,
                                    "tblGroupStandardColumns"));
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }

                        if (FieldName != null && FieldName.Length > 0 && FieldName.IndexOf(", ") > -1)
                        {
                            FieldHeading = FieldName[..(FieldName.IndexOf(", ") - 1)];
                            FieldSql = FieldName[..FieldName.IndexOf(", ")];
                            SupplementalFields = FieldName[(FieldName.IndexOf(", ") + 1)..];
                        }

                        if (FieldName != null && FieldName.Length > 0 && FieldName.IndexOf("[") > -1)
                        {
                            FieldHeading = FieldName[(FieldName.IndexOf("[") + 1)..];
                            FieldHeading = FieldHeading[..FieldHeading.IndexOf("]")];
                        }
                        else
                        {
                            FieldHeading = FieldName;
                        }

                        ParentTable = "Frequently Used";

                        var upperName = Name.ToUpper();

                        if (upperName == "POC TYPE" || upperName == "POCTYPE")
                        {
                            FieldHeading = Name;
                            FieldSql = string.Empty;
                        }
                        else if (upperName == "ENBLD" ||
                                 upperName == _stdColList[(int)StdCol.PctCom] ||
                                 upperName == "%FILL" ||
                                 upperName == "%RT" ||
                                 upperName == "%RT30D" ||
                                 upperName == "%RTY" ||
                                 upperName == "TIS" ||
                                 upperName == "OIL" ||
                                 upperName == "COMMENT" ||
                                 upperName == "LAST GOOD SCAN" ||
                                 upperName == "ALARMS" ||
                                 upperName == "COMM" ||
                                 upperName == "RUN STATUS" ||
                                 upperName == "TECHNOTE" ||
                                 upperName == "TECH NOTE")
                        {
                            FieldHeading = Name;

                            if (string.IsNullOrEmpty(FieldSql))
                            {
                                FieldSql = FieldName;
                            }
                        }
                        else
                        {
                            FieldHeading = Name;

                            if (string.IsNullOrEmpty(FieldSql))
                            {
                                FieldSql = FieldName;
                            }

                            Debug.WriteLine(Name);
                        }

                        break;
                    case (int)SourceType.Parameter:
                        FieldName = "tblParameters." + Name.Trim() + " AS [tblParameters." + Name.Trim() + "]";
                        FieldHeading = "tblParameters." + Name.Trim();
                        ParentTable = "tblParameters";
                        FieldSql = "'' AS [tblParameters." + Name.Trim() + "]";

                        break;
                    case (int)SourceType.Facility:
                        FieldName = "tblFacilityTags." + Name.Trim() + " AS [tblFacilityTags." + Name.Trim() + "]";
                        FieldHeading = "tblFacilityTags." + Name.Trim();
                        ParentTable = "tblFacilityTags";
                        FieldSql = "'' AS [" + FieldHeading + "]";
                        SupplementalFields = "0 AS [formula.tblFacilityTags." + Name + "]";

                        break;
                    default:
                        ParentTable = _viewTables.FirstOrDefault(x => x.TableId == _sourceId)?.TableName ?? string.Empty;

                        FieldName = ParentTable + "." + Name.Trim() + " AS [" + ParentTable + "." + Name.Trim() + "]";
                        FieldHeading = ParentTable + "." + Name.Trim();
                        FieldSql = FieldName;

                        break;
                }
            }
        }

        /// <summary>
        /// Gets and set the Align.
        /// </summary>
        public int Align
        {
            get => _align;
            set
            {
                _align = value;
                switch (value)
                {
                    case (int)InfragisticsAlign.DefaultAlign:
                        _align = (int)VB6Align.DefaultAlign;
                        break;
                    case (int)InfragisticsAlign.Left:
                        _align = (int)VB6Align.Left;
                        break;
                    case (int)InfragisticsAlign.Center:
                        _align = (int)VB6Align.Center;
                        break;
                    case (int)InfragisticsAlign.Right:
                        _align = (int)VB6Align.Right;
                        break;
                    case (int)InfragisticsAlign.LeftDateTimeMMDD:
                        _align = (int)VB6Align.LeftDateTimeMMDD;
                        break;
                    case (int)InfragisticsAlign.LeftDateTimeMMDDYY:
                        _align = (int)VB6Align.LeftDateTimeMMDDYY;
                        break;
                    case (int)InfragisticsAlign.LeftDateMMDD:
                        _align = (int)VB6Align.LeftDateMMDD;
                        break;
                    case (int)InfragisticsAlign.LeftDateMMDDYY:
                        _align = (int)VB6Align.LeftDateMMDDYY;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Gets and set the ParamStandardType.
        /// </summary>
        public int ParamStandardType { get; set; }

        /// <summary>
        /// Gets and set the Column Id.
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Gets and set the UnitMeasureParameter.
        /// </summary>
        public int UnitMeasureParameter
        {
            get => EnhancedEnumBase.GetValue<UnitCategory>(_unitMeasureParameter).Key;
            set => _unitMeasureParameter = EnhancedEnumBase.GetValue<UnitCategory>(value).Key;
        }

        /// <summary>
        /// Gets and set the UnitMeasureFacilityTag.
        /// </summary>
        public int UnitMeasureFacilityTag
        {
            get => EnhancedEnumBase.GetValue<UnitCategory>(_unitMeasureFacilityTag).Key;
            set => _unitMeasureFacilityTag = EnhancedEnumBase.GetValue<UnitCategory>(value).Key;
        }

        /// <summary>
        /// Gets and set the Position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets and set the Orientation.
        /// </summary>
        public int Orientation { get; set; }

        /// <summary>
        /// Gets and set the Format Id.
        /// </summary>
        public int FormatId { get; set; }

        /// <summary>
        /// Gets and set the FormatMask.
        /// </summary>
        public string FormatMask { get; set; }

        /// <summary>
        /// Gets and set the Measure.
        /// </summary>
        public string Measure
        {
            get
            {
                var key = HighPriorityUnitMeasure(_unitMeasureFacilityTag, _unitMeasureParameter,
                    _unitMeasure);
                var unitCategory1 = EnhancedEnumBase.GetValue<UnitCategory>(key);
                string measure;
                if (key > 0)
                {
                    var unitCategory2 = unitCategory1;
                    measure = !(unitCategory2 == UnitCategory.FluidRate)
                        ? !(unitCategory2 == UnitCategory.LongLength)
                            ? !(unitCategory2 == UnitCategory.ShortLength)
                                ? !(unitCategory2 == UnitCategory.Pressure)
                                    ? !(unitCategory2 == UnitCategory.Temperature)
                                        ? !(unitCategory2 == UnitCategory.GasRate)
                                            ? !(unitCategory2 == UnitCategory.Weight)
                                                ? !(unitCategory2 ==
                                                    UnitCategory.GasVolume)
                                                    ? !(unitCategory2 ==
                                                        UnitCategory.FluidVolume)
                                                        ? !(unitCategory2 ==
                                                            UnitCategory.OilRelativeDensity)
                                                            ? !(unitCategory2 ==
                                                                UnitCategory.Power)
                                                                ? "None"
                                                                : "::measurements.power"
                                                            : "::measurements.gravity"
                                                        : "::measurements.fluid volume"
                                                    : "::measurements.mcf"
                                                : "::measurements.weight"
                                            : "::measurements.gas rates"
                                        : "::measurements.temperature"
                                    : "::measurements.pressure"
                                : "::measurements.short length"
                            : "::measurements.length"
                        : "::measurements.fluid rates";
                }
                else
                {
                    measure = _measure;
                }

                return measure;
            }
            set => _measure = value;
        }

        /// <summary>
        /// Get or sets the UnitMeasure.
        /// </summary>
        public int UnitMeasure
        {
            get => EnhancedEnumBase.GetValue<UnitCategory>(_unitMeasure).Key;
            set => _unitMeasure = EnhancedEnumBase.GetValue<UnitCategory>(value).Key;
        }

        /// <summary>
        /// Get or sets the Visible.
        /// </summary>
        public int Visible { get; set; }

        /// <summary>
        /// Get or sets the Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the list of conditional formats for the column.
        /// </summary>
        public IList<ConditionalFormat> ConditionalFormats { get; set; }

        /// <summary>
        /// Get or sets the Formula.
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Get or sets the FieldType.
        /// </summary>
        public DataType FieldType { get; set; }

        /// <summary>
        /// Get or sets the ColumnAlias.
        /// </summary>
        public string ColumnAlias { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the table alias.
        /// </summary>
        public void ConvertTableAlias()
        {
            if (ParentTable == "tblWellTests")
            {
                AliasTable = "vwWellTest";
                FieldHeading = FieldHeading.Replace(ParentTable, AliasTable);
                FieldName = FieldName.Replace(ParentTable, AliasTable);
                FieldSql = FieldSql.Replace(ParentTable, AliasTable);
            }

            ParentTable = AliasTable;
        }

        /// <summary>
        /// Converts the database alignment value to the corresponding integer value.
        /// </summary>
        /// <param name="dbAlignValue">The database alignment value.</param>
        /// <returns>The corresponding integer value.</returns>
        public int ConvertDBAlign(int? dbAlignValue)
        {
            var align = 0;

            if (dbAlignValue == (int)VB6Align.DefaultAlign)
            {
                align = 0;
            }
            else if (dbAlignValue == (int)VB6Align.Left)
            {
                align = 1;
            }
            else if (dbAlignValue == (int)VB6Align.Center)
            {
                align = 2;
            }
            else if (dbAlignValue == (int)VB6Align.Right)
            {
                align = 3;
            }
            else if (dbAlignValue == (int)VB6Align.LeftDateTimeMMDD)
            {
                align = 1;
            }
            else if (dbAlignValue == (int)VB6Align.LeftDateTimeMMDDYY)
            {
                align = 1;
            }
            else if (dbAlignValue == (int)VB6Align.LeftDateMMDD)
            {
                align = 1;
            }
            else if (dbAlignValue == (int)VB6Align.LeftDateMMDDYY)
            {
                align = 1;
            }

            return align;
        }

        /// <summary>
        /// Determines if the field requires calculation.
        /// </summary>
        /// <returns>True if the field requires calculation, otherwise false.</returns>
        public bool RequiresCalculation()
        {
            switch (SourceId)
            {
                case (int)SourceType.Common:
                    switch (Name.ToUpper())
                    {
                        case var name when name == _stdColList[(int)StdCol.PctCom] || name == "TBLNODEMASTER.COMMSUCCESS":
                        case "%FILL":
                        case "%RT":
                        case "%RTY":
                        case "ALARMS":
                        case "CAMERAALARMS":
                        case "COMM":
                        case "CONTROLLER":
                        case "POCTYPE":
                        case "POC TYPE":
                        case "DRC":
                        case "ENBLD":
                        case "EXCEPTIONGROUPNAME":
                        case "HOSTALARMS":
                        case "PUMPING UNIT":
                        case "PUMPING UNIT MANUFACTURER":
                        case "ROD GRADE":
                        case "RUN STATUS":
                        case "STRINGID":
                        case "TIS":
                        case "WELL":
                            return true;

                        case "FACILITYTAGALARMS":
                        case "LAST GOOD SCAN":
                        case "LAST ALARM DATE":
                        case "LASTTESTDATE":
                            return false;
                        default:
                            return false;
                    }
                case (int)SourceType.Parameter:
                    if (FieldType == DataType.Alpha)
                    {
                        return false;
                    }

                    return true;
                case (int)SourceType.Facility:
                case (int)SourceType.ParamStandard:
                case (int)SourceType.Formula:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if the column requires formatting based on the specified source Id.
        /// </summary>
        /// <returns>True if the column requires formatting, otherwise false.</returns>
        public bool RequiresFormatting()
        {
            switch (SourceId)
            {
                case (int)SourceType.Common:
                    switch (Name.ToUpper())
                    {
                        case var name when name == _stdColList[(int)StdCol.PctCom] || name == "TBLNODEMASTER.COMMSUCCESS":
                        case "%FILL":
                        case "%RTY":
                        case "ALARMS":
                        case "CAMERAALARMS":
                        case "COMM":
                        case "DRC":
                        case "ENBLD":
                        case "EXCEPTIONGROUPNAME":
                        case "FACILITYTAGALARMS":
                        case "HOSTALARMS":
                        case "RUN STATUS":
                        case "WELL":
                            return true;
                        default:
                            return false;
                    }
                case (int)SourceType.Parameter:
                case (int)SourceType.Facility:
                case (int)SourceType.ParamStandard:
                    return true;
                default:
                    return HasConditionalFormat;
            }
        }

        #endregion

        #region Private Methods

        private int HighPriorityUnitMeasure(int valueOne, int valueTwo, int valueThree)
        {
            var prioritizedUnitMeasure = 0;

            if (valueOne > 0)
            {
                prioritizedUnitMeasure = valueOne;
            }
            else if (valueTwo > 0)
            {
                prioritizedUnitMeasure = valueTwo;
            }
            else if (valueThree > 0)
            {
                prioritizedUnitMeasure = valueThree;
            }

            return prioritizedUnitMeasure;
        }

        private Hashtable LoadCommonCollection()
        {
            if (_colStd != null && _colStd.Count > 0)
            {
                return _colStd;
            }

            _colStd = new Hashtable(_groupStatusColumns);

            _colStd.Add(_stdColList[0],
                " {fn CONVERT('0',SQL_DOUBLE)} AS [%COM], dbo.tblNodeMaster.CommSuccess AS [tblNodeMaster.CommSuccess], dbo.tblNodeMaster.CommAttempt AS [tblNodeMaster.CommAttempt]");
            _colStd.Add("%RT 30D", " CAST(dbo.[vw30DayRuntimeAverage].RT_Pct30Day AS INTEGER) AS [%RT 30D]");
            _colStd.Add("%RT",
                " {fn CONVERT('0',SQL_DOUBLE)} AS [%RT], dbo.tblNodeMaster.TodayRunTime AS [tblNodeMaster.TodayRunTime], dbo.tblNodeMaster.LastGoodScanTime AS [tblNodeMaster.LastGoodScanTime]");
            _colStd.Add("%RTY",
                " CAST(((dbo.tblWellDetails.Runtime * 100 / 24) + .5) AS INTEGER) AS [%RTY], dbo.tblWellStatistics.AlarmStateRuntime as [tblWellStatistics.AlarmStateRuntime]");
            _colStd.Add("%FILL",
                " {fn CONVERT('0',SQL_DOUBLE)} AS [%FILL], dbo.tblNodeMaster.PumpFillage AS [tblNodeMaster.PumpFillage],dbo.tblWellStatistics.AlarmStateFillage AS [tblWellStatistics.AlarmStateFillage], dbo.tblNodeMaster.WellOpType AS [tblNodeMaster.WellOpType]");
            _colStd.Add("ALARMS",
                " '' as [Alarms], dbo.tblNodeMaster.HighPriAlarm AS [tblNodeMaster.HighPriAlarm]");
            _colStd.Add("COMM", " '' as [Comm], dbo.tblNodeMaster.CommStatus AS [tblNodeMaster.CommStatus]");
            _colStd.Add("CONTROLLER", " '' as [Controller]");
            _colStd.Add("DRC",
                " '' as [DRC], dbo.tblNodeMaster.TodayRunTime AS [tblNodeMaster.TodayRunTime], dbo.tblNodeMaster.RunStatus AS [tblNodeMaster.RunStatus], dbo.tblNodeMaster.CommStatus AS [tblNodeMaster.CommStatus], dbo.tblNodeMaster.LastGoodScanTime AS [tblNodeMaster.LastGoodScanTime], dbo.tblWellDetails.DownReasonCode AS [tblWellDetails.DownReasonCode]");
            _colStd.Add("ENBLD", " '' as [ENBLD], dbo.tblNodeMaster.DisableCode AS [DisableCode]");
            _colStd.Add("FACILITYTAGALARMS", " '' as [FacilityTagAlarms]");
            _colStd.Add("HOSTALARMS",
                " '' as [HostAlarms], dbo.tblNodeMaster.HostAlarm AS [tblNodeMaster.HostAlarm], dbo.tblNodeMaster.HostAlarmState AS [tblNodeMaster.HostAlarmState]");
            _colStd.Add("PUMPING UNIT", " dbo.tblWellDetails.PumpingUnitID AS [Pumping Unit]");
            _colStd.Add("PUMPING UNIT MANUFACTURER", " dbo.tblWellDetails.PumpingUnitID AS [Pumping Unit Manufacturer]");
            _colStd.Add("ROD GRADE", " '' AS [Rod Grade]");
            _colStd.Add("RUN STATUS", " '' as [Run Status], dbo.tblNodeMaster.RunStatus AS [tblNodeMaster.RunStatus]");

            if (!_colStd.ContainsKey(_stdColList[0]))
            {
                _colStd.Add(_stdColList[0],
                    "{fn CONVERT('0',SQL_DOUBLE)} AS [%COM], dbo.tblNodeMaster.CommSuccess AS [tblNodeMaster.CommSuccess], dbo.tblNodeMaster.CommAttempt AS [tblNodeMaster.CommAttempt]");
            }

            if (!_colStd.ContainsKey("%RT 30D"))
            {
                _colStd.Add("%RT 30D",
                    " CAST(dbo.[vw30DayRuntimeAverage].RT_Pct30Day AS INTEGER) AS [%RT 30D]");
            }

            if (!_colStd.ContainsKey("%RT"))
            {
                _colStd.Add("%RT",
                    " {fn CONVERT('0',SQL_DOUBLE)} AS [%RT], dbo.tblNodeMaster.TodayRunTime AS [tblNodeMaster.TodayRunTime], dbo.tblNodeMaster.LastGoodScanTime AS [tblNodeMaster.LastGoodScanTime]");
            }

            if (!_colStd.ContainsKey("%RTY"))
            {
                _colStd.Add("%RTY",
                    " CAST(((dbo.tblWellDetails.Runtime * 100 / 24) + .5) AS INTEGER) AS [%RTY], dbo.tblWellStatistics.AlarmStateRuntime as [tblWellStatistics.AlarmStateRuntime]");
            }

            if (!_colStd.ContainsKey("%FILL"))
            {
                _colStd.Add("%FILL",
                    " {fn CONVERT('0',SQL_DOUBLE)} AS [%FILL], dbo.tblNodeMaster.PumpFillage AS [tblNodeMaster.PumpFillage],dbo.tblWellStatistics.AlarmStateFillage AS [tblWellStatistics.AlarmStateFillage], dbo.tblNodeMaster.WellOpType AS [tblNodeMaster.WellOpType]");
            }

            if (!_colStd.ContainsKey("ADDRESS"))
            {
                _colStd.Add("ADDRESS", " dbo.tblNodeMaster.Node AS [Address]");
            }

            if (!_colStd.ContainsKey("ADHOCGROUP1"))
            {
                _colStd.Add("ADHOCGROUP1", " dbo.tblNodeMaster.AdHocGroup1 AS [AdHocGroup1]");
            }

            if (!_colStd.ContainsKey("ADHOCGROUP2"))
            {
                _colStd.Add("ADHOCGROUP2", " dbo.tblNodeMaster.AdHocGroup2 AS [AdHocGroup2]");
            }

            if (!_colStd.ContainsKey("ADHOCGROUP3"))
            {
                _colStd.Add("ADHOCGROUP3", " dbo.tblNodeMaster.AdHocGroup3 AS [AdHocGroup3]");
            }

            if (!_colStd.ContainsKey("ALARMS"))
            {
                _colStd.Add("ALARMS",
                    "'' as [Alarms], dbo.tblNodeMaster.HighPriAlarm AS [tblNodeMaster.HighPriAlarm]");
            }

            if (!_colStd.ContainsKey("CAMERAALARMS"))
            {
                _colStd.Add("CAMERAALARMS",
                    " CAST(vwAggregateCameraAlarmStatus.AlarmCount AS nvarchar) AS [CameraAlarms]");
            }

            if (!_colStd.ContainsKey("CASINGPRESSURE"))
            {
                _colStd.Add("CASINGPRESSURE", " dbo.tblWellDetails.CasingPressure AS [CasingPressure]");
            }

            if (!_colStd.ContainsKey("COMM"))
            {
                _colStd.Add("COMM",
                    "'' as [Comm], dbo.tblNodeMaster.CommStatus AS [tblNodeMaster.CommStatus]");
            }

            if (!_colStd.ContainsKey("COMMENT"))
            {
                _colStd.Add("COMMENT", " dbo.tblNodeMaster.Comment AS [Comment]");
            }

            if (!_colStd.ContainsKey("CONTROLLER"))
            {
                _colStd.Add("CONTROLLER", "'' as [Controller]");
            }

            if (!_colStd.ContainsKey("CYCL"))
            {
                _colStd.Add("CYCL", " dbo.tblNodeMaster.TodayCycles AS [CYCL]");
            }

            if (!_colStd.ContainsKey("DRC"))
            {
                _colStd.Add("DRC",
                    "'' as [DRC], dbo.tblNodeMaster.TodayRunTime AS [tblNodeMaster.TodayRunTime], dbo.tblNodeMaster.RunStatus AS [tblNodeMaster.RunStatus], dbo.tblNodeMaster.CommStatus AS [tblNodeMaster.CommStatus], dbo.tblNodeMaster.LastGoodScanTime AS [tblNodeMaster.LastGoodScanTime], dbo.tblWellDetails.DownReasonCode AS [tblWellDetails.DownReasonCode]");
            }

            if (!_colStd.ContainsKey("ENBLD"))
            {
                _colStd.Add("ENBLD", "'' as [ENBLD], dbo.tblNodeMaster.DisableCode AS [DisableCode]");
            }

            if (!_colStd.ContainsKey("ENERGYGROUP"))
            {
                _colStd.Add("ENERGYGROUP", " dbo.tblNodeMaster.EnergyGroup AS [EnergyGroup]");
            }

            if (!_colStd.ContainsKey("ENERGYMODE"))
            {
                _colStd.Add("ENERGYMODE", " dbo.tblNodeMaster.EnergyMode AS [EnergyMode]");
            }

            if (!_colStd.ContainsKey("EXCEPTIONGROUPNAME"))
            {
                _colStd.Add("EXCEPTIONGROUPNAME", "'' AS ExceptionGroupName");
            }

            if (!_colStd.ContainsKey("FLUIDLEVEL"))
            {
                _colStd.Add("FLUIDLEVEL", " dbo.tblWellDetails.FluidLevel AS [FluidLevel]");
            }

            if (!_colStd.ContainsKey("GROSSRATE"))
            {
                _colStd.Add("GROSSRATE", " dbo.tblWellDetails.GrossRate AS [GrossRate]");
            }

            if (!_colStd.ContainsKey("FACILITYTAGALARMS"))
            {
                _colStd.Add("FACILITYTAGALARMS", " '' as [FacilityTagAlarms]");
            }

            if (!_colStd.ContainsKey("HOSTALARMS"))
            {
                _colStd.Add("HOSTALARMS",
                    "'' as [HostAlarms], dbo.tblNodeMaster.HostAlarm AS [tblNodeMaster.HostAlarm], dbo.tblNodeMaster.HostAlarmState AS [tblNodeMaster.HostAlarmState]");
            }

            if (!_colStd.ContainsKey("IDLETIME"))
            {
                _colStd.Add("IDLETIME", " dbo.tblWellDetails.IdleTime AS [IdleTime]");
            }

            if (!_colStd.ContainsKey("LAST ALARM DATE"))
            {
                _colStd.Add("LAST ALARM DATE", " dbo.tblNodeMaster.LastAlarmDate AS [Last Alarm Date]");
            }

            if (!_colStd.ContainsKey("LAST GOOD SCAN"))
            {
                _colStd.Add("LAST GOOD SCAN", " dbo.tblNodeMaster.LastGoodScanTime AS [Last Good Scan]");
            }

            if (!_colStd.ContainsKey("LASTTESTDATE"))
            {
                _colStd.Add("LASTTESTDATE", " dbo.tblWellDetails.LastTestDate AS [LastTestDate]");
            }

            if (!_colStd.ContainsKey("MOTORHP"))
            {
                _colStd.Add("MOTORHP", " dbo.tblWellDetails.MotorHP AS [MotorHP]");
            }

            if (!_colStd.ContainsKey("OIL"))
            {
                _colStd.Add("OIL",
                    "CAST((((100 - dbo.tblWellDetails.WaterCut) / 100 * dbo.tblWellDetails.GrossRate) + .5) AS INTEGER) AS [OIL]");
            }

            if (!_colStd.ContainsKey("OILAPIGRAVITY"))
            {
                _colStd.Add("OILAPIGRAVITY", " dbo.tblWellDetails.OilAPIGravity AS [OilAPIGravity]");
            }

            if (!_colStd.ContainsKey("OPERATIONALSCORE"))
            {
                _colStd.Add("OPERATIONALSCORE",
                    " vwOperationalScoresLast.OperationalScore AS [OperationalScore]");
            }

            if (!_colStd.ContainsKey("OP AREA"))
            {
                _colStd.Add("OP AREA", " dbo.tblStrings.StringName AS [Op Area]");
            }

            if (!_colStd.ContainsKey("OTHERWELLID"))
            {
                _colStd.Add("OTHERWELLID", " dbo.tblNodeMaster.OtherWellID1 AS [OtherWellID]");
            }

            if (!_colStd.ContainsKey("PLUNGERDIAM"))
            {
                _colStd.Add("PLUNGERDIAM", " dbo.tblWellDetails.PlungerDiam AS [PlungerDiam]");
            }

            if (!_colStd.ContainsKey("PORTID"))
            {
                _colStd.Add("PORTID", " dbo.tblNodeMaster.PortID AS [PortID]");
            }

            if (!_colStd.ContainsKey("PUMP COND"))
            {
                _colStd.Add("PUMP COND", " dbo.tblNodeMaster.LastAnalCondition AS [Pump Cond]");
            }

            if (!_colStd.ContainsKey("PUMPDEPTH"))
            {
                _colStd.Add("PUMPDEPTH", " dbo.tblWellDetails.PumpDepth AS [PumpDepth]");
            }

            if (!_colStd.ContainsKey("PUMPFILLAGE"))
            {
                _colStd.Add("PUMPFILLAGE", " dbo.tblNodeMaster.PumpFillage AS [PumpFillage]");
            }

            if (!_colStd.ContainsKey("PUMPING UNIT"))
            {
                _colStd.Add("PUMPING UNIT", " dbo.tblWellDetails.PumpingUnitID AS [Pumping Unit]");
            }

            if (!_colStd.ContainsKey("PUMPING UNIT MANUFACTURER"))
            {
                _colStd.Add("PUMPING UNIT MANUFACTURER",
                    " dbo.tblWellDetails.PumpingUnitID AS [Pumping Unit Manufacturer]");
            }

            if (!_colStd.ContainsKey("ROD GRADE"))
            {
                _colStd.Add("ROD GRADE", "'' AS [Rod Grade]");
            }

            if (!_colStd.ContainsKey("RUN STATUS"))
            {
                _colStd.Add("RUN STATUS",
                    "'' as [Run Status], dbo.tblNodeMaster.RunStatus AS [tblNodeMaster.RunStatus]");
            }

            if (!_colStd.ContainsKey("SPM"))
            {
                _colStd.Add("SPM", " dbo.tblWellDetails.SPM AS [SPM]");
            }

            if (!_colStd.ContainsKey("STRINGID"))
            {
                _colStd.Add("STRINGID", " dbo.tblNodeMaster.StringID AS [StringIDInt], ' ' AS [StringID]");
            }

            if (!_colStd.ContainsKey("STROKELENGTH"))
            {
                _colStd.Add("STROKELENGTH", " dbo.tblWellDetails.StrokeLength AS [StrokeLength]");
            }

            if (!_colStd.ContainsKey("TECH NOTE"))
            {
                _colStd.Add("TECH NOTE", "dbo.tblNodeMaster.TechNote as [Tech Note]");
            }

            if (!_colStd.ContainsKey("TIS"))
            {
                _colStd.Add("TIS",
                    "CAST(Case dbo.tblNodeMaster.Enabled WHEN '1' THEN dbo.tblNodeMaster.TimeInState ELSE null END AS varchar) as [TIS]");
            }

            if (!_colStd.ContainsKey("TUBING PRESSURE"))
            {
                _colStd.Add("TUBING PRESSURE", " dbo.tblWellDetails.TubingPressure AS [Tubing Pressure]");
            }

            if (!_colStd.ContainsKey("TUBINGANCHORDEPTH"))
            {
                _colStd.Add("TUBINGANCHORDEPTH",
                    " dbo.tblWellDetails.TubingAnchorDepth AS [TubingAnchorDepth]");
            }

            if (!_colStd.ContainsKey("WATERCUT"))
            {
                _colStd.Add("WATERCUT", " dbo.tblWellDetails.WaterCut AS [WaterCut]");
            }

            if (!_colStd.ContainsKey("WATERRATE"))
            {
                _colStd.Add("WATERRATE",
                    " CAST(((dbo.tblWellDetails.GrossRate * dbo.tblWellDetails.WaterCut) / 100 + .5) AS INTEGER) AS [WaterRate]");
            }

            if (!_colStd.ContainsKey("WATERSG"))
            {
                _colStd.Add("WATERSG", " dbo.tblWellDetails.WaterSG AS [WaterSG]");
            }

            if (!_colStd.ContainsKey("WELL"))
            {
                _colStd.Add("WELL", " dbo.tblNodeMaster.NodeID AS [Well]");
            }

            if (!_colStd.ContainsKey("WELLOPTYPE"))
            {
                _colStd.Add("WELLOPTYPE", " dbo.tblNodeMaster.WellOpType AS [WellOpType]");
            }

            if (!_colStd.ContainsKey("YCYCL"))
            {
                _colStd.Add("YCYCL", " dbo.tblWellDetails.Cycles AS [Ycycl]");
            }

            if (!_colStd.ContainsKey("YPMP"))
            {
                _colStd.Add("YPMP",
                    " CAST (CASE dbo.tblWellDetails.Runtime WHEN 24 THEN 1440 ELSE CASE dbo.tblWellDetails.Cycles WHEN 0 THEN round(dbo.tblWellDetails.Runtime * 100 / 24,0) / 100 * 1440   ELSE    round(tblWellDetails.Runtime * 100 / 24,0) / 100 * 1440 / NULLIF(tblWellDetails.Cycles,0)     END  END    AS INTEGER  ) AS [YPmp]");
            }

            return _colStd;
        }

        #endregion

    }
}
