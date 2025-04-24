using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents local phrases.
    /// </summary>
    public interface ILocalePhrases
    {

        /// <summary>
        /// Get the phrase by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="string"/>The phrase.</returns>
        string Get(int id, string correlationId);

        /// <summary>
        /// Get local phrases data by provided ids.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="ids">The ids.</param>
        /// <returns>A dictionary of int to string containing phrase ids mapped to their phrase strings.</returns>
        /// <exception cref="ArgumentNullException">Handling ids null exception.</exception>
        IDictionary<int, string> GetAll(string correlationId, params int[] ids);

    }
}
