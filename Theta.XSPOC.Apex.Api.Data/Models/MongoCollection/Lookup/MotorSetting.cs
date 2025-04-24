namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the motor settings; to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class MotorSetting : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the motor setting.
        /// </summary>
        public int MotorSettingId { get; set; }

        /// <summary>
        /// Gets or sets the name of the motor setting.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Full Load RPM. Nullable.
        /// </summary>
        public float? FullLoadRPM { get; set; }

        /// <summary>
        /// Gets or sets the maximum load for the motor setting. Nullable.
        /// </summary>
        public float? MaxLoad { get; set; }

        /// <summary>
        /// Gets or sets the maximum SPV for the motor setting. Nullable.
        /// </summary>
        public float? MaxSPV { get; set; }

        /// <summary>
        /// Gets or sets the rated horsepower for the motor setting. Nullable.
        /// </summary>
        public float? RatedHP { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated motor size. Nullable.
        /// </summary>
        public int? MotorSizeId { get; set; }

    }
}
