namespace Theta.XSPOC.Apex.Api.Common
{
    /// <summary>
    /// Provides commonly used Status Code description.
    /// </summary>
    public class StatusCodesUtility
    {
        /// <summary>
        /// Gets Status Code description for a specific Status Code.        
        /// </summary>
        public static string GetStatusCodeMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "The request has succeeded.",
                201 => "The request has been fulfilled and has resulted in one or more new resources being created.",
                202 => "The request has been accepted for processing, but the processing has not been completed.",
                204 => "The server successfully processed the request and is not returning any content.",
                400 => "The request contains invalid data.",
                401 => "You are not authorized to access this resource.",
                403 => "Access to this resource is forbidden.",
                404 => "The requested resource could not be found.",
                405 => "The method specified in the request is not allowed.",
                409 => "Failed to Update. Please try again later.",
                500 => "An unexpected error occurred. Please try again later.",
                501 => "The server does not support the functionality required to fulfill the request.",
                _ => "Unknown",
            };
        }
    }
}
