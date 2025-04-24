using System.Collections.Generic;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the gas lift analysis output.
    /// </summary>
    public class GLAnalysisOutput : AnalysisOutputBase
    {

        #region Private Fields

        private readonly IDictionary<GLCurveType, GLAnalysisCurve> _analysisCurves;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a instance of GLAnalysisOutput.
        /// </summary>
        public GLAnalysisOutput()
        {
            _analysisCurves = new Dictionary<GLCurveType, GLAnalysisCurve>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// /// Gets or sets the flowing bottomhole pressure at injection depth
        /// </summary>
        public float? FlowingBHPAtInjectionDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the injected gas-liquid ratio
        /// </summary>
        public double? InjectedGLR { get; set; }

        /// <summary>
        /// FormationGOR is calculated in View
        /// </summary>
        public double? FormationGOR { get; set; }

        /// <summary>
        /// Gets or sets the max liquid rate
        /// </summary>
        public float? MaxLiquidRate { get; set; } // Volume

        /// <summary>
        /// Gets or sets the injection rate required to produce the max liquid rate
        /// </summary>
        public float? InjectionRateForMaxLiquidRate { get; set; } // Volume

        /// <summary>
        /// Gets or sets the GLR required to produce the max liquid rate
        /// </summary>
        public double? GLRForMaxLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the optimum liquid rate
        /// </summary>
        public float? OptimumLiquidRate { get; set; } // Volume

        /// <summary>
        /// Gets or sets the injection rate required to produce the optimum liquid rate
        /// </summary>
        public float? InjectionRateForOptimumLiquidRate { get; set; } // Volume

        /// <summary>
        /// Gets or sets the GLR required to produce the optimum liquid rate
        /// </summary>
        public double? GLRForOptimumLiquidRate { get; set; }

        /// <summary>
        /// Gets or sets the fbhp for optimum liquid rate
        /// </summary>
        public float? FBHPForOptimumLiquidRate { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the minimum flowing bottomhole pressure that can be produced
        /// </summary>
        public float? MinimumFBHP { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the kill fluid level.
        /// </summary>
        public float? KillFluidLevel { get; set; } // Length

        /// <summary>
        /// Gets or sets the reservoir fluid level.
        /// </summary>
        public float? ReservoirFluidLevel { get; set; } // Length

        /// <summary>
        /// Gets or sets the gross rate ( production liquid rate )
        /// </summary>
        public float? GrossRate { get; set; } // Volume

        /// <summary>
        /// Gets or sets the valve critical velocity
        /// </summary>
        public float? ValveCriticalVelocity { get; set; } // Volume

        /// <summary>
        /// Gets or sets the tubing critical velocity
        /// </summary>
        public float? TubingCriticalVelocity { get; set; } // Volume

        /// <summary>
        /// Gets or sets the injection rate required to maintain tubing critical velocity
        /// </summary>
        public float? InjectionRateForTubingCriticalVelocity { get; set; } // Volume

        /// <summary>
        /// Gets or sets whether the analysis was able to successfully adjust results based on the downhole gauge
        /// </summary>
        public bool AdjustedAnalysisToDownholeGaugeReading { get; set; }

        /// <summary>
        /// Gets or sets the flow control device statuses.
        /// </summary>
        public FlowControlDeviceStatuses FlowControlDeviceStatuses { get; set; }

        /// <summary>
        /// Gets or sets the valve injection depth estimate result data
        /// </summary>
        public ValveInjectionDepthEstimateResult ValveInjectionDepthEstimateResultData { get; set; }

        /// <summary>
        /// Gets or sets the production performance curve
        /// </summary>
        public GLAnalysisCurve ProductionPerformanceAnalysisCurve => GetAnalysisCurve(GLCurveType.ProductionPerformanceCurve);

        /// <summary>
        /// Gets or sets the pressure performance curve
        /// </summary>
        public GLAnalysisCurve PressurePerformanceAnalysisCurve => GetAnalysisCurve(GLCurveType.PressurePerformanceCurve);

        /// <summary>
        /// Gets or sets the gas injection curve
        /// </summary>
        public GLAnalysisCurve GasInjectionAnalysisCurve
            => GetAnalysisCurve(GLCurveType.GasInjectionCurve);

        /// <summary>
        /// Gets or sets the temperature curve
        /// </summary>
        public GLAnalysisCurve TemperatureAnalysisCurve
            => GetAnalysisCurve(GLCurveType.TemperatureCurve);

        /// <summary>
        /// Gets or sets the production fluid curve
        /// </summary>
        public GLAnalysisCurve ProductionFluidAnalysisCurve
            => GetAnalysisCurve(GLCurveType.ProductionFluidCurve);

        /// <summary>
        /// Gets or sets the flowing bottomhole pressure performance curve
        /// </summary>
        public GLAnalysisCurve FlowingBottomholePressurePerformanceAnalysisCurve
            => GetAnalysisCurve(GLCurveType.FlowingBottomholePressurePerformanceCurve);

        /// <summary>
        /// Gets the injection rate for critical velocity curve
        /// </summary>
        public GLAnalysisCurve InjectionRateForCriticalVelocityAnalysisCurve
            => GetAnalysisCurve(GLCurveType.InjectionRateForCriticalVelocityCurve);

        /// <summary>
        /// Retrieves the injection rate for critical velocity curve
        /// </summary>
        public IList<Coordinate<double, double>> InjectionRateForCriticalVelocityCurve
            => GetAnalysisCurve(GLCurveType.InjectionRateForCriticalVelocityCurve)?.GetCurveCoordinateList();

        /// <summary>
        /// Retrieves the temperature curve
        /// </summary>
        public IList<Coordinate<double, double>> TemperatureCurve
            => GetAnalysisCurve(GLCurveType.TemperatureCurve)?.GetCurveCoordinateList();

        /// <summary>
        /// Retrieves the production fluid curve
        /// </summary>
        public IList<Coordinate<double, double>> ProductionFluidCurve
            => GetAnalysisCurve(GLCurveType.ProductionFluidCurve)?.GetCurveCoordinateList();

        /// <summary>
        /// Retrieves the production performance curve
        /// </summary>
        public IList<Coordinate<double, double>> ProductionPerformanceCurve =>
            GetAnalysisCurve(GLCurveType.ProductionPerformanceCurve)?.GetCurveCoordinateList();

        /// <summary>
        /// Retrieves the pressure performance curve
        /// </summary>
        public IList<Coordinate<double, double>> PressurePerformanceCurve =>
            GetAnalysisCurve(GLCurveType.PressurePerformanceCurve)?.GetCurveCoordinateList();

        /// <summary>
        /// Retrieves the gas injection curve
        /// </summary>
        public IList<Coordinate<double, double>> GasInjectionCurve =>
            GetAnalysisCurve(GLCurveType.GasInjectionCurve)?.GetCurveCoordinateList();

        /// <summary>
        /// Retrieves the pressure performance curve
        /// </summary>
        public IList<Coordinate<double, double>> FlowingBottomholePressurePerformanceCurve =>
            GetAnalysisCurve(GLCurveType.FlowingBottomholePressurePerformanceCurve)?.GetCurveCoordinateList();

        /// <summary>
        /// Gets a list of analysis curves associated with this AnalysisOutput
        /// </summary>
        public IList<GLAnalysisCurve> AnalysisCurves
            => _analysisCurves.Values.ToList();

        /// <summary>
        /// Gets or sets the FBHP curves.
        /// </summary>
        /// <value>
        /// The FBHP curves.
        /// </value>
        public AnalysisCurveSet FBHPCurves { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds or updates a curve
        /// </summary>
        /// <param name="curve">The AnalysisCurve to add to this output, or to update if it already exists</param>
        public void SetAnalysisCurve(GLAnalysisCurve curve)
        {
            if (curve != null)
            {
                var curveType = (GLCurveType)curve.CurveType;

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
        public void SetAnalysisCurve(GLCurveType curveType, IList<Coordinate<double, double>> curve)
        {
            if (curve != null && curveType != null)
            {
                if (_analysisCurves.TryGetValue(curveType, out var value))
                {
                    value.Curve = UpdateCurve(_analysisCurves[curveType].Curve, curve);
                }
                else
                {
                    var analysisCurve = new GLAnalysisCurve(curveType);

                    analysisCurve.Curve = UpdateCurve(analysisCurve.Curve, curve);
                    _analysisCurves[curveType] = analysisCurve;
                }
            }
        }

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
            var truncateCurvePastIndex = false;
            foreach (var o in original)
            {
                if (updated.Count > (i + 1))
                {
                    o.Coordinate = updated[i];
                    i++;
                }
                else
                {
                    truncateCurvePastIndex = true;
                    break;
                }
            }

            if (truncateCurvePastIndex)
            {
                original = original.Take(i).ToList();
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

        /// <summary>
        /// Removes a curve, if it exists
        /// </summary>
        /// <param name="curve">The AnalysisCurve to try to remove</param>
        public void RemoveAnalysisCurve(GLAnalysisCurve curve)
        {
            if (curve != null)
            {
                _analysisCurves.Remove((GLCurveType)curve.CurveType);
            }
        }

        /// <summary>
        /// Removes a type of curve, if it exists, from the AnalysisOutput
        /// </summary>
        /// <param name="curveType">The curve type to try to remove</param>
        public void RemoveAnalysisCurve(GLCurveType curveType)
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
        public GLAnalysisCurve GetAnalysisCurve(GLCurveType curveType)
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
        /// Creates the analysis curve set.
        /// </summary>
        /// <param name="source">The anlysis result source.</param>
        /// <param name="curveType">The curve set type.</param>
        /// <param name="fbhpCurves">The FBHP curves.</param>
        /// <param name="primaryGasLiquidRatio">The primary gas liquid ratio, if available</param>
        public void CreateAnalysisCurveSet(AnalysisResultSource source,
            CurveSetType curveType, IDictionary<double, IList<Coordinate<double, double>>> fbhpCurves,
            double? primaryGasLiquidRatio)
        {
            FBHPCurves = new AnalysisCurveSet()
            {
                AnalysisResultSource = source,
                CurveSetType = curveType,
                Curves = new List<AnalysisCurveSetMemberBase>(),
            };

            if (fbhpCurves == null)
            {
                return;
            }

            IList<CurveCoordinate> curveCoordinateList;
            foreach (var curves in fbhpCurves)
            {
                curveCoordinateList = curves.Value.Select(x => new CurveCoordinate()
                {
                    Coordinate = x
                }).ToList();

                var annotatedFlowingBottomholePressureCurve = new GasLiquidRatioCurve()
                {
                    Curve = curveCoordinateList,
                    AnnotationData = new GasLiquidRatioCurveAnnotation()
                    {
                        GasLiquidRatio = curves.Key,
                        IsPrimaryCurve = (curves.Key == primaryGasLiquidRatio),
                    },
                };

                FBHPCurves.Curves.Add(annotatedFlowingBottomholePressureCurve);
            }
        }

        #endregion

    }
}
