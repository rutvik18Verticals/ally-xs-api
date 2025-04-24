using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ESPWellMotorLeadEntity"/> entity
    /// </summary>
    public class ESPWellMotorLeads
    {

        /// <summary>
        /// This is the setup method to help build keys, indexes, and default values.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<ESPWellMotorLeadEntity>()
                .HasKey(k => new
                {
                    k.NodeId,
                    k.ESPMotorLeadId,
                    k.OrderNumber
                })
                .HasName("tblESPWellMotorLeads")
                .IsClustered(true);
        }

    }
}
