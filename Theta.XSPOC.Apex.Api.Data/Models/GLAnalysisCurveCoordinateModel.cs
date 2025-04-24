namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The GLAnalysisCurveCoordinateModel.
    /// </summary>
    public class GLAnalysisCurveCoordinateModel
    {

        /// <summary>
        /// Gets or sets Id.
        /// </summary>      
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets GasRate.
        /// </summary>      
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets IsInjectingGas.
        /// </summary>      
        public bool? IsInjectingGas { get; set; }

        /// <summary>
        /// Gets or sets State.
        /// </summary>      
        public int State { get; set; }

        /// <summary>
        /// Gets or sets InjectionPressureAtDepth.
        /// </summary>      
        public float? InjectionPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets ValveState.
        /// </summary>      
        public int ValveState { get; set; }

        /// <summary>
        /// Gets or sets TubingCriticalVelocityAtDepth.
        /// </summary>      
        public float? TubingCriticalVelocityAtDepth { get; set; }

        /// <summary>
        /// Gets or sets InjectionRateForTubingCriticalVelocity.
        /// </summary>      
        public float? InjectionRateForTubingCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or sets ClosingPressureAtDepth.
        /// </summary>      
        public float? ClosingPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets OpeningPressureAtDepth.
        /// </summary>      
        public float? OpeningPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets Depth.
        /// </summary>      
        public float? Depth { get; set; }

        /// <summary>
        /// Gets or sets GLWellValveTestRackOpeningPressure.
        /// </summary>      
        public float? GLWellValveTestRackOpeningPressure { get; set; }

        /// <summary>
        /// Gets or sets GLWellValveClosingPressureAtDepth.
        /// </summary>      
        public float? GLWellValveClosingPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets GLWellValveOpeningPressureAtDepth.
        /// </summary>      
        public float? GLWellValveOpeningPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets GLWellValveVerticalDepth.
        /// </summary>      
        public float? GLWellValveVerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets GLWellValveMeasuredDepth.
        /// </summary>      
        public float? GLWellValveMeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets BellowsArea.
        /// </summary>    
        public float BellowsArea { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>    
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Diameter.
        /// </summary>    
        public float? Diameter { get; set; }

        /// <summary>
        /// Gets or sets Manufacturer.
        /// </summary>    
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets OneMinusR.
        /// </summary>    
        public float? OneMinusR { get; set; }

        /// <summary>
        /// Gets or sets PortArea.
        /// </summary>    
        public float PortArea { get; set; }

        /// <summary>
        /// Gets or sets PortSize.
        /// </summary>    
        public float? PortSize { get; set; }

        /// <summary>
        /// Gets or sets PortToBellowsAreaRatio.
        /// </summary>    
        public float PortToBellowsAreaRatio { get; set; }

        /// <summary>
        /// Gets or sets ProductionPressureEffectFactor.
        /// </summary>    
        public float ProductionPressureEffectFactor { get; set; }

        /// <summary>
        /// Gets or sets TableAnalysisResultCurveID.
        /// </summary> 
        public int TableAnalysisResultCurveID { get; set; }

        /// <summary>
        /// Gets or sets TableCurveTypesID.
        /// </summary> 
        public int TableCurveTypesID { get; set; }

    }
}
