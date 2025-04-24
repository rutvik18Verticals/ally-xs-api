namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents states.
    /// </summary>
    public interface IStates
    {

        /// <summary>
        /// Get poc type 17 card type ms by card type name.
        /// </summary>
        /// <returns>The <seealso cref="string"/>The states.</returns>
        string GetCardTypeNamePocType17CardTypeMS(int? causeId, string correlationId);

    }
}
