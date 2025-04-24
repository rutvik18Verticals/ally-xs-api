using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Represents an interface for a String Id Store.
    /// </summary>
    public interface IStringIdStore
    {

        /// <summary>
        /// Gets the string id.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns>The list of String Id.</returns>
        IList<StringIdModel> GetStringId(string correlationId);

    }
}
