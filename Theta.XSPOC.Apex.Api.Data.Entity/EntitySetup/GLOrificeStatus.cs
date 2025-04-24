using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="GLOrificeStatusEntity"/> entity.
    /// </summary>
    public static class GLOrificeStatus
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

            modelBuilder.Entity<GLOrificeStatusEntity>()
                .HasKey(e => new
                {
                    e.NodeId,
                    e.GLAnalysisResultId
                })
                .HasName("PK_tblGLOrificeStatus_NodeIdAnalysisId");

            modelBuilder.Entity<GLOrificeStatusEntity>()
                .Property(e => e.NodeId)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnName("NodeId");

            modelBuilder.Entity<GLOrificeStatusEntity>()
                .Property(e => e.GLAnalysisResultId)
                .IsRequired(true)
                .HasColumnName("GLAnalysisResultID");

            modelBuilder.Entity<GLOrificeStatusEntity>()
                .Property(e => e.OrificeState)
                .HasDefaultValue(1);
        }

    }
}
