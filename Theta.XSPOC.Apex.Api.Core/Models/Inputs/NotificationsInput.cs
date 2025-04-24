namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for notification Input Data
    /// </summary>
    public class NotificationsInput
    {

        /// <summary>
        /// Gets or sets the asset group name.
        /// </summary>
        public string AssetGroupName { get; set; }

        /// <summary>
        /// Gets or sets the asset GUID.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the notification type id.
        /// </summary>
        public int? NotificationTypeId { get; set; }

    }
}
