namespace Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Requests
{
    public class AssetsByCustomerIdRequest
    {
        #region Properties

        /// <summary>
        /// Comma separated Customer ids.
        /// </summary>
        /// <example>1111d1d1-1ad1-1111-1f1a-11b1111e1111,2222h2d2-2ad1-2222-2f2a-2222222e2222</example>        
        public string CustomerIds { get; set; }        

        #endregion
    }
}
