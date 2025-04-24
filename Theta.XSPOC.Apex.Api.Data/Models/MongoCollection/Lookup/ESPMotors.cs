namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the esp motors  MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class ESPMotors : LookupBase
    {

        /// <summary>
        /// Gets or sets the motor id.
        /// </summary>
        public int MotorId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public float? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the HP.
        /// </summary>
        public float HP { get; set; }

        /// <summary>
        /// Gets or sets the volts.
        /// </summary>
        public float Volts { get; set; }

        /// <summary>
        /// Gets or sets the amps.
        /// </summary>
        public float Amps { get; set; }

        /// <summary>
        /// Gets or sets the locked.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the rated temperature.
        /// </summary>
        public float? RatedTemperature { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the motor model.
        /// </summary>
        public string MotorModel { get; set; }

    }
}
