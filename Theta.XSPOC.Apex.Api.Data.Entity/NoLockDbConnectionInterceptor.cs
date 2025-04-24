using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// A database connection interceptor which ensures querying of 
    /// the database with NOLOCK (dirty read; uncommitted read).
    /// </summary>
    public class NoLockDbConnectionInterceptor : DbConnectionInterceptor
    {

        /// <summary>
        /// This method is overriden to set the database connection's transaction isolation level 
        /// to 'read uncommitted' when a connection to the database is opened.
        /// </summary>
        /// <param name="connection">The established database connection.</param>
        /// <param name="eventData">The connection end event data.</param>
        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
                command.ExecuteNonQuery();
            }

            base.ConnectionOpened(connection, eventData);
        }

    }
}
