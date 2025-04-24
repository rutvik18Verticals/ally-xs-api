using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="MeterColumnEntity"/> entity.
    /// </summary>
    public class MeterColumn
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

            modelBuilder.Entity<MeterColumnEntity>()
                .Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<MeterColumnEntity>()
                .HasKey(e => new
                {
                    e.Id,
                    e.MeterTypeId,
                    e.Name,
                }).HasName("PK_tblMeterColumns");

        }
    }
}
