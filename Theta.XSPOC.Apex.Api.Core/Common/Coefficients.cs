using Theta.XSPOC.Apex.Api.Core.Common.Interface;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a set of coefficients.
    /// </summary>
    /// <seealso cref="ICoefficients" />
    public class Coefficients : IdentityBase, ICoefficients
    {

        #region Properties

        /// <summary>
        /// Gets or sets the intercept.
        /// </summary>
        public double Intercept { get; set; }

        /// <summary>
        /// Gets or sets the first-order coefficient.
        /// </summary>
        public double FirstOrder { get; set; }

        /// <summary>
        /// Gets or sets the second-order coefficient.
        /// </summary>
        public double SecondOrder { get; set; }

        /// <summary>
        /// Gets or sets the third-order coefficient.
        /// </summary>
        public double ThirdOrder { get; set; }

        /// <summary>
        /// Gets or sets the fourth-order coefficient.
        /// </summary>
        public double FourthOrder { get; set; }

        /// <summary>
        /// Gets or sets the fifth-order coefficient.
        /// </summary>
        public double FifthOrder { get; set; }

        /// <summary>
        /// Gets or sets the sixth-order coefficient.
        /// </summary>
        public double? SixthOrder { get; set; }

        /// <summary>
        /// Gets or sets the seventh-order coefficient.
        /// </summary>
        public double? SeventhOrder { get; set; }

        /// <summary>
        /// Gets or sets the eighth-order coefficient.
        /// </summary>
        public double? EighthOrder { get; set; }

        /// <summary>
        /// Gets or sets the ninth-order coefficient.
        /// </summary>
        public double? NinthOrder { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Coefficients with a default ID.
        /// </summary>
        public Coefficients()
        {
        }

        /// <summary>
        /// Initializes a new Coefficients with a specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        public Coefficients(object id)
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
            return $"Intercept = {Intercept}, 1st = {FirstOrder}, 2nd = {SecondOrder}, " +
                $"3rd = {ThirdOrder}, 4th = {FourthOrder}, 5th = {FifthOrder}, 6th = {SixthOrder}, " +
                $"7th = {SeventhOrder}, 8th = {EighthOrder}, 9th = {NinthOrder}";
        }

        #endregion

    }
}
