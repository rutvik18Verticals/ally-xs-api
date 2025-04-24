using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the Analysis Result Curves Model.
    /// </summary>
    public class AnalysisResultCurvesModel
    {

        /// <summary>
        /// Gets or sets the Analysis Result Curve ID.
        /// </summary> 
        public int AnalysisResultCurveID { get; set; }

        /// <summary>
        /// Gets or sets the Curve Types ID.
        /// </summary> 
        public int CurveTypesID { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary> 
        public int Id { get; set; }

        /// <summary>
        /// The List of  Coordinates.
        /// </summary>
        public IList<Coordinates> Coordinates { get; set; }

    }

    /// <summary>
    /// Represent the class for Coordinates with X and Y coordinates.
    /// </summary>
    public class Coordinates
    {
        /// <summary>
        /// The  X Coordinate.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The  Y Coordinate.
        /// </summary>
        public float Y { get; set; }

    }
}