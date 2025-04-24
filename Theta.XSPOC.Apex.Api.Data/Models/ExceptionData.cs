namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Defines the exception data.
    /// </summary>
    public record ExceptionData
    {

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets priority.
        /// </summary>
        public int Priority { get; set; }

    }
}