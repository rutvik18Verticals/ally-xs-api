namespace Theta.XSPOC.Apex.Api.Common
{
    /// <summary>
    /// Represents a message catalog.
    /// </summary>
    public abstract class MessageCatalog
    {

        /// <summary>
        /// Contains alert messages.
        /// </summary>
        public static class AlertMessages
        {

            /// <summary>
            /// Represents the message for relevent request and response.
            /// </summary>
            public const string ValidateServiceResult = "Invalid Status Code at Service Result.";

        }

    }
}
