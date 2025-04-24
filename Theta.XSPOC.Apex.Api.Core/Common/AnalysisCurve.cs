using System.Collections.Generic;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a generic analysis result curves.
    /// </summary>
    public abstract class AnalysisCurve : IdentityBase
    {

        private ICurveType _curveType;

        #region Properties

        /// <summary>
        /// Gets or sets the type of curve this analysis curve represents 
        /// </summary>
        public virtual ICurveType CurveType
        {
            get => _curveType;
            set => _curveType = value;
        }

        /// <summary>
        /// Gets or sets the curve this analysis curve represents
        /// </summary>
        public IList<CurveCoordinate> Curve { get; set; }

        /// <summary>
        /// Return the IndustryApplication of the analysis curve
        /// </summary>
        public abstract IndustryApplication IndustryApplication { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new analysis curve of a curve type
        /// </summary>
        public AnalysisCurve(ICurveType curveType)
        {
            Curve = new List<CurveCoordinate>();
            CurveType = curveType;
        }

        /// <summary>
        /// Initializes a new analysis with a specified ID and curve type
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="curveType">The type of analysis curve. Must be an ICurveType.</param>
        public AnalysisCurve(object id, ICurveType curveType)
            : base(id)
        {
            Curve = new List<CurveCoordinate>();
            CurveType = curveType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return the curve type id of the analysis curve
        /// </summary>
        public int GetCurveTypeId()
        {
            return ((EnhancedEnumBase)CurveType).Key;
        }

        /// <summary>
        /// Gets an IList of Coordinates for the curve
        /// </summary>
        public IList<Coordinate<double, double>> GetCurveCoordinateList()
        {
            if (Curve != null)
            {
                return (from coordinate in Curve select coordinate.Coordinate).ToList();
            }

            return null;
        }

        #endregion

    }
}
