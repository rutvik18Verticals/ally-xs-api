namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the casing sizes MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class CasingSizes : LookupBase
    {

        /// <summary>
        /// Gets or sets the inner diameter.
        /// </summary>
        public float InnerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the outer diameter.
        /// </summary>
        public float OuterDiameter { get; set; }

        /// <summary>
        /// Gets or sets the casing weight.
        /// </summary>
        public float? CasingWeight { get; set; }

    }
}
