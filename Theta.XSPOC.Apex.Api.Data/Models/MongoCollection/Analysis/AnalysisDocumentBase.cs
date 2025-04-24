using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// The analysis document base.
    /// </summary>
    [BsonKnownTypes(typeof(IPRAnalysisResult), typeof(XDiagResults))]
    public class AnalysisDocumentBase
    {
    }
}
