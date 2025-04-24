namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The OrificeStatusModel.
    /// </summary>
    public class OrificeStatusModel
    {

        /// <summary>
        /// Gets or sets InjectionPressureAtDepth.
        /// </summary> 
        public float? InjectionPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets IsInjectingGas.
        /// </summary> 
        public bool? IsInjectingGas { get; set; }

        /// <summary>
        /// Gets or sets State.
        /// </summary> 
        public int State { get; set; }

        /// <summary>
        /// Gets or sets TubingCriticalVelocityAtDepth.
        /// </summary> 
        public float? TubingCriticalVelocityAtDepth { get; set; }

        /// <summary>
        /// Gets or sets InjectionRateForTubingCriticalVelocity.
        /// </summary> 
        public float? InjectionRateForTubingCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or sets Depth.
        /// </summary> 
        public float? Depth { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary> 
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Flow Manufacturer.
        /// </summary> 
        public object FlowManufacturer { get; set; }

        /// <summary>
        /// Gets or sets FlowMeasuredDepth.
        /// </summary> 
        public float? FlowMeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets Flow Vertical Depth.
        /// </summary> 
        public float FlowVerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets Flow Port Size.
        /// </summary> 
        public float FlowPortSize { get; set; }

    }
}
