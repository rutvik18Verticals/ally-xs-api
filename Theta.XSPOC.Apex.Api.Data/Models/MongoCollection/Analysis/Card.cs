using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// A Card is a collection of data points that are used to analyze the performance of a pump.
    /// </summary>
    public class Card : AssetDocumentBase
    {

        /// <summary>
        /// Gets or sets the date the card was generated.
        ///</summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the card type.
        ///</summary>
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets the surface card.
        ///</summary>
        public string SurfaceCard { get; set; }

        /// <summary>
        /// Gets or sets the Strokes Per Minute (SPM).
        ///</summary>
        public double? StrokesPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the stroke length.
        ///</summary>
        public int? StrokeLength { get; set; }

        /// <summary>
        /// Runtime
        ///</summary>
        public double? Runtime { get; set; }

        /// <summary>
        /// Gets or sets the load limit.
        ///</summary>
        public int? LoadLimit { get; set; }

        /// <summary>
        /// PositionLimit
        ///</summary>
        public int? PositionLimit { get; set; }

        /// <summary>
        /// Gets or sets the downhole card as a string.
        ///</summary>
        public string DownholeCard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the card has been saved.
        ///</summary>
        public bool Saved { get; set; }

        /// <summary>
        /// Gets or sets the malfunction load limit.
        ///</summary>
        public int? MalfuncationLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the malfunction position limit.
        ///</summary>
        public int? MalfuncationPositionLimit { get; set; }

        /// <summary>
        /// Gets or sets the predicted card as a string.
        ///</summary>
        public string PredictedCard { get; set; }

        /// <summary>
        /// Gets or sets the area.
        ///</summary>
        public int Area { get; set; }

        /// <summary>
        /// Gets or sets the area limit.
        ///</summary>
        public int AreaLimit { get; set; }

        /// <summary>
        /// Gets or sets the POC downhole card as a string.
        ///</summary>
        public string PocDHCard { get; set; }

        /// <summary>
        /// Gets or sets the corrected card as a string.
        ///</summary>
        public string CorrectedCard { get; set; }

        /// <summary>
        /// Gets or sets the load limit 2.
        ///</summary>
        public int? LoadLimit2 { get; set; }

        /// <summary>
        /// Gets or sets the position limit 2.
        /// </summary>
        public int? PositionLimit2 { get; set; }

        /// <summary>
        /// Gets or sets the load span limit.
        /// </summary>
        public int? LoadSpanLimit { get; set; }

        /// <summary>
        /// Gets or sets the high load limit.
        /// </summary>
        public int? HiLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the low load limit.
        /// </summary>
        public int? LoLoadLimit { get; set; }

        /// <summary>
        /// Gets or sets the card area.
        /// </summary>
        public int? CardArea { get; set; }

        /// <summary>
        /// Gets or sets if the cards needs to be processed. Value of 1 is true and value of 0 is false.
        /// </summary>
        public int? ProcessCard { get; set; }

        /// <summary>
        /// Gets or sets the fill base percentage.
        /// </summary>
        public int? FillBasePct { get; set; }

        /// <summary>
        /// Gets or sets the torque plot minimum energy.
        /// </summary>
        public string TorquePlotMinEnergy { get; set; }

        /// <summary>
        /// Gets or sets the torque plot minimum torque.
        /// </summary>
        public string TorquePlotMinTorque { get; set; }

        /// <summary>
        /// Gets or sets the torque plot current.
        /// </summary>
        public string TorquePlotCurrent { get; set; }

        /// <summary>
        /// Gets or sets the POC downhole card as a string.
        /// </summary>
        public string POCDownholeCard { get; set; }

        /// <summary>
        /// Gets or sets the surface card.
        /// </summary>
        public IList<CoordinatesData<float>> SurfaceCardPoints { get; set; }

        /// <summary>
        /// Gets or sets the downhole card.
        /// </summary>
        public IList<CoordinatesData<float>> DownholeCardPoints { get; set; }

        /// <summary>
        /// Gets or sets the predicted card.
        /// </summary>
        public IList<CoordinatesData<float>> PredictedCardPoints { get; set; }

        /// <summary>
        /// Gets or sets the torque plot minimum energy.
        /// </summary>
        public IList<CoordinatesData<float>> TorquePlotMinEnergyPoints { get; set; }

        /// <summary>
        /// Gets or sets the torque plot minimum torque.
        /// </summary>
        public IList<CoordinatesData<float>> TorquePlotMinTorquePoints { get; set; }

        /// <summary>
        /// Gets or sets the torque plot current.
        /// </summary>
        public IList<CoordinatesData<float>> TorquePlotCurrentPoints { get; set; }

        /// <summary>
        /// Gets or sets the POC downhole card.
        /// </summary>
        public IList<CoordinatesData<float>> POCDownholeCardPoints { get; set; }

        /// <summary>
        /// Gets or sets the fillage.
        /// </summary>
        public double? Fillage { get; set; }

        /// <summary>
        /// Gets or sets the cause id. Lookup?
        /// </summary>
        public int? CauseID { get; set; }

        /// <summary>
        /// Gets or sets the secondary pump fillage.
        /// </summary>
        public double? SecondaryPumpFillage { get; set; }

        /// <summary>
        /// Gets or sets Permission Load Up.
        /// </summary>
        public IList<CoordinatesData<float>> PermissibleLoadUpPoints { get; set; }

        /// <summary>
        /// Gets or sets Permission Load Down.
        /// </summary>
        public IList<CoordinatesData<float>> PermissibleLoadDownPoints { get; set; }

        /// <summary>
        /// Gets or sets Electrogram Card.
        /// </summary>
        public IList<CoordinatesData<float>> ElectrogramCardPoints { get; set; }

    }
}
