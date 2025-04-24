using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Provides various math operations.
    /// </summary>
    public static class MathUtility
    {

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
                throw new ArgumentNullException(nameof(fieldValues));
            }

            //11, 17, and 23 are all prime numbers preferred for use with hashtables
            var hash = 17;

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
                    } // else
                } // unchecked
            } // foreach

            return hash;
        }

        /// <summary>
        /// Rounds the <paramref name="d"/> to the number of <paramref name="digits"/>
        /// </summary>
        /// <param name="d">The value.</param>
        /// <param name="digits">The number of digits.</param>
        /// <returns>The new rounded value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits"/> less than or equal to 0.</exception>
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
        /// Rounds the <paramref name="d"/> to the number of <paramref name="digits"/>
        /// </summary>
        /// <param name="d">The value.</param>
        /// <param name="digits">The number of digits.</param>
        /// <returns>The new rounded value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="digits"/> less than or equal to 0.</exception>
        public static double? RoundToSignificantDigits(double? d, int digits)
        {
            if (d == null)
            {
                return null;
            }

            if (digits <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(digits), $"{nameof(digits)} must be greater than zero.");
            }

            if (d == 0)
            {
                return 0;
            }

            return RoundToSignificantDigits((double)d, digits, MidpointRounding.ToEven);
        }

        private static double RoundToSignificantDigits(double d, int digits, MidpointRounding mode)
        {
            if (digits <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(digits), $"{nameof(digits)} must be greater than zero.");
            }

            if (d == 0)
            {
                return 0;
            }

            int scale = (int)Math.Floor(Math.Log10(Math.Abs(d))) + 1;

            if (scale >= digits)
            {
                return Math.Round(d, 0, mode);
            }

            double factor = Math.Pow(10, digits - scale);

            double roundedValue = Math.Round(d * factor, 0, mode) / factor;

            return roundedValue;
        }

    }
}
