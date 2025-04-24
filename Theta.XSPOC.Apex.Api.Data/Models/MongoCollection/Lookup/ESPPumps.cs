namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the esp pumps  MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class ESPPumps : LookupBase
    {

        /// <summary>
        /// Gets or sets the Id of the ESP pump.
        /// </summary>
        public int ESPPumpId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the manufacturer.
        /// </summary>
        public int? ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the pump.
        /// </summary>
        public string Pump { get; set; }

        /// <summary>
        /// Gets or sets the minimum casing size.
        /// </summary>
        public float? MinCasingSize { get; set; }

        /// <summary>
        /// Gets or sets the housing pressure limit.
        /// </summary>
        public int? HousingPressureLimit { get; set; }

        /// <summary>
        /// Gets or sets the minimum BPD (Barrels Per Day).
        /// </summary>
        public int MinBPD { get; set; }

        /// <summary>
        /// Gets or sets the maximum BPD (Barrels Per Day).
        /// </summary>
        public int MaxBPD { get; set; }

        /// <summary>
        /// Gets or sets the BEP BPD (Best Efficiency Point Barrels Per Day).
        /// </summary>
        public int? BEPBPD { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether coefficients are used.
        /// </summary>
        public bool UseCoefficients { get; set; }

        /// <summary>
        /// Gets or sets the head intercept coefficient.
        /// </summary>
        public float? HeadIntercept { get; set; }

        /// <summary>
        /// Gets or sets the HP (Horsepower) intercept coefficient.
        /// </summary>
        public float? HPIntercept { get; set; }

        /// <summary>
        /// Gets or sets the efficiency intercept coefficient.
        /// </summary>
        public float? EfficiencyIntercept { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 1st term.
        /// </summary>
        public float? Head1Coef { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 2nd term.
        /// </summary>
        public float? Head2Coef { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 3rd term.
        /// </summary>
        public float? Head3Coef { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 4th term.
        /// </summary>
        public float? Head4Coef { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 5th term.
        /// </summary>
        public float? Head5Coef { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 6th term.
        /// </summary>
        public float? Head6Coef { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 7th term.
        /// </summary>
        public float? Head7Coef { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 8th term.
        /// </summary>
        public float? Head8Coef { get; set; }

        /// <summary>
        /// Gets or sets the head coefficient for the 9th term.
        /// </summary>
        public float? Head9Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 1st term.
        /// </summary>
        public float? HP1Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 2nd term.
        /// </summary>
        public float? HP2Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 3rd term.
        /// </summary>
        public float? HP3Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 4th term.
        /// </summary>
        public float? HP4Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 5th term.
        /// </summary>
        public float? HP5Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 6th term.
        /// </summary>
        public float? HP6Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 7th term.
        /// </summary>
        public float? HP7Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 8th term.
        /// </summary>
        public float? HP8Coef { get; set; }

        /// <summary>
        /// Gets or sets the HP coefficient for the 9th term.
        /// </summary>
        public float? Hp9Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 1st term.
        /// </summary>
        public float? Eff1Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 2nd term.
        /// </summary>
        public float? Eff2Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 3rd term.
        /// </summary>
        public float? Eff3Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 4th term.
        /// </summary>
        public float? Eff4Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 5th term.
        /// </summary>
        public float? Eff5Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 6th term.
        /// </summary>
        public float? Eff6Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 7th term.
        /// </summary>
        public float? Eff7Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 8th term.
        /// </summary>
        public float? Eff8Coef { get; set; }

        /// <summary>
        /// Gets or sets the efficiency coefficient for the 9th term.
        /// </summary>
        public float? Eff9Coef { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the pump is locked.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the series information.
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the pump model.
        /// </summary>
        public string PumpModel { get; set; }

    }
}
