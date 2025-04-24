namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The represents curve set coordinates model.
    /// </summary>
    public class CurveSetCoordinatesModel
    {

        /// <summary>
        /// The id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The x.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The y.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The curve id.
        /// </summary>
        public int CurveId { get; set; }

    }
}
