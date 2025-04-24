using Microsoft.EntityFrameworkCore;
using System;
using Theta.XSPOC.Apex.Api.Data.Entity.Alarms;

namespace Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup
{
    /// <summary>
    /// Contains helper methods to set up a <seealso cref="CameraAlarmEntity"/> entity
    /// </summary>
    public class CameraAlarm
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

            modelBuilder.Entity<CameraAlarmEntity>()
                .HasKey(e => new
                {
                    e.CameraId,
                    e.AlarmType,
                }).HasName("UX_tblCameraAlarms_AlarmType");

            modelBuilder.Entity<CameraAlarmEntity>()
                .HasKey(e => new
                {
                    e.Id,
                })
                .IsClustered(true)
                .HasName("PK_tblCameraAlarms");

        }

    }
}
