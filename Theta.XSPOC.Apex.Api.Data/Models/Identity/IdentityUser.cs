using Microsoft.AspNetCore.Identity;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.Identity
{
    /// <summary>
    /// Represents the AppUser.
    /// </summary>
    public partial class AppUser : IdentityUser<int>
    {

        /// <summary>
        /// Gets or sets the WellControl.
        /// </summary>
        public bool WellControl { get; set; }

        /// <summary>
        /// Gets or sets the WellConfig.
        /// </summary>
        public bool WellConfig { get; set; }

        /// <summary>
        /// Gets or sets the Admin.
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// Gets or sets the AdminLite.
        /// </summary>
        public bool AdminLite { get; set; }

        /// <summary>
        /// Gets or sets the WellAdmin.
        /// </summary>
        public bool WellAdmin { get; set; }

        /// <summary>
        /// Gets or sets the WellConfigLite.
        /// </summary>
        public bool WellConfigLite { get; set; }

        /// <summary>
        /// Gets or sets the LastAccessDate.
        /// </summary>
        public DateTime? LastAccessDate { get; set; }

        /// <summary>
        /// Gets or sets the WebToken.
        /// </summary>
        public string WebToken { get; set; }

        /// <summary>
        /// Gets or sets the WebTokenExpire.
        /// </summary>
        public DateTime? WebTokenExpire { get; set; }

        /// <summary>
        /// Gets or sets the MustChangePassword.
        /// </summary>
        public bool MustChangePassword { get; set; }

        /// <summary>
        /// Gets or sets if the user is a first time login.
        /// </summary>
        public bool IsFirstTimeLogin { get; set; }

    }
}
