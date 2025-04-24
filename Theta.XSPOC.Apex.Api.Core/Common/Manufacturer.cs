namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the gas lift manufacturer.
    /// </summary>
    public class Manufacturer : IdentityBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the manufacturer name.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Manufacturer with a default ID.
        /// </summary>
        public Manufacturer()
        {
        }

        /// <summary>
        /// Initializes a new Manufacturer with a specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        public Manufacturer(object id)
            : base(id)
        {
        }

        #endregion

    }
}
