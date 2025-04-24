namespace Theta.XSPOC.Apex.Api.Core.Models
{
    /// <summary>
    /// The status helper model.
    /// </summary>
    public class StatusHelperModel
    {

        /// <summary>
        /// The id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The message.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
