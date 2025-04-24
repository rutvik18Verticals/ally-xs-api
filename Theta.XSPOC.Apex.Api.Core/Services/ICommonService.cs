using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for processing common services.
    /// </summary>
    public interface ICommonService
    {

        /// <summary>
        /// Get the Card type name by card type, cause id and poc type.
        /// </summary>
        /// <param name="cardType">The card type.</param>
        /// <param name="causeId">The cause id.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="phrasesCache">The phrases cache.</param>
        /// <param name="causeIdPhraseCache">The cause id phrases cache.</param>
        /// <param name="correlationId"></param>
        /// <returns>The card type name.</returns>
        string GetCardTypeName(string cardType, int? causeId, short pocType, ref IDictionary<int, string> phrasesCache,
            ref IDictionary<int, string> causeIdPhraseCache, string correlationId);

        /// <summary>
        /// Gets the system parameter. If key is in cache, return value from cache; otherwise, query database.
        /// </summary>
        /// <param name="systemParameterKey">The system parameter key.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        string GetSystemParameter(string systemParameterKey, string cacheKey, string correlationId);

        /// <summary>
        /// Gets the system parameter NextGen Significant Digits.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>">
        /// <returns>The system parameter next gen significant digits.</returns>
        int GetSystemParameterNextGenSignificantDigits(string correlationId);

    }
}
