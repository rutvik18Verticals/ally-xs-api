using System.Linq;

namespace Theta.XSPOC.Apex.Api.Common.Converters
{
    /// <summary>
    /// Converter for phrases.
    /// </summary>
    public static class PhraseConverter
    {

        #region Private Members

        private static readonly string[] Abbreviations = { "IPR", "GL", "GLR" };

        #endregion

        /// <summary>
        /// Converts the first character of the input string to uppercase and the rest of the characters to lowercase.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ConvertFirstToUpperRestToLower(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            string firstCharUpper = string.Empty;
            string restLower = string.Empty;

            string[] words = input.Split(' ');
            bool hasAbbreviation = words.Any(word => Abbreviations.Contains(word));

            if (hasAbbreviation)
            {
                int i = 0;
                foreach (var word in words)
                {
                    if (Abbreviations.Contains(word))
                    {
                        words[i] = word; // Keep abbreviation unchanged
                        i++;
                        continue;
                    }

                    if (i == 0)
                    {
                        // Convert the first character to uppercase
                        firstCharUpper = word[..1].ToUpper();
                        // Convert the rest of the characters to lowercase
                        restLower = word[1..].ToLower();
                        words[i] = firstCharUpper + restLower;
                    }
                    else
                    {
                        words[i] = word.ToLower();
                    }

                    i++;
                }

                return string.Join(' ', words);
            }

            // Convert the first character to uppercase
            firstCharUpper = input[..1].ToUpper();

            // Convert the rest of the characters to lowercase
            restLower = input[1..].ToLower();

            // Concatenate the results
            return firstCharUpper + restLower;
        }

    }
}
