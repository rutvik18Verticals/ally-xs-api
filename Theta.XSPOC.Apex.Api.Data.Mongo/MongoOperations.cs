using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// The implementation for the operations on the MongoDB.
    /// </summary>
    public class MongoOperations : IMongoOperations
    {

        #region Private Fields

        private readonly IMongoDatabase _mongoDatabase;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor for <seealso cref="MongoOperations"/>.
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="database"/> is null
        /// or
        /// When <paramref name="loggerFactory"/> is null.
        /// </exception>
        public MongoOperations(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
        {
            _mongoDatabase = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IMongoOperations Implementation

        /// <summary>
        /// Get all the documents of <typeparamref name="T"/> from the <paramref name="collectionName"/> collection
        /// in the MongoDB instance.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="correlationId">The correlation id.</param>
        public IList<T> FindAll<T>(string collectionName, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(MongoOperations)}" +
                $" {nameof(FindAll)}", correlationId);

            var mongoCollection = _mongoDatabase.GetCollection<T>(collectionName);

            var filter = Builders<T>.Filter.Empty;
            var result = mongoCollection.Find<T>(filter);

            return result.ToList();
        }

        /// <summary>
        /// Get one documents of <typeparamref name="T"/> from the <paramref name="collectionName"/> collection
        /// in the MongoDB instance.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="filterDefinition"></param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The documents of the collection.</returns>
        public IList<T> Find<T>(string collectionName, FilterDefinition<T> filterDefinition, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(MongoOperations)}" +
                $" {nameof(FindAll)}", correlationId);
            try
            {
                var mongoCollection = _mongoDatabase.GetCollection<T>(collectionName, null);

                var filter = filterDefinition;
                var result = mongoCollection.Find<T>(filter);

                return result.ToList();

            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in  {nameof(MongoOperations)} {nameof(FindAll)}" +
                $" {ex.Message}", correlationId);
            }

            return null;
        }

        /// <summary>
        /// Get one documents of <typeparamref name="T"/> from the <paramref name="collectionName"/> collection
        /// in the MongoDB instance.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <typeparam name="K">The key type.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="keyName">The key name to be used in the filter.</param>
        /// <param name="key">The key to use to find the document.</param>
        /// <param name="correlationId">The correlation id.</param>
        public T Find<T, K>(string collectionName, string keyName, K key, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(MongoOperations)}" +
                $" {nameof(FindAll)}", correlationId);
            try
            {
                var mongoCollection = _mongoDatabase.GetCollection<T>(collectionName);

                var filter = Builders<T>.Filter.Eq(keyName, key);
                var result = mongoCollection.Find<T>(filter);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in  {nameof(MongoOperations)} {nameof(Find)}" +
                $" {ex.Message}", correlationId);
            }

            return default;
        }

        /// <summary>
        /// Get one documents of <typeparamref name="T"/> from the <paramref name="collectionName"/> collection
        /// in the MongoDB instance.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <typeparam name="K">The key type.</typeparam>
        /// <param name="collectionName">The collection name.</param>
        /// <param name="keyName">The key name to be used in the filter.</param>
        /// <param name="key">The key to use to find the document.</param>
        /// <param name="correlationId">The correlation id.</param>
        public IList<T> FindMany<T, K>(string collectionName, string keyName, K key, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(MongoOperations)}" +
                $" {nameof(FindAll)}", correlationId);
            try
            {
                var mongoCollection = _mongoDatabase.GetCollection<T>(collectionName);
                var filter = Builders<T>.Filter.Eq(keyName, key);
                var result = mongoCollection.Find<T>(filter);

                return result.ToList();
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in  {nameof(MongoOperations)} {nameof(FindMany)}" +
                $" {ex.Message}", correlationId);
            }

            return null;
        }

        #endregion

    }
}
