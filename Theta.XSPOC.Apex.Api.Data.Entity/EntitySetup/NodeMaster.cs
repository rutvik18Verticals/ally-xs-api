using Microsoft.EntityFrameworkCore;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="NodeMasterEntity"/> entity.
    /// </summary>
    public static class NodeMaster
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

            modelBuilder.Entity<NodeMasterEntity>(entity =>
            {
                entity.HasKey(e => e.NodeId)
                    .HasName("aaaaatblNodeMaster_PK")
                    .IsClustered(false);

                entity.HasIndex(e => e.CommSuccess, "NUmComSuccess");

                entity.HasIndex(e => e.NodeId, "Name").IsUnique();

                entity.HasIndex(e => e.CommAttempt, "NumCommAttempts");

                entity.Property(e => e.NodeId)
                    .HasMaxLength(50)
                    .HasColumnName("NodeID");

                entity.Property(e => e.AdhocGroup1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdhocGroup2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdhocGroup3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AllowStartLock).HasDefaultValueSql("((0))");

                entity.Property(e => e.Apipassword).HasColumnName("APIPassword");

                entity.Property(e => e.Apiport).HasColumnName("APIPort");

                entity.Property(e => e.Apiusername)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APIUsername");

                entity.Property(e => e.ApplicationId)
                    .HasDefaultValueSql("((99))")
                    .HasColumnName("ApplicationID");

                entity.Property(e => e.AssetGuid)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("AssetGUID");

                entity.Property(e => e.CommStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment).HasMaxLength(500);

                entity.Property(e => e.Comment2).HasMaxLength(500);

                entity.Property(e => e.Comment3).HasMaxLength(500);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreationDateTime).HasColumnType("datetime");

                entity.Property(e => e.DefaultWindowId).HasColumnName("DefaultWindowID");

                entity.Property(e => e.DisableCode).HasMaxLength(6);

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.FilterId)
                    .HasDefaultValueSql("((0))")
                    .HasColumnName("FilterID");

                entity.Property(e => e.FiterId)
                    .HasDefaultValueSql("((0))")
                    .HasColumnName("FiterID");

                entity.Property(e => e.GroupSdflag).HasColumnName("GroupSDFlag");

                entity.Property(e => e.HighPriAlarm).HasMaxLength(50);

                entity.Property(e => e.HostAlarm)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kwh).HasColumnName("KWH");

                entity.Property(e => e.LastAlarmDate).HasColumnType("datetime");

                entity.Property(e => e.LastAnalCondition)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.LastGoodHistCollection).HasColumnType("datetime");

                entity.Property(e => e.LastGoodScanTime).HasColumnType("datetime");

                entity.Property(e => e.MapId).HasColumnName("MapID");

                entity.Property(e => e.Node)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OtherWellId1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OtherWellID1");

                entity.Property(e => e.ParentNodeId)
                    .HasMaxLength(50)
                    .HasColumnName("ParentNodeID");

                entity.Property(e => e.PocType).HasColumnName("POCType");

                entity.Property(e => e.PortId)
                    .HasDefaultValueSql("((0))")
                    .HasColumnName("PortID");

                entity.Property(e => e.RecSpm).HasColumnName("RecSPM");

                entity.Property(e => e.RunStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RuntimeAccGo).HasColumnName("RuntimeAccGO");

                entity.Property(e => e.RuntimeAccGoyest).HasColumnName("RuntimeAccGOYest");

                entity.Property(e => e.RuntimeAccGoyy).HasColumnName("RuntimeAccGOYY");

                entity.Property(e => e.StartsAccGo).HasColumnName("StartsAccGO");

                entity.Property(e => e.StartsAccGoyest).HasColumnName("StartsAccGOYest");

                entity.Property(e => e.StartsAccGoyy).HasColumnName("StartsAccGOYY");

                entity.Property(e => e.StringId).HasColumnName("StringID");

                entity.Property(e => e.TechNote).HasMaxLength(500);

                entity.Property(e => e.TimeInState).HasDefaultValueSql("((0))");

                entity.Property(e => e.TodayCycles).HasDefaultValueSql("((0))");

                entity.Property(e => e.TodayRuntime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Tzdaylight).HasColumnName("TZDaylight");

                entity.Property(e => e.Tzoffset).HasColumnName("TZOffset");

                entity.Property(e => e.VoiceNodeId)
                    .HasMaxLength(50)
                    .HasColumnName("VoiceNodeID");
            });
        }

    }
}
