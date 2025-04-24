namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The model for the card coordinate data.
    /// </summary>
    public class CardCoordinateModel
    {

        #region Private Fields

        private int? _area;
        private int? _areaLimit;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        public int Area
        {
            get => _area ?? 0;
            set => _area = value;
        }

        /// <summary>
        /// Gets or sets the area limit.
        /// </summary>
        public int AreaLimit
        {
            get => _areaLimit ?? 0;
            set => _areaLimit = value;
        }

        /// <summary>
        /// Gets or sets the binary down hole card.
        /// </summary>
        public byte[] DownHoleCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the fillage.
        /// </summary>
        public float? Fillage { get; set; }

        /// <summary>
        /// Gets or sets the fill base percent.
        /// </summary>
        public int? FillBasePercent { get; set; }

        /// <summary>
        /// Gets or sets the hi load limit.
        /// </summary>
        public int? HiLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the load limit.
        /// </summary>
        public int? LoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the load limit 2.
        /// </summary>
        public short? LoadLimit2 { get; set; }

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
        /// Gets or sets the binary permissible load down.
        /// </summary>
        public byte[] PermissibleLoadDownBinary { get; set; }

        /// <summary>
        /// Gets or sets the binary permissible load up.
        /// </summary>
        public byte[] PermissibleLoadUpBinary { get; set; }

        /// <summary>
        /// Gets or sets the POCDownholeCard.
        /// </summary>
        public string POCDownholeCard { get; set; }

        /// <summary>
        /// Gets or sets the binary poc down hole card.
        /// </summary>
        public byte[] PocDownHoleCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the position limit.
        /// </summary>
        public int? PositionLimit { get; set; }

        /// <summary>
        /// Gets or sets the position limit 2.
        /// </summary>
        public short? PositionLimit2 { get; set; }

        /// <summary>
        /// Gets or sets the predicted card.
        /// </summary>
        public string PredictedCard { get; set; }

        /// <summary>
        /// Gets or sets the binary predicted card.
        /// </summary>
        public byte[] PredictedCardBinary { get; set; }

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
        /// Gets or sets the binary surface card.
        /// </summary>
        public byte[] SurfaceCardBinary { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        public short PocType { get; set; }

        #endregion

    }
}
