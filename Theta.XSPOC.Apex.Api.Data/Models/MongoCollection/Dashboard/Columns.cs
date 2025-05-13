namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// Column Model.
    /// </summary>
    public class Columns
    {

        /// <summary>
        /// Gets or sets the Order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the column label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the key for retrival from api.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets flag if it is selected.
        /// </summary>
        public bool Selected { get; set; }

    }
}
