using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ESPWellPump"/> entity
    /// </summary>
    public class ESPWellPump
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

            modelBuilder.Entity<ESPWellPumpEntity>()
                .HasKey(e => new
                {
                    e.ESPWellId,
                    e.ESPPumpId,
                    e.OrderNumber
                }).HasName("PK_tblESPWellPumps");

            modelBuilder.Entity<ESPWellPumpEntity>()
                .Property(e => e.ESPWellId)
                .IsRequired(true)
                .HasColumnType("ESPWellID");

            modelBuilder.Entity<ESPWellPumpEntity>()
                .Property(e => e.ESPPumpId)
                .IsRequired(true)
                .HasColumnName("ESPPumpID");

            modelBuilder.Entity<ESPWellPumpEntity>()
                .Property(e => e.OrderNumber)
                .IsRequired(true)
                .HasColumnName("OrderNumber");
        }

    }
}
