using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Meter Trend Data.
    /// </summary>
    public class MeterTrendData : TrendData
    {

        #region Private Members

        private enum LocalePhraseIDs
        {
            Volume = 513, // Volume
            AccumVolume = 1111, // Accum. Volume
            TargetRate = 1077, // Target Rate
            InstantRate = 1109, // Instant Rate
            Pressure = 754, // Pressure
            TargetPressure = 1074, // Target Pressure
            ValvePosition = 1113, // Valve Position
            Custom = 4188, // Custom ,
            CasingPressure = 265, // Casing Pressure   
            TubingPressure = 264, // Tubing Pressure
        }

        #endregion

        /// <summary>
        /// Gets or sets the device type.
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public int? UnitType { get; set; }

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <seealso cref="MeasurementTrendData"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public MeterTrendData(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="MeterTrendData"/>.
        /// </summary>
        /// <param name="deviceType">The device type.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public MeterTrendData(int deviceType, string name, string description)
            : base(name)
        {
            this.Name = name;
            this.Description = description;
            this.DeviceType = deviceType;
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="MeterTrendData"/>.
        /// </summary>
        /// <param name="deviceType">The device type.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="unitType">The unit type.</param>
        public MeterTrendData(int deviceType, string name, string description, int? unitType)
            : this(deviceType, name, description)
        {
            this.UnitType = unitType;
        }

        #endregion

        /// <summary>
        /// Returns the data within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>An <see cref="IList{DataPoint}" /> containing the data points found.</returns>
        /// <remarks>The Note property will be set to "Manual" or "Scan" depending on the record source.</remarks>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();
            return data;
        }

        /// <summary>
        /// Gets the meter trend data items.
        /// </summary>
        /// <param name="meterTrendData">List of <seealso cref="IList{MeterColumnItemModel}"/> model object.</param>
        /// <param name="deviceType">The device type.</param>
        /// <param name="meterTypeId">The meter type id.</param>
        /// <param name="phrases">The locale phrase dictionary.</param>
        /// <returns>The <seealso cref="IList{MeterTrendData}"/> items.</returns>
        public static IList<MeterTrendData> GetItems(IList<MeterColumnItemModel> meterTrendData, int deviceType,
            int meterTypeId, IDictionary<int, string> phrases)
        {
            if (phrases == null)
            {
                return null;
            }

            IList<MeterTrendData> meterTrendDataList = new List<MeterTrendData>();

            var columns = meterTrendData.Where(x => x.MeterTypeId == meterTypeId);
            var lookup = new SortedDictionary<string, string>();
            string key = string.Empty;
            string phrase = string.Empty;

            foreach (var col in columns)
            {
                if (!lookup.ContainsKey(col.Name) && !string.IsNullOrEmpty(col.Alias))
                {
                    lookup.Add(col.Name, col.Alias);
                }
            }

            meterTrendDataList.Add(GetMeterTrendData("Volume", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("AccumVolume", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("TargetRate", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("InstantRate", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("Pressure", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("TargetPressure", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("CasingPressure", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("TubingPressure", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("ValvePosition", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("Custom01", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("Custom02", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("Custom03", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("Custom04", deviceType, lookup, phrases));
            meterTrendDataList.Add(GetMeterTrendData("Custom05", deviceType, lookup, phrases));

            return meterTrendDataList.ToList();
        }

        #region Private Methods

        private static MeterTrendData GetMeterTrendData(string key, int deviceType,
            SortedDictionary<string, string> colAliases, IDictionary<int, string> phrases)
        {
            int? unitType = GetUnitType(key, deviceType);

            if (colAliases.TryGetValue(key, out var phrase) == false)
            {
                switch (key)
                {
                    case "Volume":
                        phrase = phrases[(int)LocalePhraseIDs.Volume];
                        break;
                    case "AccumVolume":
                        phrase = phrases[(int)LocalePhraseIDs.AccumVolume];
                        break;
                    case "TargetRate":
                        phrase = phrases[(int)LocalePhraseIDs.TargetPressure];
                        break;
                    case "InstantRate":
                        phrase = phrases[(int)LocalePhraseIDs.InstantRate];
                        break;
                    case "Pressure":
                        phrase = phrases[(int)LocalePhraseIDs.Pressure];
                        break;
                    case "TargetPressure":
                        phrase = phrases[(int)LocalePhraseIDs.TargetPressure];
                        break;
                    case "CasingPressure":
                        phrase = phrases[(int)LocalePhraseIDs.CasingPressure];
                        break;
                    case "TubingPressure":
                        phrase = phrases[(int)LocalePhraseIDs.TubingPressure];
                        break;
                    case "ValvePosition":
                        phrase = phrases[(int)LocalePhraseIDs.ValvePosition];
                        break;
                    case "Custom01":
                        phrase = String.Format(phrases[(int)LocalePhraseIDs.Custom], 1);
                        break;
                    case "Custom02":
                        phrase = String.Format(phrases[(int)LocalePhraseIDs.Custom], 2);
                        break;
                    case "Custom03":
                        phrase = String.Format(phrases[(int)LocalePhraseIDs.Custom], 3);
                        break;
                    case "Custom04":
                        phrase = String.Format(phrases[(int)LocalePhraseIDs.Custom], 4);
                        break;
                    case "Custom05":
                        phrase = String.Format(phrases[(int)LocalePhraseIDs.Custom], 5);
                        break;
                    default:
                        break;
                }
            }

            return new MeterTrendData(deviceType, key, phrase, unitType);
        }

        private static int? GetUnitType(string name, int deviceType)
        {
            int? unitType = null;

            switch (name)
            {
                case "Volume":
                case "AccumVolume":
                case "TargetRate":
                case "InstantRate":
                    if ((41 <= deviceType && 49 >= deviceType) || (541 <= deviceType && 549 >= deviceType))
                    {
                        unitType = UnitCategory.GasRate.Key;
                    }
                    else if ((81 <= deviceType && 89 >= deviceType) || (581 <= deviceType && 589 >= deviceType))
                    {
                        unitType = UnitCategory.FluidRate.Key;
                    }
                    else if ((51 <= deviceType && 59 >= deviceType) || (551 <= deviceType && 559 >= deviceType))
                    {
                        unitType = UnitCategory.FluidRate.Key;
                    }
                    else
                    {
                        unitType = UnitCategory.None.Key;
                    }

                    break;
                case "Pressure":
                case "TargetPressure":
                case "CasingPressure":
                case "TubingPressure":
                    unitType = UnitCategory.Pressure.Key;

                    break;
                default:
                    break;
            }

            return unitType;
        }

        #endregion

    }
}
