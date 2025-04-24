namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the esp motor leads MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class ESPMotorLeads : LookupBase
    {

        /// <summary>
        /// Gets or sets the motor lead id.
        /// </summary>
        public int MotorLeadId { get; set; }

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
        /// Gets or sets the locked.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the motor lead type.
        /// </summary>
        public string MotorLeadType { get; set; }

        /// <summary>
        /// Gets or sets the motor lead description.
        /// </summary>
        public string MotorLeadDescription { get; set; }

    }

}
