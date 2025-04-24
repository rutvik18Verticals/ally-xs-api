using System;

namespace Theta.XSPOC.Apex.Kernel.MathFunctions
{
    /// <summary>
    /// Provides various math operations.
    /// </summary>
    public static class MathUtility
    {

        /// <summary>
        /// Truncates a value to a specified number of significant digits.
        /// </summary>
        /// <param name="d">The value.</param>
        /// <param name="digits">The number of significant digits.</param>
        /// <returns>The value truncated to the specified number of significant digits.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits"/> is less than or equal to zero.
        /// </exception>
        public static double TruncateToSignificantDigits(double d, int digits)
        {
            if (digits <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(digits), $"{nameof(digits)} must be greater than zero.");
            }

            if (d == 0)
            {
                return 0;
            }

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1 - digits);
            return scale * Math.Truncate(d / scale);
        }

        /// <summary>
        /// Rounds a value to a specified number of significant digits.
        /// </summary>
        /// <param name="d">The value.</param>
        /// <param name="digits">The number of significant digits.</param>
        /// <returns>The value rounded to the specified number of significant digits.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits"/> is less than or equal to zero.
        /// </exception>
        public static double RoundToSignificantDigits(double d, int digits)
        {
            if (digits <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(digits), $"{nameof(digits)} must be greater than zero.");
            }

            if (d == 0)
            {
                return 0;
            }

            return RoundToSignificantDigits(d, digits, MidpointRounding.ToEven);
        }

        /// <summary>
        /// Rounds a value to a specified number of significant digits.
        /// </summary>
        /// <param name="d">The value.</param>
        /// <param name="digits">The number of significant digits.</param>
        /// <param name="mode">Indicates how to round the value if it is halfway between two numbers.</param>
        /// <returns>The value rounded to the specified number of significant digits.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits"/> is less than or equal to zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="mode"/> is not a valid value of <see cref="MidpointRounding"/>.
        /// </exception>
        public static double RoundToSignificantDigits(double d, int digits, MidpointRounding mode)
        {
            if (digits <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(digits), $"{nameof(digits)} must be greater than zero.");
            }

            if (d == 0)
            {
                return 0;
            }

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits, mode);
        }

        /// <summary>
        /// Generates a hash code for specified field values.
        /// </summary>
        /// <param name="fieldValues">The values of the fields that define equality</param>
        /// <returns>A hash code</returns>
        /// <exception cref="ArgumentNullException">fieldValues is null</exception>
        public static int GenerateHashCode(params object[] fieldValues)
        {
            if (fieldValues == null)
            {
                throw new ArgumentNullException("fieldValues");
            }

            //11, 17, and 23 are all prime numbers preferred for use with hashtables
            int hash = 17;

            foreach (var value in fieldValues)
            {
                //allow arithmetic overflow
                unchecked
                {
                    //distinguish between null and zero
                    if (value == null)
                    {
                        hash *= 11;
                    }
                    else
                    {
                        hash = hash * 23 + value.GetHashCode();
                    }// else
                }// unchecked
            }// foreach

            return hash;
        }
    }
}