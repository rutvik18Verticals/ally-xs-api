namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The class to represent component model.
    /// </summary>
    public class ComponentItemModel
    {

        #region Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the major component that failed
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The application type of the component
        /// </summary>
        public int? Application { get; set; }

        /// <summary>
        /// Gets or sets the component id.
        /// </summary>
        public int? ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        public int? PhraseId { get; set; }

        #endregion

    }
}
