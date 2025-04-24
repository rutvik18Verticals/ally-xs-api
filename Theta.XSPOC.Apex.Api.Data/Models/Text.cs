using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents an object whose primary purpose is to contain text.
    /// </summary>
    public class Text : ValueContainerBase<string>
    {

        #region Static Properties

        /// <summary>
        /// Represents an empty text
        /// </summary>
        public static Text Empty { get; } = new Text(string.Empty);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Text with a specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <exception cref="ArgumentNullException">value is null</exception>
        public Text(string value)
            : base(value)
        {
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Initializes a new instance of the Text class with a specified value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>A new Text object</returns>
        /// <exception cref="ArgumentNullException">value is null</exception>
        public static Text FromString(string value)
        {
            return new Text(value);
        }

        #endregion

    }
}
