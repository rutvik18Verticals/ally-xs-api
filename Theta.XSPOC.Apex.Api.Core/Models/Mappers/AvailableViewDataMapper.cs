using System;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Mappers
{
    /// <summary>
    /// Maps AvailableViewModelModel to AvailableViewResponseValues object.
    /// </summary>
    public class AvailableViewDataMapper
    {

        #region Public Methods

        /// <summary>
        /// Maps the <paramref name="model"/> to a <seealso cref="AvailableViewModel"/> domain object.
        /// </summary>
        /// <param name="model">A <seealso cref="AvailableViewModel"/> domain object.</param>
        /// <returns>A <seealso cref="AvailableViewData"/> representing the provided <paramref name="model"/> 
        /// in the domain.</returns>
        public static AvailableViewData MapToDomainObject(AvailableViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var result = new AvailableViewData()
            {
                ViewId = model.ViewId,
                ViewName = model.ViewName,
                IsSelectedView = model.IsSelectedView,
            };

            return result;
        }

        #endregion

    }
}
