using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the tblUserSecurity.
    /// </summary>
    [Table("tblUserSecurity")]
    public class UserSecurityEntity
    {

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Key]
        [Column("UserName")]
        public string UserName { get; set; }

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
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public string Email { get; set; }

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

    }
}
