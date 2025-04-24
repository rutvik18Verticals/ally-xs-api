namespace Theta.XSPOC.Apex.Api.Common.Calculators
{
    /// <summary>
    /// Represents static constants needed in calculations.
    /// </summary>
    public static class Constants
    {

        /// <summary>
        /// The constant for psi per foot.
        /// </summary>
        public static double PsiPerFoot { get; set; } = 0.433;

        /// <summary>
        /// Mongo DB Collection Names
        /// </summary>
        public struct MongoDBCollection
        {
            /// <summary>
            /// /
            /// </summary>
            public const string ASSET_COLLECTION = "Asset";

            /// <summary>
            /// /
            /// </summary>
            public const string CARD_COLLECTION = "Card";
        }

    }
}
