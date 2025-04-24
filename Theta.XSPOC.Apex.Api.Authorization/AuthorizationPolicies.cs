using Microsoft.AspNetCore.Authorization;

namespace Theta.XSPOC.Apex.Api.Authorization
{
    /// <summary>
    /// Static class to define authorization policies.
    /// </summary>
    public static class AuthorizationPolicies
    {

        /// <summary>
        /// Method to return the policy that requires the user to be able to control a well.
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy CanControlWell()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("WellControl", "True")
                .Build();
        }

        /// <summary>
        /// Method to return the policy that requires the user to be able to configure a well.
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy CanConfigWell()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("WellConfig", "True")
                .Build();
        }

    }
}
