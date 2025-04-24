using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.Camera
{
    /// <summary>
    /// The poc types table.
    /// </summary>
    [Table("tblCameras")]
    public class CameraEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("Name", TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hostname used to connect.
        /// </summary>
        [Column("Hostname", TypeName = "nvarchar")]
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the port used to connect.
        /// </summary>
        [Column("Port")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the username used to connect.
        /// </summary>
        [Column("Username", TypeName = "nvarchar")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password used to connect.
        /// </summary>
        [Column("Password", TypeName = "nvarchar")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets video format id.
        /// </summary>
        [Column("VideoFormatID")]
        public int VideoFormatId { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [Column("Token", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the camera type id.
        /// </summary>
        [Column("CameraTypeID")]
        public int CameraTypeId { get; set; }

        /// <summary>
        /// Gets or sets the camera configuration id.
        /// </summary>
        [Column("CameraConfigurationID")]
        public int? CameraConfigurationId { get; set; }

    }
}