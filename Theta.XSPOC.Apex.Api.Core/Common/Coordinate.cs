namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// An (x, y) coordinate,
    /// </summary>
    /// <typeparam name="TX">The type for the x value</typeparam>
    /// <typeparam name="TY">The type for the y value</typeparam>
    public struct Coordinate<TX, TY>
        where TX : struct
        where TY : struct
    {

        /// <summary>
        /// Gets the x value
        /// </summary>
        public TX XValue { get; private set; }

        /// <summary>
        /// Get the y value
        /// </summary>
        public TY YValue { get; private set; }

        /// <summary>
        /// Initializes a coordinate with the specified x and y values
        /// </summary>
        /// <param name="x">Value of the x coordinate</param>
        /// <param name="y">Value of the y coordinate</param>
        public Coordinate(TX x, TY y)
            : this()
        {
            XValue = x;
            YValue = y;
        }

        #region Overridden Object Members

        /// <summary>
        /// Returns a string that represents this instance
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return $"({XValue}, {YValue})";
        }

        #endregion

    }
}
