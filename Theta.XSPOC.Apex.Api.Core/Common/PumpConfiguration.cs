using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Common.Interface;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the class comparing pump configurations.
    /// </summary>
    public class PumpConfiguration : IEquatable<PumpConfiguration>
    {

        /// <summary>
        /// The configured pump.
        /// </summary>
        public Pump Pump { get; set; }

        /// <summary>
        /// The stage count associated with the pump.
        /// </summary>
        public int? NumberOfStages { get; set; }

        #region Overridden Object Members

        /// <summary>
        /// Returns a string that represents this instance
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return Pump?.ToString() ?? string.Empty;
        }

        #endregion

        #region IEquatable<PumpConfiguration> Members

        /// <summary>
        /// Determines whether field-level equality exists.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>True if both objects have the same field values, false otherwise.</returns>
        public bool Equals(PumpConfiguration other)
        {
            if (other == null)
            {
                return false;
            }

            return Pump == other.Pump && NumberOfStages == other.NumberOfStages;
        }

        /// <summary>
        /// Gets the hash code for a PumpConfiguration object for comparison.
        /// </summary>
        /// <param name="obj">The <see cref="PumpConfiguration"/> object.</param>
        /// <returns>The hash code of the given object.</returns>
        public int GetHashCode(PumpConfiguration obj)
        {
            return MathUtility.GenerateHashCode(obj?.Pump, obj?.NumberOfStages);
        }

        /// <summary>
        /// Determines whether the objects equal each other.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><c>True</c> if the objects equal other; otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as PumpConfiguration);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code of the given object.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion

    }

    /// <summary>
    /// Represent the class for pump.
    /// </summary>
    public class Pump : IdentityBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Series (This is the OD of the equipment.)
        /// </summary>
        public string Series { get; set; }

        /// <summary>      
        /// Gets or sets the pump model (This is the description.)
        /// </summary>
        public string PumpModel { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer
        /// </summary>
        public ESPManufacturerModel Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the model name
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the minimum casing diameter
        /// </summary>
        public float MinCasingDiameter { get; set; }

        /// <summary>
        /// Gets or sets the housing pressure limit
        /// </summary>
        public float HousingPressureLimit { get; set; }

        /// <summary>
        /// Gets or sets the minimum daily volume
        /// </summary>
        public float MinDailyVolume { get; set; }

        /// <summary>
        /// Gets or sets the maximum daily volume
        /// </summary>
        public float MaxDailyVolume { get; set; }

        /// <summary>
        /// Gets or sets the BEP daily volume
        /// </summary>
        public float BEPDailyVolume { get; set; }

        /// <summary>
        /// Gets or sets the source for measuring performance
        /// </summary>
        public IPumpPerformanceSource PerformanceSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pump was created by the user.
        /// </summary>
        public bool IsCustom { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Pump with a default ID
        /// </summary>
        public Pump()
        {
        }

        /// <summary>
        /// Initializes a new Pump with a specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        public Pump(object id)
            : base(id)
        {
        }

        #endregion

        #region Overridden Object Members

        /// <summary>
        /// Returns a string that represents this instance
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return $"{Manufacturer?.Manufacturer} {Model} {Name}";
        }

        #endregion

    }

    /// <summary>
    /// Represent the class for pump curve point list.
    /// </summary>
    public class PumpCurvePointList : IPumpPerformanceSource, IList<PumpCurvePoint>
    {

        #region Enums

        private enum CurveType
        {

            Head,
            Power,
            Efficiency,

        }

        #endregion

        #region Fields

        private readonly IList<PumpCurvePoint> _curvePoints;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new PumpCurvePointList.
        /// </summary>
        public PumpCurvePointList()
        {
            _curvePoints = new List<PumpCurvePoint>();
        }

        /// <summary>
        /// Initializes a new PumpCurvePointList with specified curve points.
        /// </summary>
        /// <param name="curvePoints">The curve points to assign to the list.</param>
        public PumpCurvePointList(IList<PumpCurvePoint> curvePoints)
        {
            _curvePoints = new List<PumpCurvePoint>(curvePoints);
        }

        #endregion

        #region IList<PumpCurvePoint> Members

        /// <summary>
        /// Determines the index of a specified item in the list.
        /// </summary>
        /// <param name="item">The item to locate.</param>
        /// <returns>The index of the item if found; otherwise, -1.</returns>
        public int IndexOf(PumpCurvePoint item)
        {
            return _curvePoints.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item into the list at a specified index.
        /// </summary>
        /// <param name="index">The index at which the item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException">index is not valid.</exception>
        public void Insert(int index, PumpCurvePoint item)
        {
            _curvePoints.Insert(index, item);
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">index is not valid.</exception>
        public void RemoveAt(int index)
        {
            _curvePoints.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is not valid.</exception>
        public PumpCurvePoint this[int index]
        {
            get => _curvePoints[index];
            set => _curvePoints[index] = value;
        }

        #endregion

        #region ICollection<PumpCurvePoint> Members

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(PumpCurvePoint item)
        {
            _curvePoints.Add(item);
        }

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        public void Clear()
        {
            _curvePoints.Clear();
        }

        /// <summary>
        /// Determines whether the list contains a specific item.
        /// </summary>
        /// <param name="item">The item to locate.</param>
        /// <returns>true if the item is found; otherwise, false.</returns>
        public bool Contains(PumpCurvePoint item)
        {
            return _curvePoints.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the list to an array starting at a specified index.
        /// </summary>
        /// <param name="array">The destination array of the elements.</param>
        /// <param name="arrayIndex">The index in array at which copying begins.</param>
        public void CopyTo(PumpCurvePoint[] array, int arrayIndex)
        {
            _curvePoints.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements.
        /// </summary>
        public int Count => _curvePoints.Count;

        /// <summary>
        /// Gets a value indicating whether the list is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Removes the first occurrence of a specified item from the list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public bool Remove(PumpCurvePoint item)
        {
            return _curvePoints.Remove(item);
        }

        #endregion

        #region IEnumerable<PumpCurvePoint> Members

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<PumpCurvePoint> GetEnumerator()
        {
            return _curvePoints.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _curvePoints.GetEnumerator();
        }

        #endregion

        #region Implementation of IPumpPerformanceSource

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Pump Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Pump Curve</returns>
        public Coefficients GetPressureCoefficients()
        {
            // The result we'll be returning
            var result = GetCoefficients(CurveType.Head);
            return result;
        }

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Power Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Power Curve</returns>
        public Coefficients GetPowerCoefficients()
        {
            var result = GetCoefficients(CurveType.Power);
            return result;
        }

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Efficiency Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Efficiency Curve</returns>
        public Coefficients GetEfficiencyCoefficients()
        {
            var result = GetCoefficients(CurveType.Efficiency);
            return result;
        }

        #endregion

        #region Private Methods

        private Coefficients GetCoefficients(CurveType type)
        {
            Coefficients result = null;

            // Lists to store the x and y values that will be used by the Fit.Polynomial function
            var xValues = new List<double>();
            var yValues = new List<double>();

            IEnumerable<PumpCurvePoint> orderedCurvePoints = _curvePoints.OrderBy(c => c.FlowRate);

            // If there aren't at least 6 coordinates, then we can't calculate this.
            switch (type)
            {
                case CurveType.Head:
                    if (_curvePoints.Count(c => c.Head != null) < 5)
                    {
                        return null;
                    }

                    // Extract the x and y values
                    foreach (var curvePoint in orderedCurvePoints.Where(curvePoint => curvePoint.Head != null))
                    {
                        xValues.Add(curvePoint.FlowRate.Value);
                        yValues.Add(curvePoint.Head.Value);
                    }

                    AddYInterceptCoordinatesIfNecessary(ref xValues, ref yValues);

                    break;

                case CurveType.Power:
                    if (_curvePoints.Count(c => c.Power != null) < 5)
                    {
                        return null;
                    }

                    // Extract the x and y values
                    foreach (var curvePoint in orderedCurvePoints.Where(curvePoint => curvePoint.Power != null))
                    {
                        xValues.Add(curvePoint.FlowRate.Value);
                        yValues.Add(curvePoint.Power.Value);
                    }

                    AddYInterceptCoordinatesIfNecessary(ref xValues, ref yValues);

                    break;
                case CurveType.Efficiency:
                    if (_curvePoints.Count(c => c.EfficiencyPercent.HasValue) < 5)
                    {
                        return null;
                    }

                    // Extract the x and y values
                    foreach (var curvePoint in
                             orderedCurvePoints.Where(curvePoint => curvePoint.EfficiencyPercent.HasValue))
                    {
                        xValues.Add(curvePoint.FlowRate.Value);
                        yValues.Add(curvePoint.EfficiencyPercent.Value);
                    }

                    if (xValues[0] > 50)
                    {
                        xValues.Insert(0, 0);
                        yValues.Insert(0, 0);
                    }

                    break;
                default:
                    break;
            }

            if (xValues.Count == 5)
            {
                return null;
            }

            // There's no need to catch the ArgumentException from this since we verified that there are at least
            // 6 curve points earlier in this method.

            var calculatedCoefficients = Fit.Polynomial(xValues.ToArray(), yValues.ToArray(), 5);
            result = new Coefficients
            {
                Intercept = calculatedCoefficients[0],
                FirstOrder = calculatedCoefficients[1],
                SecondOrder = calculatedCoefficients[2],
                ThirdOrder = calculatedCoefficients[3],
                FourthOrder = calculatedCoefficients[4],
                FifthOrder = calculatedCoefficients[5],
            };

            return result;
        }

        private void AddYInterceptCoordinatesIfNecessary(ref List<double> xValues, ref List<double> yValues)
        {
            if (xValues[0] > 50)
            {
                //because the xValues have been inserted in order by flowrate, the first two points
                //are assumed to be the two lowest values for flowrate.
                var xValuesInterp = new double[]
                {
                    xValues[0], xValues[1]
                };
                var yValuesInterp = new double[]
                {
                    yValues[0], yValues[1]
                };

                var interpolation = Interpolate.Linear(xValuesInterp, yValuesInterp);
                xValues.Insert(0, 0);
                yValues.Insert(0, interpolation.Interpolate(0));
            }
        }

        #endregion

    }

    /// <summary>
    /// Represent the class for pump curve point.
    /// </summary>
    public class PumpCurvePoint : IdentityBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the flow rate per day
        /// </summary>
        public double? FlowRate { get; set; }

        /// <summary>
        /// Gets or sets the head
        /// </summary>
        public double? Head { get; set; }

        /// <summary>
        /// Gets or sets the power
        /// </summary>
        public double? Power { get; set; }

        /// <summary>
        /// Gets or sets the efficiency percent
        /// </summary>
        public double? EfficiencyPercent { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new PumpCurvePoint with a default ID
        /// </summary>
        public PumpCurvePoint()
        {
        }

        /// <summary>
        /// Initializes a new PumpCurvePoint with a specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        public PumpCurvePoint(object id)
            : base(id)
        {
        }

        #endregion

    }

    /// <summary>
    /// Represent the class for pump co-efficients.
    /// </summary>
    public class PumpCoefficients : IPumpPerformanceSource
    {

        #region Properties

        /// <summary>
        /// Gets or sets the pressure coefficients.
        /// </summary>
        public Coefficients PressureCoefficients { get; set; }

        /// <summary>
        /// Gets or sets the power coefficients.
        /// </summary>
        public Coefficients PowerCoefficients { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficients.
        /// </summary>
        public Coefficients EfficiencyCoefficients { get; set; }

        #endregion

        #region Implementation of IPumpPerformanceSource

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Pump Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Pump Curve</returns>
        public Coefficients GetPressureCoefficients()
        {
            return PressureCoefficients;
        }

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Power Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Power Curve</returns>
        public Coefficients GetPowerCoefficients()
        {
            return PowerCoefficients;
        }

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Efficiency Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Efficiency Curve</returns>
        public Coefficients GetEfficiencyCoefficients()
        {
            return EfficiencyCoefficients;
        }

        #endregion

    }

}
