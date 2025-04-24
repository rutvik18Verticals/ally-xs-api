using System;

namespace Theta.XSPOC.Apex.Kernel.MathFunctions
{
    /// <summary>
    /// Utility class to perform bit functions.
    /// </summary>
    public static class BitFunctions
    {

        #region Public Static Methods

        /// <summary>
        /// This method will return <c>true</c> or <c>false</c> depending on
        /// the value of the nth bit <paramref name="bit"/> of an integer <paramref name="word"/>
        /// <paramref name="bit"/> is expected to be a value from 1 to 32.
        /// </summary>
        /// <param name="word">The value to examine.</param>
        /// <param name="bit">The bit number to examine.</param>
        /// <returns>
        /// <c>true</c> if the nth <paramref name="bit"/> part of the <paramref name="word"/>
        /// is set to 1. <c>false</c> otherwise.
        /// </returns>
        public static bool ExamineBit(int word, int bit)
        {
            var exponent = bit - 1;
            int mask;

            if (bit == 32)
            {
                mask = -2147483648;
            }
            else
            {
                if (int.TryParse(Math.Pow(2, exponent).ToString(), out mask) == false)
                {
                    throw new ArithmeticException(
                        $"Unable to parse {Math.Pow(2, exponent).ToString()}");
                }
            }

            if (word < 0 && bit == 32)
            {
                return true;
            }

            return ((word & mask) > 0);
        }

        /// <summary>
        /// This method will return <c>true</c> or <c>false</c> depending on
        /// the value of the nth bit <paramref name="bit"/> of a long <paramref name="word"/>
        /// <paramref name="bit"/> is expected to be a value from 1 to 32.
        /// </summary>
        /// <param name="word">The value to examine.</param>
        /// <param name="bit">The bit number to examine.</param>
        /// <returns>
        /// <c>true</c> if the nth <paramref name="bit"/> part of the <paramref name="word"/>
        /// is set to 1. <c>false</c> otherwise.
        /// </returns>
        public static bool ExamineBit(long word, int bit)
        {
            var exponent = bit - 1;

            if (long.TryParse(Math.Truncate(Math.Pow(2, exponent)).ToString(), out var mask) == false)
            {
                throw new ArithmeticException(
                    $"Unable to parse {Math.Pow(2, exponent).ToString()}");
            }

            return (word & mask) > 0;
        }

        #endregion

    }
}
