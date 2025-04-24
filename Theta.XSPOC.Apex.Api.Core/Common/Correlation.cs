using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the available correlation names.
    /// </summary>
    public class Correlation : EnhancedEnumBase
    {

        #region Static Properties

        /// <summary>
        /// Gets the Hagedorn and Brown Correlation
        /// </summary>
        public static Correlation HagedornAndBrown { get; } =
            Create<Correlation>(1, Text.FromString("Hagedorn and Brown"));

        /// <summary>
        /// Gets the Beggs and Brill Correlation
        /// </summary>
        public static Correlation BeggsAndBrill { get; } =
            Create<Correlation>(2, Text.FromString("Beggs and Brill"));

        /// <summary>
        /// Gets the Beggs and Robinson Correlation
        /// </summary>
        public static Correlation BeggsAndRobinson { get; } =
            Create<Correlation>(3, Text.FromString("Beggs and Robinson"));

        /// <summary>
        /// Gets the Vasquez and Beggs Correlation
        /// </summary>
        public static Correlation VasquezAndBeggs { get; } =
            Create<Correlation>(4, Text.FromString("Vasquez and Beggs"));

        /// <summary>
        /// Gets the Brill and Beggs Correlation
        /// </summary>
        public static Correlation BrillAndBeggs { get; } =
            Create<Correlation>(5, Text.FromString("Brill and Beggs"));

        /// <summary>
        /// Gets the Vogel Correlation
        /// </summary>
        public static Correlation Vogel { get; } =
            Create<Correlation>(6, Text.FromString("Vogel"));

        /// <summary>
        /// Gets the Constant PI Correlation
        /// </summary>
        public static Correlation ConstantPI { get; } =
            Create<Correlation>(7, Text.FromString("Constant PI"));

        /// <summary>
        /// Gets the Composite Correlation
        /// </summary>
        public static Correlation Composite { get; } =
            Create<Correlation>(8, Text.FromString("Composite"));

        /// <summary>
        /// Gets the Coleman correlation
        /// </summary>
        public static Correlation Coleman { get; } =
            Create<Correlation>(9, Text.FromString("Coleman"));

        /// <summary>
        /// Gets the Turner correlation
        /// </summary>
        public static Correlation Turner { get; } =
            Create<Correlation>(10, Text.FromString("Turner"));

        /// <summary>
        /// Gets the Dynamic IPR correlation
        /// </summary>
        public static Correlation DynamicIPR { get; } =
            Create<Correlation>(11, Text.FromString("Dynamic"));

        /// <summary>
        /// Gets the Glaso correlation
        /// </summary>
        public static Correlation Glaso { get; } =
            Create<Correlation>(12, Text.FromString("Glaso"));

        /// <summary>
        /// Gets the Duns and Ros correlation
        /// </summary>
        public static Correlation DunsandRos { get; } =
            Create<Correlation>(13, Text.FromString("Duns and Ros"));

        /// <summary>
        /// Gets the Standing correlation
        /// </summary>
        public static Correlation Standing { get; } =
            Create<Correlation>(14, Text.FromString("Standing"));

        /// <summary>
        /// Gets the Fetkovich IPR correlation.
        /// </summary>
        public static Correlation Fetkovich { get; } =
            Create<Correlation>(15, Text.FromString("Fetkovich"));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Correlation with a specified key and name
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="name">The name</param>
        public Correlation(int key, Text name) : base(key, name)
        {
        }

        #endregion

    }
}
