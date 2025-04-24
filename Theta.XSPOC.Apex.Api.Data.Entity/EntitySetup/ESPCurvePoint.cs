using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="ESPCurvePointEntity"/> entity
    /// </summary>
    public class ESPCurvePoint
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

            modelBuilder.Entity<ESPCurvePointEntity>()
                .HasKey(e => new
                {
                    e.ESPPumpID,
                    e.FlowRate
                }).HasName("PK_tblESPCurvePoints_1");

            modelBuilder.Entity<ESPCurvePointEntity>()
                .Property(e => e.ESPPumpID)
                .IsRequired(true)
                .HasColumnName("ESPPumpID");

            modelBuilder.Entity<ESPCurvePointEntity>()
                .Property(e => e.FlowRate)
                .IsRequired(true)
                .HasColumnName("FlowRate");
        }

    }
}
