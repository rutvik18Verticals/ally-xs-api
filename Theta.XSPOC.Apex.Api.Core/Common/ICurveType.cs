namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the ICurveType.
    /// </summary>
    public interface ICurveType
    {

        #region Properties

        /// <summary>
        /// Gets the unit category that this curve's x axis is measured in.
        /// </summary>
        UnitCategory XAxisUnitCategory { get; }

        /// <summary>
        /// Gets the unit category that this curve's y axis is measured in.
        /// </summary>
        UnitCategory YAxisUnitCategory { get; }

        #endregion

    }
}
