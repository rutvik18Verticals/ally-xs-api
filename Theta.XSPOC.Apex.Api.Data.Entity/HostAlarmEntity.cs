using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Host Alarm table.
    /// </summary>
    [Table("tblHostAlarms")]
    public class HostAlarmEntity
    {

        /// <summary>
        /// Gets or sets NodeId.
        /// </summary>
        [Required]
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets Address.
        /// </summary>
        public int? Address { get; set; }

        /// <summary>
        /// Gets or sets LoLimit.
        /// </summary>
        public float? LoLimit { get; set; }

        /// <summary>
        /// Gets or sets LoLoLimit.
        /// </summary>
        public float? LoLoLimit { get; set; }

        /// <summary>
        /// Gets or sets HiLimit.
        /// </summary>
        public float? HiLimit { get; set; }

        /// <summary>
        /// Gets or sets HiHiLimit.
        /// </summary>
        public float? HiHiLimit { get; set; }

        /// <summary>
        /// Gets or sets DiscreteNormalState
        /// </summary>
        public int? DiscreteNormalState { get; set; }

        /// <summary>
        /// Gets or sets AlarmType.
        /// </summary>
        public int AlarmType { get; set; }

        /// <summary>
        /// Gets or sets AlarmState.
        /// </summary>
        public int? AlarmState { get; set; }

        /// <summary>
        /// Gets or sets Priority.
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or sets Average30D.
        /// </summary>
        public float? Average30D { get; set; }

        /// <summary>
        /// Gets or sets StandardDev.
        /// </summary>
        public float? StandardDev { get; set; }

        /// <summary>
        /// Gets or sets PagerId.
        /// </summary>
        [Column("PagerID")]
        [MaxLength(50)]
        [Unicode(false)]
        public string PagerId { get; set; }

        /// <summary>
        /// Gets or sets PagerMessage.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string PagerMessage { get; set; }

        /// <summary>
        /// Gets or sets PagingEnabled.
        /// </summary>
        public bool? PagingEnabled { get; set; }

        /// <summary>
        /// Gets or sets NumDays.
        /// </summary>
        public float? NumDays { get; set; }

        /// <summary>
        /// Gets or sets PercentChange.
        /// </summary>
        public int? PercentChange { get; set; }

        /// <summary>
        /// Gets or sets CyclesLimit.
        /// </summary>
        public int? CyclesLimit { get; set; }

        /// <summary>
        /// Gets or sets CyclesValue.
        /// </summary>
        public int? CyclesValue { get; set; }

        /// <summary>
        /// Gets or sets MinToMaxLimit.
        /// </summary>
        public int? MinToMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets IgnoreZeroAddress.
        /// </summary>
        public int? IgnoreZeroAddress { get; set; }

        /// <summary>
        /// Gets or sets AlarmAction.
        /// </summary>
        public int? AlarmAction { get; set; }

        /// <summary>
        /// Gets or sets Enabled.
        /// </summary>
        [Required]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets or sets LastStateChange.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? LastStateChange { get; set; }

        /// <summary>
        /// Gets or sets ValueChange.
        /// </summary>
        public float? ValueChange { get; set; }

        /// <summary>
        /// Gets or sets IgnoreValue.
        /// </summary>
        public float? IgnoreValue { get; set; }

        /// <summary>
        /// Gets or sets XdiagOutputsId.
        /// </summary>
        [Column("XDiagOutputsID")]
        public int? XdiagOutputsId { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets PushEnabled.
        /// </summary>
        public bool PushEnabled { get; set; }

        /// <summary>
        /// Gets or sets ExactValue.
        /// </summary>
        public float? ExactValue { get; set; }

        /// <summary>
        /// Gets or sets PersistentNotificationMinutes.
        /// </summary>
        public int? PersistentNotificationMinutes { get; set; }

        /// <summary>
        /// Gets or sets DateTimeNotificationIssued.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? DateTimeNotificationIssued { get; set; }

    }
}
