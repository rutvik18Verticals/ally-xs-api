using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{

    /// <summary>
    /// Base class for column formatters.
    /// </summary>
    public abstract class ColumnFormatterBase
    {

        /// <summary>
        /// Converts a color to its hexadecimal representation.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The hexadecimal representation of the color.</returns>
        protected string ConvertToHex(Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// Converts an argb to its hexadecimal representation.
        /// </summary>
        /// <param name="argb">The argb to convert.</param>
        /// <returns>The hexadecimal representation of the argb.</returns>
        protected string ConvertToHex(int argb)
        {
            var color = Color.FromArgb(argb);
            return ConvertToHex(color);
        }

        /// <summary>
        /// Converts an ABGR value to its ARGB representation.
        /// </summary>
        /// <param name="bgrValue">The ABGR value to convert.</param>
        /// <returns>The ARGB representation of the ABGR value.</returns>
        protected string ConvertABGRtoARGB(int bgrValue)
        {
            string hexValue = bgrValue.ToString("X6");

            int blueLevel = int.Parse(hexValue[..2], System.Globalization.NumberStyles.HexNumber);
            int greenLevel = int.Parse(hexValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int redLevel = int.Parse(hexValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            var color = Color.FromArgb(redLevel, greenLevel, blueLevel);

            return ConvertToHex(color);
        }

        /// <summary>
        /// Determines whether the specified data row is enabled.
        /// </summary>
        /// <param name="dr">The data row to check.</param>
        /// <returns><c>true</c> if the data row is enabled; otherwise, <c>false</c>.</returns>
        protected bool IsEnabled(IDictionary<string, object> dr)
        {
            var enabled = dr.FirstOrDefault(x => x.Key.Equals("Enabled", StringComparison.OrdinalIgnoreCase));

            return enabled.Value?.ToString() == "True";
        }

        /// <summary>
        /// Retrieves the value associated with the specified key from the data row.
        /// </summary>
        /// <param name="dr">The data row to retrieve the value from.</param>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified key.</returns>
        protected string GetKey(IDictionary<string, object> dr, string key)
        {
            return dr.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Key;
        }

    }
}
