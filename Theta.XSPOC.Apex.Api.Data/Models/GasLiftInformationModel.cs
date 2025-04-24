namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the gas lift information model.
    /// </summary>
    public class GasLiftInformationModel
    {

        /// <summary>
        /// Gets or sets the gas injection rate.
        /// </summary>
        public string GasInjectionRate { get; set; }

        /// <summary>
        /// Gets or sets the yesterday gas injection volume.
        /// </summary>
        public string YesterdayGasInjectionVolume { get; set; }

    }
}
