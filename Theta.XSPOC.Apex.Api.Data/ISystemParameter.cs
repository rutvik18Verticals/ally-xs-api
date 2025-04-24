namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Represents an interface for retrieving values from the system parameters.
    /// </summary>
    public interface ISystemParameter
    {

        /// <summary>
        /// Retrieves the system parameter value.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The system parameter value.</returns>
        string Get(string parameter, string correlationId);

    }
}
