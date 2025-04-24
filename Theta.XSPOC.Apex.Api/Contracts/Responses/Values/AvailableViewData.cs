namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Describes a available view data value that needs to be send out.
    /// </summary>
    public class AvailableViewData
    {

        /// <summary>
        /// The  id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The  type name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The selected.
        /// </summary>
        public bool IsSelectedView { get; set; }

    }
}
