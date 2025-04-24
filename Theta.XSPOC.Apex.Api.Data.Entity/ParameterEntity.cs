using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System TblParameter table.
    /// </summary>
    [Table("tblParameters")]
    public class ParameterEntity
    {

        /// <summary>
        /// Get and set the Poc type.
        /// </summary>
        public int Poctype { get; set; }

        /// <summary>
        /// Get and set the Address.
        /// </summary>
        [Column("Address")]
        public int Address { get; set; }

        /// <summary>
        /// Get and set the Description.
        /// </summary>
        [Column("Description")]
        [MaxLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Get and set the DataType.
        /// </summary>
        [Column("DataType")]
        public short? DataType { get; set; }

        /// <summary>
        /// Get and set the Access.
        /// </summary>
        [Column("Access", TypeName = "nvarchar")]
        [MaxLength(10)]
        public string Access { get; set; }

        /// <summary>
        /// Get and set the Scale Factor.
        /// </summary>
        [Column("ScaleFactor")]
        public float? ScaleFactor { get; set; }

        /// <summary>
        /// Get and set the Offset.
        /// </summary>
        [Column("Offset")]
        public float? Offset { get; set; }

        /// <summary>
        /// Get and set the Setpoint.
        /// </summary>
        [Column("Setpoint")]
        public bool Setpoint { get; set; }

        /// <summary>
        /// Get and set the Status Scan.
        /// </summary>
        [Column("StatusScan")]
        public bool StatusScan { get; set; }

        /// <summary>
        /// Get and set the Data Collection.
        /// </summary>
        [Column("DataCollection")]
        public bool DataCollection { get; set; }

        /// <summary>
        /// Get and set the Decimals.
        /// </summary>
        public int Decimals { get; set; }

        /// <summary>
        /// Get and set the Collection Mode.
        /// </summary>
        [Column("CollectionMode")]
        public int CollectionMode { get; set; }

        /// <summary>
        /// Get and set the SetpointGroup.
        /// </summary>
        [Column("SetpointGroup")]
        public int? SetpointGroup { get; set; }

        /// <summary>
        /// Get and set the Group Status View.
        /// </summary>
        [Column("GroupStatusView")]
        public int? GroupStatusView { get; set; }

        /// <summary>
        /// Get and set the Tag.
        /// </summary>
        [Column("Tag")]
        [MaxLength(50)]
        public string Tag { get; set; }

        /// <summary>
        /// Get and set the StateId.
        /// </summary>
        [Column("StateID")]
        public int? StateId { get; set; }

        /// <summary>
        /// Get and set the Locked.
        /// </summary>
        [Column("Locked")]
        public bool? Locked { get; set; }

        /// <summary>
        /// Get and set the Fast Scan.
        /// </summary>
        [Column("FastScan")]
        public bool? FastScan { get; set; }

        /// <summary>
        /// Get and set the Phrase Id.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

        /// <summary>
        /// Get and set the Unit Type.
        /// </summary>
        [Column("UnitType")]
        public int UnitType { get; set; }

        /// <summary>
        /// Get and set the Destination Type.
        /// </summary>
        [Column("DestinationType")]
        public int? DestinationType { get; set; }

        /// <summary>
        /// Get and set the Param Standard Type.
        /// </summary>
        [Column("ParamStandardType")]
        public int? ParamStandardType { get; set; }

        /// <summary>
        /// Get and set the Archive Function.
        /// </summary>
        [Column("ArchiveFunction")]
        public int ArchiveFunction { get; set; }

        /// <summary>
        /// Get and set the Earliest Supported Version.
        /// </summary>
        [Column("EarliestSupportedVersion")]
        public float EarliestSupportedVersion { get; set; }

    }
}
