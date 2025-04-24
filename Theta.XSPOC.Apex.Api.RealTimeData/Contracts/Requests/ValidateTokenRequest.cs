namespace Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Requests
{
    public class ValidateTokenRequest
    {
        #region Properties

        /// <summary>
        /// UserAccount
        /// </summary>
        /// <example>abc@abc.com</example>        
        public string UserAccount { get; set; }

        /// <summary>
        /// Api Token Key.
        /// </summary>
        /// <example>abcd-xyz-token</example>        
        public string ApiTokenKey { get; set; }

        /// <summary>
        /// Api Token Value.
        /// </summary> 
        /// <example>U29tZVN1cGVyU2VjdXJlU3VwZXJMb25nU2VjcmV0S2V5QXNJdElzVGVycmlibHlMb25nQW5</example>        
        public string ApiTokenValue { get; set; }

        

        #endregion
    }
}
