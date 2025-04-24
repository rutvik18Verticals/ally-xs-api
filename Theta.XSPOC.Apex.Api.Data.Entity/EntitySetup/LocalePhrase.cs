using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="LocalePhraseEntity"/> entity.
    /// </summary>
    public static class LocalePhrase
    {

        /// <summary>
        /// This is the setup method to help build keys, indexes, and default values.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="modelBuilder"/> is null.
        /// </exception>
        public static void Setup(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<LocalePhraseEntity>()
                .HasKey(e => e.PhraseId)
                .IsClustered(false);

            modelBuilder.Entity<LocalePhraseEntity>()
                .ToTable("tblLocalePhrases");

            modelBuilder.Entity<LocalePhraseEntity>()
                .HasIndex(e => e.PhraseId, "IX_tblLocalePhrases")
                .IsUnique()
                .IsClustered();

            modelBuilder.Entity<LocalePhraseEntity>()
                .Property(e => e.PhraseId)
                .ValueGeneratedNever()
                .HasColumnName("PhraseID");

            modelBuilder.Entity<LocalePhraseEntity>()
                .Property(e => e.German)
                .HasMaxLength(1024)
                .HasColumnName("1031");

            modelBuilder.Entity<LocalePhraseEntity>()
                .Property(e => e.English)
                .HasMaxLength(1024)
                .HasColumnName("1033");

            modelBuilder.Entity<LocalePhraseEntity>()
                .Property(e => e.Spanish)
                .HasMaxLength(1024)
                .HasColumnName("1034");

            modelBuilder.Entity<LocalePhraseEntity>()
                .Property(e => e.French)
                .HasMaxLength(1024)
                .HasColumnName("1036");

            modelBuilder.Entity<LocalePhraseEntity>()
                .Property(e => e.Russian)
                .HasMaxLength(1024)
                .HasColumnName("1049");

            modelBuilder.Entity<LocalePhraseEntity>()
                .Property(e => e.Chinese)
                .HasMaxLength(1024)
                .HasColumnName("2052");
        }

    }
}
