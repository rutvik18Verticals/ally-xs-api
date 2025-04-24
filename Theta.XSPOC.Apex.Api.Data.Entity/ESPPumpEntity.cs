using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The esp pumps table.
    /// </summary>
    [Table("tblESPPumps")]
    public partial class ESPPumpEntity
    {

        /// <summary>
        /// Gets or sets the esp pump id.
        /// </summary>
        [Key]
        [Column("ESPPumpID")]
        public int ESPPumpId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        [Column("ManufacturerID")]
        public int? ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the pump.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Pump { get; set; }

        /// <summary>
        /// Gets or sets the min casing size.
        /// </summary>
        public float? MinCasingSize { get; set; }

        /// <summary>
        /// Gets or sets the housing pressure limit.
        /// </summary>
        public int? HousingPressureLimit { get; set; }

        /// <summary>
        /// Gets or sets the minimum bpd.
        /// </summary>
        [Column("MinBPD")]
        public int MinBPD { get; set; }

        /// <summary>
        /// Gets or sets the maximum bpd.
        /// </summary>
        [Column("MaxBPD")]
        public int MaxBPD { get; set; }

        /// <summary>
        /// Gets or sets the value indicating the use of coefficients.
        /// </summary>
        public bool? UseCoefficients { get; set; }

        /// <summary>
        /// Gets or sets the Head Intercept.
        /// </summary>
        public double? HeadIntercept { get; set; }

        /// <summary>
        /// Gets or sets the HP Intercept.
        /// </summary>
        [Column("HPIntercept")]
        public double? HPIntercept { get; set; }

        /// <summary>
        /// Gets or sets the Efficiency Intercept.
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
        [Column("HP1Coef")]
        public double? HP1Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 2 coefficient.
        /// </summary>
        [Column("HP2Coef")]
        public double? HP2Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 3 coefficient.
        /// </summary>
        [Column("HP3Coef")]
        public double? HP3Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 4 coefficient.
        /// </summary>
        [Column("HP4Coef")]
        public double? HP4Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 5 coefficient.
        /// </summary>
        [Column("HP5Coef")]
        public double? HP5Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 6 coefficient.
        /// </summary>
        [Column("HP6Coef")]
        public double? HP6Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 7 coefficient.
        /// </summary>
        [Column("HP7Coef")]
        public double? HP7Coef { get; set; }

        /// <summary>
        /// Gets or sets the hp 8 coefficient.
        /// </summary>
        [Column("HP8Coef")]
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
        /// Gets or sets the series.
        /// </summary>
        [MaxLength(50)]
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the pump model.
        /// </summary>
        [MaxLength(50)]
        public string PumpModel { get; set; }

    }
}
