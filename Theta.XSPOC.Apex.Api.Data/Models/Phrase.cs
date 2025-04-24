namespace Theta.XSPOC.Apex.Api.Data.Models
{

    /// <summary>
    /// Represents a phrase.
    /// </summary>
    public class Phrase : Text
    {
        #region Properties

        /// <summary>
        /// Gets the ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the text
        /// </summary>
        public string Text => Value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Phrase with a specified ID, language, and text
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="text">The text</param>
        public Phrase(int id, string text)
            : base(text)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new Phrase with a specified ID, language, and text
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="text">The text</param>
        /// <param name="setter">
        /// When this method returns, contains a delegate that can be used to modify this instance
        /// </param>
        public Phrase(int id, string text, out PhraseSetter setter)
            : this(id, text)
        {
            setter = new PhraseSetter(Modify);
        }

        #endregion

        #region Private Methods

        private void Modify(string text)
        {
            Value = text;
        }

        #endregion

        /// <summary>
        /// Encapsulates a method that allows a Phrase to be modified post-construction
        /// </summary>
        /// <param name="text">The text</param>
        public delegate void PhraseSetter(string text);
    }
}
