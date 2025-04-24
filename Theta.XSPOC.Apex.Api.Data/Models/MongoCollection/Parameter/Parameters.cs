namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter
{
    /// <summary>
    /// Represents a collection of parameters.
    /// </summary>
    public class Parameters : ParametersBase
    {

        /// <summary>
        /// Gets or sets the additional details.
        /// </summary>
        public ParameterDocumentBase ParameterDocument { get; set; }

    }
}