using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Plunger Lift Trend Data.
    /// </summary>
    public class PlungerLiftTrendData : TrendData
    {

        #region Private Members

        private enum LocalePhraseIDs
        {
            CasingPressure = 265, // Production Peak BOE   
            TubingPressure = 264, // Production Peak BOE   
            InjectionPressure = 20140, // Injection Pressure   
            DifferentialPressure = 20141, // Differential Pressure
            LinePressure = 20142, // Line Pressure
            LineTemperature = 20143, // Line Temperature
            UnitTemperature = 20144, //Unit Temperature
            BoardTemperature = 20145, //Board Temperature
            Battery = 20147, // Battery Voltage
            FCU_DifferentialPressure = 20195, // Weight
            FCU_LinePressure = 20196, // FCU Line Pressure
            FCU_LineTemperature = 20197, //FCU Line Temperature
            FCU_Rate = 20198, // FCU Rate
            FlowRate = 2994, // Flow Rate
        }

        #endregion

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public int UnitType { get; set; }

        #region Contructors

        /// <summary>
        /// Initializes a new PlungerLiftTrendData with a specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public PlungerLiftTrendData(string key)
            : base(key)
        {
            this.UnitType = 0;
        }

        /// <summary>
        /// Initializes a new PlungerLiftTrendData with node id, name, description.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public PlungerLiftTrendData(string nodeId, string name, string description)
            : base(name)
        {
            this.UnitType = 0;
            this.Name = name;
            if (!string.IsNullOrEmpty(description))
            {
                this.Description = description;
            }
            else
            {
                this.Description = name;
            }

            this.NodeId = nodeId;
        }

        /// <summary>
        /// Initializes a new PlungerLiftTrendData with node id, name, description and unit type.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="unitType">The unit type.</param>
        public PlungerLiftTrendData(string nodeId, string name, string description, int unitType)
            : this(nodeId, name, description)
        {
            this.UnitType = unitType;
        }

        #endregion

        /// <summary>
        /// Overridden method to get the Data Points in the given date range.
        /// </summary>
        /// <param name="startDate">The start Date.</param>
        /// <param name="endDate">The end Date.</param>
        /// <returns>The <seealso cref="IList{DataPoint}"/></returns>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();

            return data;
        }

        /// <summary>
        /// Gets the plunger lift trend data.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="phrases">The locale phrase dictionary.</param>
        /// <returns>An array of <seealso cref="PlungerLiftTrendData"/> items.</returns>
        public static PlungerLiftTrendData[] GetItems(string nodeId, short pocType, IDictionary<int, string> phrases)
        {
            if (phrases == null)
            {
                return null;
            }

            IList<PlungerLiftTrendData> dataTrends;

            if (pocType == (short)DeviceType.PCS_Ferguson_4000_Single_Well)
            {
                dataTrends = GetPcsSingleWellItems(nodeId, phrases);
            }
            else if (pocType == (short)DeviceType.PCS_Ferguson_Gas_Lift)
            {
                dataTrends = GetGasLiftItems(nodeId, phrases).ToList();
            }
            else
            {
                dataTrends = GetPlungerLiftTrendDataItems(nodeId, phrases).ToList();
            }

            var dataTrendsArray = dataTrends.ToArray();

            Array.Sort(dataTrendsArray);

            return dataTrendsArray;
        }

        #region Private Methods

        private static IList<PlungerLiftTrendData> GetPlungerLiftTrendDataItems(string nodeId, IDictionary<int, string> phrases)
        {
            return new List<PlungerLiftTrendData>
                {
                    new PlungerLiftTrendData(nodeId, "CasingPressure", phrases[(int)LocalePhraseIDs.CasingPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "TubingPressure", phrases[(int)LocalePhraseIDs.TubingPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "InjectionPressure", phrases[(int)LocalePhraseIDs.InjectionPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "DifferentialPressure", phrases[(int)LocalePhraseIDs.DifferentialPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "LinePressure", phrases[(int)LocalePhraseIDs.LinePressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "LineTemperature", phrases[(int)LocalePhraseIDs.LineTemperature], UnitCategory.Temperature.Key),
                    new PlungerLiftTrendData(nodeId, "UnitTemperature", phrases[(int)LocalePhraseIDs.UnitTemperature], UnitCategory.Temperature.Key),
                    new PlungerLiftTrendData(nodeId, "BoardTemperature", phrases[(int)LocalePhraseIDs.BoardTemperature], UnitCategory.Temperature.Key),
                    new PlungerLiftTrendData(nodeId, "Battery", phrases[(int)LocalePhraseIDs.Battery]),
                    new PlungerLiftTrendData(nodeId, "FCU_DifferentialPressure", phrases[(int)LocalePhraseIDs.FCU_DifferentialPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "FCU_LinePressure", phrases[(int)LocalePhraseIDs.FCU_LinePressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "FCU_LineTemperature", phrases[(int)LocalePhraseIDs.FCU_LineTemperature], UnitCategory.Temperature.Key),
                    new PlungerLiftTrendData(nodeId, "FCU_Rate", phrases[(int)LocalePhraseIDs.FCU_Rate], UnitCategory.GasRate.Key)
                };
        }

        private static IList<PlungerLiftTrendData> GetPcsSingleWellItems(string nodeId, IDictionary<int, string> phrases)
        {
            return new List<PlungerLiftTrendData>
                {
                    new PlungerLiftTrendData(nodeId, "CasingPressure", phrases[(int)LocalePhraseIDs.CasingPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "TubingPressure", phrases[(int)LocalePhraseIDs.TubingPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "LinePressure", phrases[(int)LocalePhraseIDs.LinePressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "FCU_Rate", phrases[(int)LocalePhraseIDs.FCU_Rate], UnitCategory.GasVolume.Key)
                };
        }

        private static IList<PlungerLiftTrendData> GetGasLiftItems(string nodeId, IDictionary<int, string> phrases)
        {
            // LinePressure is actually Production Temperature
            // FCU_Rate is actually Flow Rate
            return new List<PlungerLiftTrendData>
                {
                    new PlungerLiftTrendData(nodeId, "CasingPressure", phrases[(int)LocalePhraseIDs.CasingPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "TubingPressure", phrases[(int)LocalePhraseIDs.TubingPressure], UnitCategory.Pressure.Key),
                    new PlungerLiftTrendData(nodeId, "LinePressure", phrases[(int)LocalePhraseIDs.LinePressure], UnitCategory.Temperature.Key),
                    new PlungerLiftTrendData(nodeId, "FCU_Rate", phrases[(int)LocalePhraseIDs.FlowRate], UnitCategory.GasVolume.Key)
                };
        }

        #endregion

    }
}
