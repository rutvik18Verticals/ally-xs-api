using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// The analysis document.
    /// </summary>
    public class Analysis : AssetDocumentBase
    {

        /// <summary>
        /// Gets or sets the date of the well test or card.
        /// </summary>
        public DateTime AnalysisDate { get; set; }

        /// <summary>
        /// Gets or sets the date of Analysis Proccessed Date
        /// </summary>
        public DateTime AnalysisProccessedDate { get; set; }

        /// <summary>
        /// Gets or sets the analysis type. <seealso cref="AnalysisType"/>
        /// </summary>
        public string AnalysisType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string WellTestId { get; set; }

        /// <summary>
        /// Gets or sets the analysis result data.
        /// </summary>
        public AnalysisDocumentBase Result { get; set; }

        /// <summary>
        /// Gets or sets the legacy Id of the Alalysis.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

    }
}
