using System.Collections.Generic;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the outputs for an ESP analysis result.
    /// </summary>
    public class AnalysisOutput : AnalysisOutputBase
    {

        #region Private Fields

        private readonly IDictionary<ESPCurveType, ESPAnalysisCurve> _analysisCurves;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisOutput"/>.
        /// </summary>
        public AnalysisOutput()
        {
            _analysisCurves = new Dictionary<ESPCurveType, ESPAnalysisCurve>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pressure across pump
        /// </summary>
        public float? PressureAcrossPump { get; set; }

        /// <summary>
        /// Gets or sets the head across pump
        /// </summary>
        public float? HeadAcrossPump { get; set; }

        /// <summary>
        /// Gets or sets the pump discharge pressure
        /// </summary>
        public float? PumpDischargePressure { get; set; }

        /// <summary>
        /// Gets or sets the frictional loss in tubing
        /// </summary>
        public float? FrictionalLossInTubing { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency
        /// </summary>
        public double? PumpEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the calculated fluid level above pump
        /// </summary>
        public float? CalculatedFluidLevelAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the fluid specific gravity
        /// </summary>
        public float? FluidSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the pump static pressure
        /// </summary>
        public float? PumpStaticPressure { get; set; }

        /// <summary>
        /// Gets or sets the maximum running frequency.
        /// </summary>
        public float? MaxRunningFrequency { get; set; }

        /// <summary>
        /// Gets or sets the motor load percentage.
        /// </summary>
        public float? MotorLoadPercentage { get; set; }

        /// <summary>
        /// Gets or sets the gas handling outputs
        /// </summary>
        public GasHandlingAnalysisOutput GasHandlingOutputs { get; set; }

        /// <summary>
        /// The diagnostics performed on the analysis results
        /// </summary>
        public AnalysisDiagnostics Diagnostics { get; set; }

        /// <summary>
        /// Gets or sets the efficiency curve
        /// </summary>
        public ESPAnalysisCurve EfficiencyAnalysisCurve
            => GetAnalysisCurve(ESPCurveType.EfficiencyCurve);

        /// <summary>
        /// Gets or sets the power curve
        /// </summary>
        public ESPAnalysisCurve PowerAnalysisCurve
            => GetAnalysisCurve(ESPCurveType.PowerCurve);

        /// <summary>
        /// Gets or sets the pump curve
        /// </summary>
        public ESPAnalysisCurve PumpAnalysisCurve
            => GetAnalysisCurve(ESPCurveType.PumpCurve);

        /// <summary>
        /// Gets or sets the recommended range bottom curve
        /// </summary>
        public ESPAnalysisCurve RecommendedRangeBottomAnalysisCurve
            => GetAnalysisCurve(ESPCurveType.RecommendedRangeBottom);

        /// <summary>
        /// Gets or sets the recommended range top curve
        /// </summary>
        public ESPAnalysisCurve RecommendedRangeTopAnalysisCurve
            => GetAnalysisCurve(ESPCurveType.RecommendedRangeTop);

        /// <summary>
        /// Gets or sets the recommended range right curve
        /// </summary>
        public ESPAnalysisCurve RecommendedRangeRightAnalysisCurve
            => GetAnalysisCurve(ESPCurveType.RecommendedRangeRight);

        /// <summary>
        /// Gets or sets the recommended range left curve
        /// </summary>
        public ESPAnalysisCurve RecommendedRangeLeftAnalysisCurve
            => GetAnalysisCurve(ESPCurveType.RecommendedRangeLeft);

        /// <summary>
        /// Gets or sets the well performance curve
        /// </summary>
        public ESPAnalysisCurve WellPerformanceAnalysisCurve
            => GetAnalysisCurve(ESPCurveType.WellPerformanceCurve);

        /// <summary>
        /// Gets or sets the efficiency curve
        /// </summary>
        public IList<Coordinate<double, double>> EfficiencyCurve
            => GetAnalysisCurve(ESPCurveType.EfficiencyCurve).GetCurveCoordinateList();

        /// <summary>
        /// Gets or sets the power curve
        /// </summary>
        public IList<Coordinate<double, double>> PowerCurve
            => GetAnalysisCurve(ESPCurveType.PowerCurve).GetCurveCoordinateList();

        /// <summary>
        /// Gets or sets the pump curve
        /// </summary>
        public IList<Coordinate<double, double>> PumpCurve
            => GetAnalysisCurve(ESPCurveType.PumpCurve).GetCurveCoordinateList();

        /// <summary>
        /// Gets or sets the recommended range bottom curve
        /// </summary>
        public IList<Coordinate<double, double>> RecommendedRangeBottomCurve
            => GetAnalysisCurve(ESPCurveType.RecommendedRangeBottom).GetCurveCoordinateList();

        /// <summary>
        /// Gets or sets the recommended range top curve
        /// </summary>
        public IList<Coordinate<double, double>> RecommendedRangeTopCurve
            => GetAnalysisCurve(ESPCurveType.RecommendedRangeTop).GetCurveCoordinateList();

        /// <summary>
        /// Gets or sets the recommended range right curve
        /// </summary>
        public IList<Coordinate<double, double>> RecommendedRangeRightCurve
            => GetAnalysisCurve(ESPCurveType.RecommendedRangeRight).GetCurveCoordinateList();

        /// <summary>
        /// Gets or sets the reccommended range left curve
        /// </summary>
        public IList<Coordinate<double, double>> RecommendedRangeLeftCurve
            => GetAnalysisCurve(ESPCurveType.RecommendedRangeLeft).GetCurveCoordinateList();

        /// <summary>
        /// Gets or sets the well performance curve
        /// </summary>
        public IList<Coordinate<double, double>> WellPerformanceCurve
            => GetAnalysisCurve(ESPCurveType.WellPerformanceCurve).GetCurveCoordinateList();

        /// <summary>
        /// Gets a list of analysis curves associated with this AnalysisOutput
        /// </summary>
        public IList<ESPAnalysisCurve> AnalysisCurves
            => _analysisCurves.Values.ToList();

        /// <summary>
        /// Gets or sets the Tornado curves.
        /// </summary>
        /// <value>
        /// The Tornado curves.
        /// </value>
        public AnalysisCurveSet TornadoCurves { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds or updates a curve
        /// </summary>
        /// <param name="curve">The AnalysisCurve to add to this output, or to update if it already exists</param>
        public void SetAnalysisCurve(ESPAnalysisCurve curve)
        {
            if (curve != null)
            {
                var curveType = (ESPCurveType)curve.CurveType;

                if (_analysisCurves.TryGetValue(curveType, out var value))
                {
                    value.Curve = UpdateCurve(_analysisCurves[curveType].Curve, curve.Curve);
                }
                else
                {
                    _analysisCurves.Add(curveType, curve);
                }
            }
        }

        /// <summary>
        /// Adds or updates the analysis curve.
        /// </summary>
        /// <param name="curveType">Type of the curve.</param>
        /// <param name="curve">The curve coordinates</param>
        public void SetAnalysisCurve(ESPCurveType curveType, IList<Coordinate<double, double>> curve)
        {
            if (curve != null && curveType != null)
            {
                if (_analysisCurves.TryGetValue(curveType, out var value))
                {
                    value.Curve = UpdateCurve(_analysisCurves[curveType].Curve, curve);
                }
                else
                {
                    var analysisCurve = new ESPAnalysisCurve(curveType);
                    analysisCurve.CurveType = curveType;
                    analysisCurve.Curve = UpdateCurve(analysisCurve.Curve, curve);
                    _analysisCurves[curveType] = analysisCurve;
                }
            }
        }

        /// <summary>
        /// Removes a curve, if it exists
        /// </summary>
        /// <param name="curve">The AnalysisCurve to try to remove</param>
        public void RemoveAnalysisCurve(ESPAnalysisCurve curve)
        {
            if (curve != null)
            {
                _analysisCurves.Remove((ESPCurveType)curve.CurveType);
            }
        }

        /// <summary>
        /// Removes a type of curve, if it exists, from the AnalysisOutput
        /// </summary>
        /// <param name="curveType">The curve type to try to remove</param>
        public void RemoveAnalysisCurve(ESPCurveType curveType)
        {
            if (curveType != null)
            {
                _analysisCurves.Remove(curveType);
            }
        }

        /// <summary>
        /// Returns an AnalysisCurve of the requested type, if it exists. Otherwise returns null
        /// </summary>
        /// <param name="curveType">The curve type to try to retrieve</param>
        public ESPAnalysisCurve GetAnalysisCurve(ESPCurveType curveType)
        {
            if (curveType != null && _analysisCurves.TryGetValue(curveType, out var value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates the tornado curve set.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="curveType">Type of the curve.</param>
        /// <param name="tornadoCurves">The tornado curves.</param>
        public void CreateTornadoCurveSet(AnalysisResultSource source,
            CurveSetType curveType, IDictionary<double, IList<Coordinate<double, double>>> tornadoCurves)
        {
            TornadoCurves = new AnalysisCurveSet()
            {
                AnalysisResultSource = source,
                CurveSetType = curveType,
                Curves = new List<AnalysisCurveSetMemberBase>(),
            };

            if (tornadoCurves == null)
            {
                return;
            }

            IList<CurveCoordinate> curveCoordinateList;
            foreach (var curves in tornadoCurves)
            {
                curveCoordinateList = curves.Value.Select(x => new CurveCoordinate()
                {
                    Coordinate = x
                }).ToList();

                var annotatedTornadoCurve = new TornadoCurve()
                {
                    Curve = curveCoordinateList,
                    AnnotationData = new TornadoCurveAnnotation()
                    {
                        Frequency = curves.Key,
                    },
                };

                TornadoCurves.Curves.Add(annotatedTornadoCurve);
            }
        }

        #endregion

        #region Private Methods

        private IList<CurveCoordinate> UpdateCurve(IList<CurveCoordinate> original, IList<CurveCoordinate> updated)
        {
            //Replace coordinates in the original curve with coordinates from the updated curve. 
            //If we run out of updated coordinates before we're done with the original list, 
            //we remove whatever is left since the new curve is shorter than the original curve
            var i = 0;
            foreach (var o in original)
            {
                if (updated.Count > (i + 1))
                {
                    o.Coordinate = new Coordinate<double, double>(updated[i].Coordinate.XValue, updated[i].Coordinate.YValue);
                    i++;
                }
                else
                {
                    original.Remove(o);
                }
            }

            //If we have coordinates left in the new list after we replaced all the coordinates in the original list,
            //the new list is longer than the original list and we add the remainder to the end of the original list
            if (i == original.Count)
            {
                for (var j = i; j < updated.Count; j++)
                {
                    original.Add(updated[j]);
                }
            }

            return original;
        }

        private IList<CurveCoordinate> UpdateCurve(IList<CurveCoordinate> original, IList<Coordinate<double, double>> updated)
        {
            //Replace coordinates in the original curve with coordinates from the updated curve. 
            //If we run out of updated coordinates before we're done with the original list, 
            //we remove whatever is left since the new curve is shorter than the original curve
            var i = 0;
            foreach (var o in original)
            {
                if (updated.Count > (i + 1))
                {
                    o.Coordinate = updated[i];
                    i++;
                }
                else
                {
                    original.Remove(o);
                }
            }

            //If we have coordinates left in the new list after we replaced all the coordinates in the original list,
            //the new list is longer than the original list and we add the remainder to the end of the original list
            if (i == original.Count)
            {
                for (var j = i; j < updated.Count; j++)
                {
                    var coord = new CurveCoordinate()
                    {
                        Coordinate = updated[j],
                    };
                    original.Add(coord);
                }
            }

            return original;
        }

        #endregion

    }
}
