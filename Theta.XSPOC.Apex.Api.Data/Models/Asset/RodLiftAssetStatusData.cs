using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.Asset
{
    /// <summary>
    /// Represents the rod list asset status data.
    /// </summary>
    public record RodLiftAssetStatusData
    {

        /// <summary>
        /// Gets or sets the rod lift asset status core data.
        /// </summary>
        public RodLiftAssetStatusCoreData CoreData { get; set; }

        /// <summary>
        /// Gets or sets the register data.
        /// </summary>
        public IList<RegisterData> RegisterData { get; set; }

        /// <summary>
        /// Gets or sets the alarm data.
        /// </summary>
        public IList<AlarmData> AlarmData { get; set; }

        /// <summary>
        /// Gets or sets the exception data.
        /// </summary>
        public IList<ExceptionData> ExceptionData { get; set; }

        /// <summary>
        /// Gets or sets the param standard data.
        /// </summary>
        public IList<ParamStandardData> ParamStandardData { get; set; }

        /// <summary>
        /// Gets or sets the rod strings.
        /// </summary>
        public IList<RodStringData> RodStrings { get; set; }

        /// <summary>
        /// Gets or sets the defaults the user has set.
        /// </summary>
        public IDictionary<string, UserDefaultItem> UserDefaults { get; set; }

        /// <summary>
        /// Gets or sets the defaults diagram type.
        /// </summary>
        public int DiagramType { get; set; }

        /// <summary>
        /// Gets or sets the gl analysis information data.
        /// </summary>
        public GLAnalysisInformationModel GLAnalysisData { get; set; }

        /// <summary>
        /// Gets or sets the esp motorinformation.
        /// </summary>
        public ESPMotorInformationModel ESPMotorInformation { get; set; }

        /// <summary>
        /// Gets or sets the esp pumpinformation.
        /// </summary>
        public ESPPumpInformationModel ESPPumpInformation { get; set; }

        /// <summary>
        /// Gets or sets the chemical injection information.
        /// </summary>
        public ChemicalInjectionInformationModel ChemicalInjectionInformation { get; set; }

        /// <summary>
        /// Gets or sets the plunger lift data.
        /// </summary>
        public PlungerLiftDataModel PlungerLiftData { get; set; }

        /// <summary>
        /// Gets or sets the facility Tag data.
        /// </summary>
        public FacilityDataModel FacilityTagData { get; set; }

        /// <summary>
        /// Gets or sets the PID Data Model.
        /// </summary>
        public PIDDataModel PIDDataModel { get; set; }

        /// <summary>
        /// Gets or sets the valve control data.
        /// </summary>
        public ValveControlDataModel ValveControlData { get; set; }

        /// <summary>
        /// Gets or sets the refresh interval in seconds.
        /// </summary>
        public int RefreshInterval { get; set; }

        /// <summary>
        /// Gets or sets the smarten live url.
        /// </summary>
        public string SmartenLiveUrl { get; set; }

    }
}
