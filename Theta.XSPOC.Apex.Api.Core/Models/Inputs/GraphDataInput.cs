using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for Graph Data Input
    /// </summary>
    public class GraphDataInput
    {

        #region Properties
        /// <summary>
        /// Gets or sets AssetId
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets StartDate
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets EndDate
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets Aggregate
        /// </summary>
        public string Aggregate { get; set; }

        /// <summary>
        /// Gets or sets AggregateMethod
        /// </summary>
        public string AggregateMethod { get; set; }
        /// <summary>
        /// Gets or sets ReteriveMixMax
        /// </summary>
        public bool ReteriveMixMax { get; set; }

        #endregion

    }
}