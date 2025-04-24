using MongoDB.Bson.Serialization.Attributes;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.ESP;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.GasLift;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.Injection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.PCP;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.RodPump;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// This class defines the base class for the asset detail.
    /// </summary>
    [BsonKnownTypes(typeof(DefaultAsset), typeof(RodPumpDetail), typeof(ESPDetail), typeof(GasLiftDetail), typeof(PCPDetail), typeof(InjectionDetails))]
    public abstract class AssetDetailBase
    {
    }
}
