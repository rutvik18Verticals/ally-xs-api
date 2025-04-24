using MongoDB.Driver;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// The interface that defines operations on the MongoDB.
    /// </summary>
    public interface IMongoOperations
    {

        /// <summary>
        /// Get all the documents of <typeparamref name="T"/> from the <paramref name="collectionName"/> collection
        /// in the MongoDB instance.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="correlationId">The correlation id.</param>
        IList<T> FindAll<T>(string collectionName, string correlationId);

        /// <summary>
        /// Get one documents of <typeparamref name="T"/> from the <paramref name="collectionName"/> collection
        /// in the MongoDB instance and filter definition.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="filterDefinition">The documents of the collection.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        IList<T> Find<T>(string collectionName, FilterDefinition<T> filterDefinition, string correlationId);

        /// <summary>
        /// Get one documents of <typeparamref name="T"/> from the <paramref name="collectionName"/> collection
        /// in the MongoDB instance.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <typeparam name="K">The key type.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="keyName">The key name to be used in the filter.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="key">The key to use to find the document.</param>
        T Find<T, K>(string collectionName, string keyName, K key, string correlationId);

        /// <summary>
        /// Get many documents of <typeparamref name="T"/> from the <paramref name="collectionName"/> collection
        /// in the MongoDB instance.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <typeparam name="K">The key type.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="keyName">The key name to be used in the filter.</param>
        /// <param name="key">The key to use to find the document.</param>
        /// <param name="correlationId">The correlation id.</param>
        IList<T> FindMany<T, K>(string collectionName, string keyName, K key, string correlationId);

    }
}
