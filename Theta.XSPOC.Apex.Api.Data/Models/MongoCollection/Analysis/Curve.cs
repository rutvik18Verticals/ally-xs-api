using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// Represents a curve.
    /// </summary>
    public class Curve
    {
        /// <summary>
        /// Gets or sets the CurveType.
        /// </summary>
        public string CurveType { get; set; }

        /// <summary>
        /// Gets or sets the Points.
        /// </summary>
        public IList<Point> Points { get; set; }
    }
}