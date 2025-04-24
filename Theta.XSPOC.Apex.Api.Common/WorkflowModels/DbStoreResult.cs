namespace Theta.XSPOC.Apex.Api.Common.WorkflowModels
{
    /// <summary>
    /// Represents the result of attempting a database operation. Meant to inform the encapsulating layers about
    /// the actions that should be taken.
    /// </summary>
    public class DbStoreResult
    {

        /// <summary>
        /// Gets or sets an optional message, meant to describe unsuccessful results in particular.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the kind of error that occurred. Classified by <seealso cref="ErrorType"/>.
        /// </summary>
        public ErrorType KindOfError { get; set; } = ErrorType.None;

    }
}
