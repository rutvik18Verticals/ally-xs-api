using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the card data model.
    /// </summary>
    public class CardDataModel
    {

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        public int Area { get; set; }

        /// <summary>
        /// Gets or sets the area limit.
        /// </summary>
        public int AreaLimit { get; set; }

        /// <summary>
        /// Gets or sets the card area.
        /// </summary>
        public int? CardArea { get; set; }

        /// <summary>
        /// Gets or sets the card type.
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets the cause id.
        /// </summary>
        public int? CauseId { get; set; }

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        public DateTime CardDate { get; set; }

        /// <summary>
        /// Gets or sets the fillage.
        /// </summary>
        public float? Fillage { get; set; }

        /// <summary>
        /// Gets or sets the fill base percent.
        /// </summary>
        public int? FillBasePercent { get; set; }

        /// <summary>
        /// Gets or sets the load limit.
        /// </summary>
        public int? LoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the load limit 2.
        /// </summary>
        public short? LoadLimit2 { get; set; }

        /// <summary>
        /// Gets or sets the load span limit.
        /// </summary>
        public int? LoadSpanLimit { get; set; }

        /// <summary>
        /// Gets or sets the low load limit.
        /// </summary>
        public int? LowLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the malfunction load limit.
        /// </summary>
        public int? MalfunctionLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the malfunction position limit.
        /// </summary>
        public int? MalfunctionPositionLimit { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the poc DH card.
        /// </summary>
        public string PocDHCard { get; set; }

        /// <summary>
        /// Gets or sets the POCDownholeCard.
        /// </summary>
        public string POCDownholeCard { get; set; }

        /// <summary>
        /// Gets or sets the binary poc down hole card.
        /// </summary>
        public byte[] PocDownHoleCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the runtime.
        /// </summary>
        public float? Runtime { get; set; }

        /// <summary>
        /// Gets or sets the secondary pump fillage.
        /// </summary>
        public float? SecondaryPumpFillage { get; set; }

        /// <summary>
        /// Gets or sets the strokes per minute.
        /// </summary>
        public float? StrokesPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the stroke length.
        /// </summary>
        public int? StrokeLength { get; set; }

        /// <summary>
        /// Gets or sets the surface card.
        /// </summary>
        public string SurfaceCard { get; set; }

    }
}
