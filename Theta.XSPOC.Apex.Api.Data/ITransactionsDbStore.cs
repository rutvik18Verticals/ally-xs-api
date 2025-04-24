using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Manages access to the <seealso cref="TransactionsModel"/> collection.
    /// </summary>
    public interface ITransactionsDbStore
    {

        /// <summary>
        /// Persists the given <param name="transaction"/> in the data store. If it is not present it will be added. If it is
        /// present, it will be updated.
        /// </summary>
        Task<bool> UpdateAsync(TransactionsModel transaction);

    }
}