using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.RodPump
{
    /// <summary>
    /// A class with properties for the rod pump details of an asset.
    /// </summary>
    public class RodPumpDetail : WellDetailsBase
    {

        /// <summary>
        /// Gets or sets the rods of the rod pump asset.
        /// </summary>
        public IList<Rod> Rods { get; set; }

        /// <summary>
        /// Gets or sets the motor kind of the motor asset.
        /// </summary>
        public Lookup.Lookup MotorKind { get; set; }

        /// <summary>
        /// Gets or sets the motor size of the motor asset.
        /// </summary>
        public Lookup.Lookup MotorSize { get; set; }

        /// <summary>
        /// Gets or sets the motor setting of the motor asset.
        /// </summary>
        public Lookup.Lookup MotorSetting { get; set; }

    }
}
