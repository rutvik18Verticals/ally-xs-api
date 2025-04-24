namespace Theta.XSPOC.Apex.Api.Common.WorkflowModels
{
    /// <summary>
    /// Used with the <seealso cref="DbStoreResult"/> to describe whether the result is an error, and if so, what
    /// kind of error.
    /// </summary>
    public enum ErrorType
    {

        /// <summary>
        /// The operation succeeded.
        /// </summary>
        None = 0,

        /// <summary>
        /// The operation failed, but the failure is likely recoverable so the operation can be retried.
        /// </summary>
        LikelyRecoverable = 1,

        /// <summary>
        /// The operation failed in a way that is known to not be recoverable.
        /// </summary>
        NotRecoverable = 2,

    }
}
