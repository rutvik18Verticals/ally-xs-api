namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms
{
    /// <summary>
    /// A RTU class with properties
    /// </summary>
    public class RTU : AlarmDocumentBase
    {
        /// <summary>
        /// Gets or sets whether callout is enabled for the RTU.
        /// </summary>
        public bool? CalloutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the normal state.
        /// </summary>
        public bool? NormalState { get; set; }

        /// <summary>
        /// Gets or sets whether the RTU is locked.
        /// </summary>
        public bool? Locked { get; set; }
    }

}
