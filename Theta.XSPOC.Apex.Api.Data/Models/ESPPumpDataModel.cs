using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the esp pump data model.
    /// </summary>
    public class ESPPumpDataModel
    {

        /// <summary>
        /// Get or sets the esp pump id.
        /// </summary>
        public int ESPPumpId { get; set; }

        /// <summary>
        /// Get or sets the pump name.
        /// </summary>
        public string Pump { get; set; }

        /// <summary>
        /// Get or sets the minimum casing size.
        /// </summary>
        public float? MinCasingSize { get; set; }

        /// <summary>
        /// Get or sets the housing pressure limit.
        /// </summary>
        public int? HousingPressureLimit { get; set; }

        /// <summary>
        /// Get or sets the minimum bpd.
        /// </summary>
        public int MinBPD { get; set; }

        /// <summary>
        /// Get or sets the maximum bpd.
        /// </summary>
        public int MaxBPD { get; set; }

        /// <summary>
        /// Get or sets the minimum bep bpd.
        /// </summary>
        public int? BEPBPD { get; set; }

        /// <summary>
        /// Get or sets the value that indicates the use of coefficients.
        /// </summary>
        public bool? UseCoefficients { get; set; }

        /// <summary>
        /// Get or sets the head intercept.
        /// </summary>
        public double? HeadIntercept { get; set; }

        /// <summary>
        /// Gets or sets the hp intercept.
        /// </summary>
        public double? HPIntercept { get; set; }

        /// <summary>
        /// Gets or sets the efficiency intercept.
        /// </summary>
        public double? EfficiencyIntercept { get; set; }

        /// <summary>
        /// Gets or sets the head 1 coefficient.
        /// </summary>
        public double? Head1Coef { get; set; }

        /// <summary>
        /// Gets or sets the head 2 coefficient.
        /// </summary>
        public double? Head2Coef { get; set; }

        /// <summary>
        /// Gets or sets the head 3 coefficient.
        /// </summary>
        public double? Head3Coef { get; set; }

        /// <summary>
        /// Gets or sets the head 4 coefficient.
        /// </summary>
        public double? Head4Coef { get; set; }

        /// <summary>
        /// Gets or sets the head 5 coefficient.
        /// </summary>
        public double? Head5Coef { get; set; }

        /// <summary>
        /// Gets or sets the head 6 coefficient.
        /// </summary>
        public double? Head6Coef { get; set; }

        /// <summary>
        /// Gets or sets the head 7 coefficient.
        /// </summary>
        public double? Head7Coef { get; set; }

        /// <summary>
        /// Gets or sets the head 8 coefficient.
        /// </summary>
        public double? Head8Coef { get; set; }

        /// <summary>
        /// Gets or sets the head 9 coefficient.
        /// </summary>
        public double? Head9Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 1 coefficient.
        /// </summary>
        public double? HP1Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 2 coefficient.
        /// </summary>
        public double? HP2Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 3 coefficient.
        /// </summary>
        public double? HP3Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 4 coefficient.
        /// </summary>
        public double? HP4Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 5 coefficient.
        /// </summary>
        public double? HP5Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 6 coefficient.
        /// </summary>
        public double? HP6Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 7 coefficient.
        /// </summary>
        public double? HP7Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 8 coefficient.
        /// </summary>
        public double? HP8Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 9 coefficient.
        /// </summary>
        public double? HP9Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 1 coefficient.
        /// </summary>
        public double? Eff1Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 2 coefficient.
        /// </summary>
        public double? Eff2Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 3 coefficient.
        /// </summary>
        public double? Eff3Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 4 coefficient.
        /// </summary>
        public double? Eff4Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 5 coefficient.
        /// </summary>
        public double? Eff5Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 6 coefficient.
        /// </summary>
        public double? Eff6Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 7 coefficient.
        /// </summary>
        public double? Eff7Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 8 coefficient.
        /// </summary>
        public double? Eff8Coef { get; set; }

        /// <summary>
        /// Gets or sets the eff 9 coefficient.
        /// </summary>
        public double? Eff9Coef { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the series data.
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the pump model.
        /// </summary>
        public string PumpModel { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        public ESPManufacturerModel Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the curve points.
        /// </summary>
        public IList<ESPCurvePointModel> CurvePoints { get; set; }

    }
}
