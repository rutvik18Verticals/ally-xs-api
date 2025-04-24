using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Camera
{
    /// <summary>
    /// Represents a Camera collection.
    /// </summary>
    public class Camera : DocumentBase
    {
        /// <summary>
        /// Gets or sets the AssetId associated with the Camera.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Camera.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hostname of the Camera.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the port number for the Camera.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the username used to access the Camera.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password used to access the Camera.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the video format ID for the Camera.
        /// </summary>
        public int VideoFormatId { get; set; }

        /// <summary>
        /// Gets or sets the token associated with the Camera.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the camera type.
        /// </summary>
        public Lookup.Lookup CameraType { get; set; }

        /// <summary>
        /// Gets or sets the camera configuration.
        /// </summary>
        public Lookup.Lookup CameraConfiguration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the camera is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the legacy Id of the Contacts.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }
    }
}
