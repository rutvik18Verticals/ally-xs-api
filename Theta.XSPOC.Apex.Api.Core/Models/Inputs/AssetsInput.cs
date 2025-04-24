namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// This represents the Assets Input.
    /// </summary>
    public class AssetsInput
    {

        /// <summary>
        /// The Asset Group id.
        /// </summary>
        public string AssetGroup { get; set; }

        /// <summary>
        /// Gets and sets the boolean value to check if the architecture is new.
        /// </summary>
        public bool IsNewArchitecture { get; set; }

    }
}
