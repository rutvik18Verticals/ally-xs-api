using System;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Mappers
{
    /// <summary>
    /// Maps <seealso cref="NotificationsTypesModel"/> to <seealso cref="NotificationTypesData"/>. 
    /// </summary>
    public class NotificationTypesDataMapper
    {

        #region Public Methods

        /// <summary>
        /// Maps the <paramref name="entity"/> to a <seealso cref="NotificationsTypesModel"/> domain object.
        /// </summary>
        /// <param name="entity">The entity to map from.</param>
        /// <returns>A <seealso cref="NotificationTypesData"/> representing the provided <paramref name="entity"/> 
        /// in the domain.</returns>
        public static NotificationTypesData MapToDomainObject(NotificationsTypesModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = new NotificationTypesData()
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
            };

            return result;
        }

        #endregion

    }
}
