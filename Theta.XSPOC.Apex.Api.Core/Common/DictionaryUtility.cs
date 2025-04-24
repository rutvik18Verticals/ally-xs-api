using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Supports various operations on dictionaries.
    /// </summary>
    public static class DictionaryUtility
    {

        /// <summary>
        /// Gets the value associated with a specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to get the value from.</param>
        /// <param name="key">The key to get the value for.</param>
        /// <returns>
        /// The value associated with the specified key if found; otherwise, the default value of TValue.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> is null
        /// OR
        /// <paramref name="key"/> is null.
        /// </exception>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            dictionary.TryGetValue(key, out var value);

            return value;
        }

    }
}
