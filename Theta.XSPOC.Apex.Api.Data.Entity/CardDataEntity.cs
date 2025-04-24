using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The card data table.
    /// </summary>
    [Table("tblCardData")]
    public class CardDataEntity
    {

        #region Private Fields

        private int? _area;
        private int? _areaLimit;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the analysis date.
        /// </summary>
        [Column("AnalysisDate")]
        public DateTime? AnalysisDate { get; set; }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        [Column("Area")]
        public int Area
        {
            get => _area ?? 0;
            set => _area = value;
        }

        /// <summary>
        /// Gets or sets the area limit.
        /// </summary>
        [Column("AreaLimit")]
        public int AreaLimit
        {
            get => _areaLimit ?? 0;
            set => _areaLimit = value;
        }

        /// <summary>
        /// Gets or sets the card area.
        /// </summary>
        [Column("CardArea")]
        public int? CardArea { get; set; }

        /// <summary>
        /// Gets or sets the card type.
        /// </summary>
        [Column("CardType")]
        [MaxLength(1)]
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets the cause id.
        /// </summary>
        [Column("CauseID")]
        public int? CauseId { get; set; }

        /// <summary>
        /// Gets or sets the corrected card.
        /// </summary>
        [Column("CorrectedCard", TypeName = "text")]
        public string CorrectedCard { get; set; }

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        [Column("Date")]
        public DateTime CardDate { get; set; }

        /// <summary>
        /// Gets or sets the down hole card.
        /// </summary>
        [Column("DownholeCard", TypeName = "text")]
        public string DownHoleCard { get; set; }

        /// <summary>
        /// Gets or sets the binary down hole card.
        /// </summary>
        [Column("DownholeCardB")]
        public byte[] DownHoleCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the binary electrogram card.
        /// </summary>
        [Column("ElectrogramCardB")]
        public byte[] ElectrogramCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the fillage.
        /// </summary>
        [Column("Fillage")]
        public float? Fillage { get; set; }

        /// <summary>
        /// Gets or sets the fill base percent.
        /// </summary>
        [Column("FillBasePct")]
        public int? FillBasePercent { get; set; }

        /// <summary>
        /// Gets or sets the hi load limit.
        /// </summary>
        [Column("HiLoadLimit")]
        public int? HiLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the load limit.
        /// </summary>
        [Column("LoadLimit")]
        public int? LoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the load limit 2.
        /// </summary>
        [Column("LoadLimit2")]
        public short? LoadLimit2 { get; set; }

        /// <summary>
        /// Gets or sets the load span limit.
        /// </summary>
        [Column("LoadSpanLimit")]
        public int? LoadSpanLimit { get; set; }

        /// <summary>
        /// Gets or sets the low load limit.
        /// </summary>
        [Column("LoLoadLimit")]
        public int? LowLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the malfunction load limit.
        /// </summary>
        [Column("MalLoadLimit")]
        public int? MalfunctionLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the malfunction position limit.
        /// </summary>
        [Column("MalPositionLimit")]
        public int? MalfunctionPositionLimit { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [Required]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the binary permissible load down.
        /// </summary>
        [Column("PermissibleLoadDownB")]
        public byte[] PermissibleLoadDownBinary { get; set; }

        /// <summary>
        /// Gets or sets the binary permissible load up.
        /// </summary>
        [Column("PermissibleLoadUpB")]
        public byte[] PermissibleLoadUpBinary { get; set; }

        /// <summary>
        /// Gets or sets the poc DH card.
        /// </summary>
        [Column("PocDHCard", TypeName = "text")]
        public string PocDHCard { get; set; }

        /// <summary>
        /// Gets or sets the POCDownholeCard.
        /// </summary>
        [Column("POCDownholeCard", TypeName = "text")]
        public string POCDownholeCard { get; set; }

        /// <summary>
        /// Gets or sets the binary poc down hole card.
        /// </summary>
        [Column("POCDownholeCardB")]
        public byte[] PocDownHoleCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the position limit.
        /// </summary>
        [Column("PositionLimit")]
        public int? PositionLimit { get; set; }

        /// <summary>
        /// Gets or sets the position limit 2.
        /// </summary>
        [Column("PositionLimit2")]
        public short? PositionLimit2 { get; set; }

        /// <summary>
        /// Gets or sets the predicted card.
        /// </summary>
        [Column("PredictedCard", TypeName = "text")]
        public string PredictedCard { get; set; }

        /// <summary>
        /// Gets or sets the binary predicted card.
        /// </summary>
        [Column("PredictedCardB")]
        public byte[] PredictedCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the process card. Value of 1 means to process.
        /// </summary>
        [Column("ProcessCard")]
        public int? ProcessCard { get; set; }

        /// <summary>
        /// Gets or sets the runtime.
        /// </summary>
        [Column("Runtime")]
        public float? Runtime { get; set; }

        /// <summary>
        /// Gets or sets the saved.
        /// </summary>
        [Column("Saved")]
        public bool Saved { get; set; }

        /// <summary>
        /// Gets or sets the secondary pump fillage.
        /// </summary>
        [Column("SecondaryPumpFillage")]
        public float? SecondaryPumpFillage { get; set; }

        /// <summary>
        /// Gets or sets the strokes per minute.
        /// </summary>
        [Column("SPM")]
        public float? StrokesPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the stroke length.
        /// </summary>
        [Column("StrokeLength")]
        public int? StrokeLength { get; set; }

        /// <summary>
        /// Gets or sets the surface card.
        /// </summary>
        [Column("SurfaceCard", TypeName = "text")]
        public string SurfaceCard { get; set; }

        /// <summary>
        /// Gets or sets the binary surface card.
        /// </summary>
        [Column("SurfaceCardB")]
        public byte[] SurfaceCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the torque plot current.
        /// </summary>
        [Column("TorquePlotCurrent", TypeName = "text")]
        public string TorquePlotCurrent { get; set; }

        /// <summary>
        /// Gets or sets the binary torque plot current.
        /// </summary>
        [Column("TorquePlotCurrentB")]
        public byte[] TorquePlotCurrentBinary { get; set; }

        /// <summary>
        /// Gets or sets the torque plot minimum energy.
        /// </summary>
        [Column("TorquePlotMinEnergy", TypeName = "text")]
        public string TorquePlotMinimumEnergy { get; set; }

        /// <summary>
        /// Gets or sets the binary torque plot minimum energy.
        /// </summary>
        [Column("TorquePlotMinEnergyB")]
        public byte[] TorquePlotMinimumEnergyBinary { get; set; }

        /// <summary>
        /// Gets or sets the torque plot minimum torque.
        /// </summary>
        [Column("TorquePlotMinTorque", TypeName = "text")]
        public string TorquePlotMinimumTorque { get; set; }

        /// <summary>
        /// Gets or sets the binary torque plot minimum torque.
        /// </summary>
        [Column("TorquePlotMinTorqueB")]
        public byte[] TorquePlotMinimumTorqueBinary { get; set; }

        #endregion

    }
}
