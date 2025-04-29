using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security
{
    /// <summary>
    /// This class defines the user MongoDB document.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User : CustomerDocumentBase
    {

        /// <summary>
        /// Gets or sets the connexia user id.
        /// </summary>
        public string CNX_UserId { get; set; }

        /// <summary>
        /// Gets or sets if the user is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the contact id.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string ContactId { get; set; }

        /// <summary>
        /// Gets or sets the last login date.
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// Gets or sets the grafa editor.
        /// </summary>
        public string GrafaEditor { get; set; }

        /// <summary>
        /// Gets or sets the license.
        /// </summary>
        public object License { get; set; }

        /// <summary>
        /// Gets or sets the B2C domain name.
        /// </summary>
        public string B2cdomainname { get; set; }

        /// <summary>
        /// Gets or sets the most common preferences.
        /// </summary>
        public IList<Preference> MostCommonPreferences { get; set; }

        /// <summary>
        /// Gets or sets MustChangePassword
        /// </summary>
        public bool MustChangePassword { get; set; }

    }
}