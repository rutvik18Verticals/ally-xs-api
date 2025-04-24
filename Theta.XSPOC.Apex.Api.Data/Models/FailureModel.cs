using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the failure data model
    /// </summary>
    public class FailureModel
    {

        /// <summary>
        /// The id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The node id.
        /// </summary>
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// The failure date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The identified date.
        /// </summary>
        public DateTime? IdentifiedDate { get; set; }

        /// <summary>
        /// The rig up date.
        /// </summary>
        public DateTime? RigUpDate { get; set; }

        /// <summary>
        /// The rig down date.
        /// </summary>
        public DateTime? RigDownDate { get; set; }

        /// <summary>
        /// The recovery date.
        /// </summary>
        public DateTime? RecoveryDate { get; set; }

        /// <summary>
        /// The component id.
        /// </summary>
        public int? ComponentId { get; set; }

        /// <summary>
        /// The failure component.
        /// </summary>
        public ComponentItemModel Component { get; set; }

        /// <summary>
        /// The sub component id.
        /// </summary>
        public int? SubcomponentId { get; set; }

        /// <summary>
        /// The failure sub component.
        /// </summary>
        public ComponentItemModel SubComponent { get; set; }

        /// <summary>
        /// The mode id.
        /// </summary>
        public int? ModeId { get; set; }

        /// <summary>
        /// The mechanism id.
        /// </summary>
        public int? MechanismId { get; set; }

        /// <summary>
        /// The total cost.
        /// </summary>
        public decimal? TotalCost { get; set; }

        /// <summary>
        /// The requested service id.
        /// </summary>
        public int? RequestedServiceId { get; set; }

        /// <summary>
        /// The pull reason id.
        /// </summary>
        public int? PullReasonId { get; set; }

        /// <summary>
        /// The notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The value specifying the record is locked or not.
        /// </summary>
        public bool Locked { get; set; }

    }
}
