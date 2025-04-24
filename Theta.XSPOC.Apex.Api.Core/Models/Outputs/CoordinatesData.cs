namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// A generic class for representing coordinates.
    /// </summary>
    public class CoordinatesData<T> where T : struct
    {

        /// <summary>
        /// The coordinate x.
        /// </summary>
        public T X { get; set; }

        /// <summary>
        /// The coordinate y.
        /// </summary>
        public T Y { get; set; }

    }
}
