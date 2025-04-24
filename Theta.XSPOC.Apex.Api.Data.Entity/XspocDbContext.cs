using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity.Alarms;
using Theta.XSPOC.Apex.Api.Data.Entity.Camera;
using Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup;
using Theta.XSPOC.Apex.Api.Data.Entity.Logging;
using Theta.XSPOC.Apex.Api.Data.Entity.Models;
using Theta.XSPOC.Apex.Api.Data.Entity.RodLift;
using Theta.XSPOC.Apex.Api.Data.Entity.XDIAG;
using Theta.XSPOC.Apex.Kernel.Data.Sql.EntitySetup.Utilities;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The database context for XSPOC database.
    /// </summary>
    public class XspocDbContext : DbContext
    {

        #region Private Fields

        private readonly IDateTimeConverter _dateTimeConverter;

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor that creates a new instance of the <seealso cref="XspocDbContext"/>.
        /// </summary>
        /// <param name="dateTimeConverter">
        /// The <seealso cref="IDateTimeConverter"/> used to convert dates to and from app server time and apply
        /// offsets from the application server time configured in the app settings.
        /// </param>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="dateTimeConverter"/> is null.
        /// </exception>
        public XspocDbContext(IDateTimeConverter dateTimeConverter, DbContextOptions<XspocDbContext> options)
            : base(options)
        {
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
        }

        /// <summary>
        /// This protected constructor allows multiple concrete subclasses to call this base constructor using their
        /// different generic <seealso cref="DbContextOptions{TContext}"/> instances. See article at
        /// https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#dbcontextoptions
        /// </summary>
        /// <param name="dateTimeConverter"></param>
        /// <param name="contextOptions">The non generic db context options.</param>
        protected XspocDbContext(IDateTimeConverter dateTimeConverter, DbContextOptions contextOptions)
            : base(contextOptions)
        {
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
        }

        #endregion

        #region Table Properties

        /// <summary>
        /// Gets or sets the current alarm config by poc type data table.
        /// </summary>
        public virtual DbSet<AlarmConfigByPocTypeEntity> AlarmConfigByPocTypes { get; set; }

        /// <summary>
        /// Gets or sets the current alarm config by poc type data table.
        /// </summary>
        public virtual DbSet<AlarmEventEntity> AlarmEvents { get; set; }

        /// <summary>
        /// Gets or sets the camera alarms data table.
        /// </summary>
        public virtual DbSet<CameraAlarmEntity> CameraAlarms { get; set; }

        /// <summary>
        /// Gets or sets the camera alarm types data table.
        /// </summary>
        public virtual DbSet<CameraAlarmTypeEntity> CameraAlarmTypes { get; set; }

        /// <summary>
        /// Gets or sets the cameras data table.
        /// </summary>
        public virtual DbSet<CameraEntity> Cameras { get; set; }

        /// <summary>
        /// Gets or sets the card data table.
        /// </summary>
        public virtual DbSet<CardDataEntity> CardData { get; set; }

        /// <summary>
        /// Gets or sets the current raw scan data table.
        /// </summary>
        public virtual DbSet<CurrentRawScanDataEntity> CurrentRawScans { get; set; }

        /// <summary>
        /// Gets or sets the custom rod pumping units table.
        /// </summary>
        public virtual DbSet<CustomPumpingUnitEntity> CustomPumpingUnits { get; set; }

        /// <summary>
        /// Gets or sets the ESP analysis results table.
        /// </summary>
        public virtual DbSet<ESPAnalysisResultsEntity> ESPAnalysisResults { get; set; }

        /// <summary>
        /// Gets or sets the exceptions table.
        /// </summary>
        public virtual DbSet<ExceptionEntity> Exceptions { get; set; }

        /// <summary>
        /// Gets or sets the events table.
        /// </summary>
        public virtual DbSet<EventsEntity> Events { get; set; }

        /// <summary>
        /// Gets or sets the event groups table.
        /// </summary>
        public virtual DbSet<EventGroupsEntity> EventGroups { get; set; }

        /// <summary>
        /// Gets or sets the group membership cache table.
        /// </summary>
        public virtual DbSet<GroupMembershipCacheEntity> GroupMembershipCache { get; set; }

        /// <summary>
        /// Gets or sets the GroupStandardColumn table.
        /// </summary>
        public virtual DbSet<GroupStandardColumnEntity> GroupStandardColumns { get; set; }

        /// <summary>
        /// Gets or sets the DataTypes table.
        /// </summary>
        public virtual DbSet<DataTypesEntity> DataTypes { get; set; }

        /// <summary>
        /// Gets or sets the NodeMasters table.
        /// </summary>
        public virtual DbSet<NodeMasterEntity> NodeMasters { get; set; }

        /// <summary>
        /// Gets or sets the NodeTree table.
        /// </summary>
        public virtual DbSet<NodeTreeEntity> NodeTree { get; set; }

        /// <summary>
        /// Gets or sets the WellDetails table.
        /// </summary>
        public virtual DbSet<WellDetailsEntity> WellDetails { get; set; }

        /// <summary>
        /// Gets or sets the WellStatisticEntity table.
        /// </summary>
        public virtual DbSet<WellStatisticEntity> WellStatistics { get; set; }

        /// <summary>
        /// Gets or sets the String table.
        /// </summary>
        public virtual DbSet<StringsEntity> Strings { get; set; }

        /// <summary>
        /// Gets or sets the ConditionalFormat table.
        /// </summary>
        public virtual DbSet<ConditionalFormatEntity> ConditionalFormats { get; set; }

        /// <summary>
        /// Gets or sets the GroupStatusView table.
        /// </summary>
        public virtual DbSet<GroupStatusViewEntity> GroupStatusView { get; set; }

        /// <summary>
        /// Gets or sets the GroupStatusColumnFormat table.
        /// </summary>
        public virtual DbSet<GroupStatusColumnFormatEntity> GroupStatusColumnFormats { get; set; }

        /// <summary>
        /// Gets or sets the ParamStandardTypes table.
        /// </summary>
        public virtual DbSet<ParamStandardTypesEntity> ParamStandardTypes { get; set; }

        /// <summary>
        /// Gets or sets the Parameter table.
        /// </summary>
        public virtual DbSet<ParameterEntity> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the State table.
        /// </summary>
        public virtual DbSet<StatesEntity> States { get; set; }

        /// <summary>
        /// Gets or sets the GroupStatusViewsColumn table.
        /// </summary>
        public virtual DbSet<GroupStatusViewsColumnEntity> GroupStatusViewsColumns { get; set; }

        /// <summary>
        /// Gets or sets the GroupStatusColumn table.
        /// </summary>
        public virtual DbSet<GroupStatusColumnEntity> GroupStatusColumns { get; set; }

        /// <summary>
        /// Gets or sets the control actions table.
        /// </summary>
        public virtual DbSet<ControlActionsEntity> ControlActions { get; set; }

        /// <summary>
        /// Gets or sets the poc type actions table.
        /// </summary>
        public virtual DbSet<POCTypeActionsEntity> POCTypeActions { get; set; }

        /// <summary>
        /// Gets or sets the esp well details table.
        /// </summary>  
        public virtual DbSet<ESPWellDetailsEntity> ESPWellDetails { get; set; }

        /// <summary>
        /// Gets or sets the esp cables table.
        /// </summary>  
        public virtual DbSet<ESPCablesEntity> ESPCables { get; set; }

        /// <summary>
        /// Gets or sets the esp motor leads table.
        /// </summary>  
        public virtual DbSet<ESPMotorLeadsEntity> ESPMotorLeads { get; set; }

        /// <summary>
        /// Gets or sets the esp seals leads table.
        /// </summary>  
        public virtual DbSet<ESPSealsEntity> ESPSeals { get; set; }

        /// <summary>
        /// Gets or sets the esp motor table.
        /// </summary>  
        public virtual DbSet<ESPMotorsEntity> ESPMotors { get; set; }

        /// <summary>
        /// Gets or sets the vwAggregateCameraAlarmStatus table.
        /// </summary>
        public virtual DbSet<VwAggregateCameraAlarmStatus> VwAggregateCameraAlarmStatuses { get; set; }

        /// <summary>
        /// Gets or sets the vwOperationalScoresLast table.
        /// </summary>
        public virtual DbSet<VwOperationalScoresLast> VwOperationalScoresLasts { get; set; }

        /// <summary>
        /// Gets or sets the vw30DayRuntimeAverage table.
        /// </summary>
        public virtual DbSet<Vw30DayRuntimeAverage> Vw30DayRuntimeAverages { get; set; }

        /// <summary>
        /// Gets or sets the port master table.
        /// </summary>  
        public virtual DbSet<PortMaster> PortConfigurations { get; set; }

        /// <summary>
        /// Gets or sets the GetLatestValuesByParamStandardType.
        /// </summary>
        /// <param name="paramStandardType">The param standard type.</param>
        /// <returns>Result from <seealso cref="TvfGetLatestValuesByParamStandardType"/>.</returns>
        public IQueryable<GetLatestValuesByParamStandardType> TvfGetLatestValuesByParamStandardType(int paramStandardType)
        {
            return FromExpression(() => TvfGetLatestValuesByParamStandardType(paramStandardType));
        }

        /// <summary>
        /// Gets or sets the GetLatestValuesStateByParamStandardType.
        /// </summary>
        /// <param name="paramStandardType">The param standard type.</param>
        /// <returns>Result from <seealso cref="TvfGetLatestValuesByParamStandardTypeWithStateIdText"/>.</returns>
        public IQueryable<GetLatestValuesStateByParamStandardType> TvfGetLatestValuesByParamStandardTypeWithStateIdText(
            int paramStandardType)
        {
            return FromExpression(() => TvfGetLatestValuesByParamStandardTypeWithStateIdText(paramStandardType));
        }

        /// <summary>
        /// Gets or sets the curve coordinates table.
        /// </summary>
        public virtual DbSet<CurveCoordinatesEntity> CurveCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the curve set coordinates table.
        /// </summary>
        public virtual DbSet<CurveSetCoordinatesEntity> CurveSetCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the analysis result curves table.
        /// </summary>
        public virtual DbSet<AnalysisResultCurvesEntity> AnalysisResultCurves { get; set; }

        /// <summary>
        /// Gets or sets the curve type table.
        /// </summary>
        public virtual DbSet<CurveTypesEntity> CurveTypes { get; set; }

        /// <summary>
        /// Gets or sets the states table.
        /// </summary>
        public virtual DbSet<PumpingUnitManufacturerEntity> PumpingUnitManufacturer { get; set; }

        /// <summary>
        /// Gets or sets the status registers table.
        /// </summary>
        public virtual DbSet<PumpingUnitsEntity> PumpingUnits { get; set; }

        /// <summary>
        /// Gets or sets the system parameters table.
        /// </summary>
        public virtual DbSet<SystemParametersEntity> SystemParameters { get; set; }

        /// <summary>
        /// Gets or sets the well test table.
        /// </summary>
        public virtual DbSet<WellTestEntity> WellTest { get; set; }

        /// <summary>
        /// Gets or sets the XDiag result table.
        /// </summary>
        public virtual DbSet<XDiagResultEntity> XDiagResult { get; set; }

        /// <summary>
        /// Gets or sets the esp curve point table.
        /// </summary>
        public virtual DbSet<ESPCurvePointEntity> ESPCurvePoints { get; set; }

        /// <summary>
        /// Gets or sets the esp manufacturers table.
        /// </summary>
        public virtual DbSet<ESPManufacturerEntity> ESPManufacturers { get; set; }

        /// <summary>
        /// Gets or sets the esp pumps table.
        /// </summary>
        public virtual DbSet<ESPPumpEntity> ESPPumps { get; set; }

        /// <summary>
        /// Gets or sets the esp well pumps table.
        /// </summary>
        public virtual DbSet<ESPWellPumpEntity> ESPWellPumps { get; set; }

        /// <summary>
        /// Gets or sets the esp well cables table.
        /// </summary>
        public virtual DbSet<ESPWellCableEntity> ESPWellCables { get; set; }

        /// <summary>
        /// Gets or sets the esp well seals table.
        /// </summary>
        public virtual DbSet<ESPWellSealEntity> ESPWellSeals { get; set; }

        /// <summary>
        /// Gets or sets the esp well motors table.
        /// </summary>
        public virtual DbSet<ESPWellMotorEntity> ESPWellMotors { get; set; }

        /// <summary>
        /// Gets or sets the esp well motor lead table.
        /// </summary>
        public virtual DbSet<ESPWellMotorLeadEntity> ESPWellMotorLead { get; set; }

        /// <summary>
        /// Gets or sets the SurveyDatum result table.
        /// </summary>
        public virtual DbSet<SurveyDataEntity> SurveyData { get; set; }

        /// <summary>
        /// Gets or sets the group status user view  table.
        /// Gets or sets the analysis curves set member table.
        /// </summary>
        public virtual DbSet<AnalysisCurveSetMembersEntity> AnalysisCurveSetMembers { get; set; }

        /// <summary>
        /// Gets or sets the esp tornado  curve set annotations table.
        /// </summary>
        public virtual DbSet<GroupStatusUserViewEntity> GroupStatusUserView { get; set; }

        /// <summary>
        /// Gets or sets the facility tags table.
        /// </summary>
        public virtual DbSet<FacilityTagsEntity> FacilityTags { get; set; }

        /// <summary>
        /// Gets or sets the facility tags group table.
        /// </summary>
        public virtual DbSet<FacilityTagGroupsEntity> FacilityTagGroups { get; set; }

        /// <summary>
        /// Gets or sets the LocalePhraseEntity table.
        /// </summary>
        public virtual DbSet<LocalePhraseEntity> LocalePhrases { get; set; }

        /// <summary>
        /// Gets or sets the GL Analysis Results table.
        /// </summary>
        public virtual DbSet<GLAnalysisResultsEntity> GLAnalysisResults { get; set; }

        /// <summary>
        /// Gets or sets the GL Orifice Status result table.
        /// </summary>
        public virtual DbSet<GLOrificeStatusEntity> GLOrificeStatus { get; set; }

        /// <summary>
        /// Gets or sets the GL Valve table.
        /// </summary>
        public virtual DbSet<GLValveEntity> GLValve { get; set; }

        /// <summary>
        /// Gets or sets the GL Valve Status table.
        /// </summary>
        public virtual DbSet<GLValveStatusEntity> GLValveStatus { get; set; }

        /// <summary>
        /// Gets or sets the GL Well Orifice table.
        /// </summary>
        public virtual DbSet<GLWellOrificeEntity> GLWellOrifice { get; set; }

        /// <summary>
        /// Gets or sets the GL Well Valve table.
        /// </summary>
        public virtual DbSet<GLWellValveEntity> GLWellValve { get; set; }

        /// <summary>
        /// Gets or sets the GL Well Detail table.
        /// </summary>
        public virtual DbSet<GLWellDetailEntity> GLWellDetail { get; set; }

        /// <summary>
        /// Gets or sets the Analysis Correlation table.
        /// </summary>
        public virtual DbSet<AnalysisCorrelationEntity> AnalysisCorrelation { get; set; }

        /// <summary>
        /// Gets or sets the IPR Analysis Results table.
        /// </summary>
        public virtual DbSet<IPRAnalysisResultsEntity> IPRAnalysisResult { get; set; }

        /// <summary>
        /// Gets or sets the Sensitivity Analysis IPR table.
        /// </summary>
        public virtual DbSet<SensitivityAnalysisIPREntity> SensitivityAnalysisIPR { get; set; }

        /// <summary>
        /// Gets or sets the GL Manufactuerer table.
        /// </summary>
        public virtual DbSet<GLManufacturerEntity> GLManufacturer { get; set; }

        /// <summary>
        /// Gets or sets the esp tornado  curve set annotations table.
        /// </summary>
        public virtual DbSet<ESPTornadoCurveSetAnnotationsEntity> ESPTornadoCurveSetAnnotations { get; set; }

        /// <summary>
        /// Gets or sets the analysis curve set table.
        /// </summary>
        public virtual DbSet<AnalysisCurveSetEntity> AnalysisCurveSets { get; set; }

        /// <summary>
        /// Gets or sets the AnalysisTypeEntity table.
        /// </summary>
        public virtual DbSet<AnalysisTypeEntity> AnalysisTypeEntities { get; set; }

        /// <summary>
        /// Gets or sets the ApplicationEntity table.
        /// </summary>
        public virtual DbSet<ApplicationEntity> ApplicationEntities { get; set; }

        /// <summary>
        /// Gets or sets the CorrelationEntity table.
        /// </summary>
        public virtual DbSet<CorrelationEntity> CorrelationEntities { get; set; }

        /// <summary>
        /// Gets or sets the CorrelationTypeEntity table.
        /// </summary>
        public virtual DbSet<CorrelationTypeEntity> CorrelationTypeEntities { get; set; }

        /// <summary>
        /// Gets or sets the AnalysisCurveSetTypeEntity table.
        /// </summary>
        public virtual DbSet<AnalysisCurveSetTypeEntity> AnalysisCurveSetTypeEntities { get; set; }

        /// <summary>
        /// Gets or sets the UnitTypeEntity table.
        /// </summary>
        public virtual DbSet<UnitTypeEntity> UnitTypes { get; set; }

        /// <summary>
        /// Gets or sets the GlflowControlDeviceStateEntity table.
        /// </summary>
        public virtual DbSet<GlflowControlDeviceStateEntity> GlflowControlDeviceStateEntities { get; set; }

        /// <summary>
        /// Gets or sets the GLValveConfigurationOptionsEntity table.
        /// </summary>
        public virtual DbSet<GLValveConfigurationOptionsEntity> GLValveConfigurationOptionsEntities { get; set; }

        /// <summary>
        /// Gets or sets the group parameter table.
        /// </summary>
        public virtual DbSet<GroupParameterEntity> GroupParameters { get; set; }

        /// <summary>
        /// Gets or sets the Data History Entity table.
        /// </summary>
        public virtual DbSet<DataHistoryEntity> DataHistory { get; set; }

        /// <summary>
        /// Gets or sets the Data History Archive Entity table.
        /// </summary>
        public virtual DbSet<DataHistoryArchiveEntity> DataHistoryArchive { get; set; }

        /// <summary>
        /// Gets or sets the XDiagRodResult table.
        /// </summary>
        public virtual DbSet<XDiagRodResultsEntity> XDiagRodResult { get; set; }

        /// <summary>
        /// Gets or sets the Group Data History table.
        /// </summary>
        public virtual DbSet<GroupDataHistoryEntity> GroupDataHistory { get; set; }

        /// <summary>
        /// Gets or sets the Failure Component table.
        /// </summary>
        public virtual DbSet<FailureComponentEntity> FailureComponent { get; set; }

        /// <summary>
        /// Gets or sets the Failure Sub Component table.
        /// </summary>
        public virtual DbSet<FailureSubComponentEntity> FailureSubComponent { get; set; }

        /// <summary>
        /// Gets or sets the Failure Meter Column table.
        /// </summary>
        public virtual DbSet<MeterColumnEntity> MeterColumn { get; set; }

        /// <summary>
        /// Gets or sets the PCSF Datalog Configuration table.
        /// </summary>
        public virtual DbSet<PCSFDatalogConfigurationEntity> PCSFDatalogConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the Meter History table.
        /// </summary>
        public virtual DbSet<MeterHistoryEntity> MeterHistory { get; set; }

        /// <summary>
        /// Gets or sets the PCSF Datalog Configuration table.
        /// </summary>
        public virtual DbSet<PCSFDatalogRecordEntity> PCSFDatalogRecord { get; set; }

        /// <summary>
        /// Gets or sets the PCSF Datalog Configuration table.
        /// </summary>
        public virtual DbSet<OperationalScoreEntity> OperationalScore { get; set; }

        /// <summary>
        /// Gets or sets the Production Statistics table.
        /// </summary>
        public virtual DbSet<ProductionStatisticsEntity> ProductionStatistics { get; set; }

        /// <summary>
        /// Gets or sets the Plunger Lift DataHistory table.
        /// </summary>
        public virtual DbSet<PlungerLiftDataHistoryEntity> PlungerLiftDataHistory { get; set; }

        /// <summary>
        /// Gets or sets the transactions table.
        /// </summary>
        public virtual DbSet<TransactionEntity> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the Perforation table.
        /// </summary>
        public virtual DbSet<PerforationEntity> Perforation { get; set; }

        /// <summary>
        /// Gets or sets the glcurvesetAnnotations table.
        /// </summary>
        public virtual DbSet<GLCurveSetAnnotationEntity> GLCurveSetAnnotation { get; set; }

        /// <summary>
        /// Gets or sets the Set Point Group table.
        /// </summary>
        public virtual DbSet<SetpointGroupEntity> SetpointGroups { get; set; }

        /// <summary>
        /// Gets or sets the transactions table.
        /// </summary>
        public virtual DbSet<SavedParameterEntity> SavedParameters { get; set; }

        /// <summary>
        /// Gets or sets the Failure table.
        /// </summary>
        public virtual DbSet<FailureEntity> Failures { get; set; }

        /// <summary>
        /// Gets or sets the host alarm table.
        /// </summary>
        public virtual DbSet<HostAlarmEntity> HostAlarm { get; set; }

        /// <summary>
        /// Gets or sets the tubings table.
        /// </summary>
        public virtual DbSet<TubingEntity> Tubings { get; set; }

        /// <summary>
        /// Gets or sets the tubing sizes table.
        /// </summary>
        public virtual DbSet<TubingSizeEntity> TubingSizes { get; set; }

        /// <summary>
        /// Gets or sets the group status table.
        /// </summary>
        public virtual DbSet<GroupStatusTableEntity> GroupStatusTable { get; set; }

        /// <summary>
        /// Gets or sets the poc type table.
        /// </summary>
        public virtual DbSet<POCTypeEntity> PocType { get; set; }

        /// <summary>
        /// Gets or sets the company table.
        /// </summary>
        public virtual DbSet<CompanyEntity> Company { get; set; }

        /// <summary>
        /// Gets or sets the exceptions table.
        /// </summary>
        public virtual DbSet<ExceptionEntity> Exception { get; set; }

        /// <summary>
        /// Gets or sets the rod table.
        /// </summary>
        public virtual DbSet<RodEntity> Rod { get; set; }

        /// <summary>
        /// Gets or sets the rod grade entity.
        /// </summary>
        public virtual DbSet<RodGradeEntity> RodGrade { get; set; }

        /// <summary>
        /// Gets or sets the UserSecurityEntity table.
        /// </summary>
        public virtual DbSet<UserSecurityEntity> UserSecurity { get; set; }

        /// <summary>
        /// Get or sets the user defaults table.
        /// </summary>
        public virtual DbSet<UserDefaultEntity> UserDefaults { get; set; }

        /// <summary>
        /// Gets or sets the  analytics classification entity.
        /// </summary>
        public virtual DbSet<AnalyticsClassificationEntity> AnalyticsClassification { get; set; }

        /// <summary>
        /// Gets or sets the analytics classification type entity.
        /// </summary>
        public virtual DbSet<AnalyticsClassificationTypeEntity> AnalyticsClassificationType { get; set; }

        /// <summary>
        /// Gets or sets the alarm config by poctype entity.
        /// </summary>
        public virtual DbSet<AlarmConfigByPocTypeEntity> AlarmConfigByPocType { get; set; }

        /// <summary>
        /// Gets or sets the analytics xdiagoutputs entity.
        /// </summary>
        public virtual DbSet<XDIAGOutputEntity> XDIAGOutputs { get; set; }

        /// <summary>
        /// Gets or sets the analytics CameraAlarm entity.
        /// </summary>
        public virtual DbSet<CameraAlarmEntity> CameraAlarm { get; set; }

        /// <summary>
        /// Gets or sets the analytics AlarmEvents entity.
        /// </summary>
        public virtual DbSet<AlarmEventEntity> AlarmEvent { get; set; }

        /// <summary>
        /// Gets or sets the analytics Camera Alarm Type entity.
        /// </summary>
        public virtual DbSet<CameraAlarmTypeEntity> CameraAlarmType { get; set; }

        /// <summary>
        /// Gets or sets the XDIAG results last table.
        /// </summary>
        public virtual DbSet<XDIAGResultsLastEntity> XDIAGResultLast { get; set; }

        /// <summary>
        /// Gets or sets the rod pumping motor kinds table.
        /// </summary>
        public virtual DbSet<RodMotorKindEntity> RodMotorKinds { get; set; }

        /// <summary>
        /// Gets or sets the rod pump motor settings table.
        /// </summary>
        public virtual DbSet<RodMotorSettingEntity> RodMotorSettings { get; set; }

        /// <summary>
        /// Gets or sets the status registers table.
        /// </summary>
        public virtual DbSet<StatusRegisterEntity> StatusRegisters { get; set; }

        ///// <summary>
        ///// Gets or sets the rod pump motors table.
        ///// </summary>
        //public virtual DbSet<PumpingUnitsEntity> RodPumpMotors { get; set; }

        ///// <summary>
        ///// Gets or sets the rod pump manufacturers table.
        ///// </summary>
        //public virtual DbSet<PumpingUnitManufacturerEntity> RodPumpManufacturers { get; set; }

        /// <summary>
        /// Gets or sets the rod string sizes table.
        /// </summary>
        public virtual DbSet<RodStringSizeEntity> RodStringSizes { get; set; }

        /// <summary>
        /// Gets or sets the rod strings table.
        /// </summary>
        public virtual DbSet<RodEntity> RodStrings { get; set; }

        /// <summary>
        /// Gets or sets the rod string grades table.
        /// </summary>
        public virtual DbSet<RodGradeEntity> RodStringGrades { get; set; }

        /// <summary>
        /// Gets or sets the default graph view trends table.
        /// </summary>
        public virtual DbSet<GraphViewsEntity> GraphViews { get; set; }

        /// <summary>
        /// Gets or sets the default graph view trends table.
        /// </summary>
        public virtual DbSet<GraphViewTrendsEntity> GraphViewTrends { get; set; }

        /// <summary>
        /// Gets or sets the default graph view trends table.
        /// </summary>
        public virtual DbSet<GraphViewSettingsEntity> GraphViewSetting { get; set; }

        #endregion

        #region DbContext Override Methods

        /// <summary>
        /// This method will setup the configuration on the tables. For example defining unique keys,
        /// foreign keys, ect.
        /// </summary>
        /// <param name="modelBuilder">the model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntitySetup.Events.Setup(modelBuilder);
            EntitySetup.EventGroups.Setup(modelBuilder);
            EntitySetup.GroupMembershipCache.Setup(modelBuilder);
            EntitySetup.AlarmConfigByPocType.Setup(modelBuilder);
            CurrentRawScanData.Setup(modelBuilder);
            ExceptionTable.Setup(modelBuilder);
            EntitySetup.FacilityTags.Setup(modelBuilder);
            EntitySetup.HostAlarm.Setup(modelBuilder);
            EntitySetup.CardData.Setup(modelBuilder);
            NodeMaster.Setup(modelBuilder);
            EntitySetup.PumpingUnits.Setup(modelBuilder);
            XdiagResult.Setup(modelBuilder);
            EntitySetup.PumpingUnitManufacturer.Setup(modelBuilder);
            RodString.Setup(modelBuilder);
            EntitySetup.RodMotorSettings.Setup(modelBuilder);
            EntitySetup.States.Setup(modelBuilder);
            StatusRegister.Setup(modelBuilder);
            UserDefault.Setup(modelBuilder);
            EntitySetup.WellDetails.Setup(modelBuilder);
            EntitySetup.WellTest.Setup(modelBuilder);
            EntitySetup.States.Setup(modelBuilder);
            LocalePhrase.Setup(modelBuilder);
            ESPAnalysisResult.Setup(modelBuilder);
            ESPCurvePoint.Setup(modelBuilder);
            ESPPump.Setup(modelBuilder);
            EntitySetup.GroupStatusUserView.Setup(modelBuilder);
            EntitySetup.GroupStatusView.Setup(modelBuilder);
            EntitySetup.FacilityTags.Setup(modelBuilder);
            GroupStandardColumn.Setup(modelBuilder);
            EntitySetup.DataTypes.Setup(modelBuilder);
            EntitySetup.GLAnalysisResults.Setup(modelBuilder);
            EntitySetup.GLOrificeStatus.Setup(modelBuilder);
            EntitySetup.GLValve.Setup(modelBuilder);
            EntitySetup.GLValveStatus.Setup(modelBuilder);
            EntitySetup.CurveCoordinates.Setup(modelBuilder);
            EntitySetup.CurveSetCoordinates.Setup(modelBuilder);
            CurveType.Setup(modelBuilder);
            ESPWellPump.Setup(modelBuilder);
            GroupStatusColumn.Setup(modelBuilder);
            GroupParameter.Setup(modelBuilder);
            Parameter.Setup(modelBuilder);
            GroupStatusViewsColumn.Setup(modelBuilder);
            DataHistoryResults.Setup(modelBuilder);
            DataHistoryArchiveResults.Setup(modelBuilder);
            EntitySetup.XDiagRodResult.Setup(modelBuilder);
            EntitySetup.GroupDataHistory.Setup(modelBuilder);
            EntitySetup.PCSFDatalogConfiguration.Setup(modelBuilder);
            EntitySetup.MeterColumn.Setup(modelBuilder);
            EntitySetup.MeterHistory.Setup(modelBuilder);
            EntitySetup.FailureComponent.Setup(modelBuilder);
            EntitySetup.FailureSubComponent.Setup(modelBuilder);
            EntitySetup.SensitivityAnalysisIPR.Setup(modelBuilder);
            EntitySetup.PCSFDatalogRecord.Setup(modelBuilder);
            OperationalScoreResults.Setup(modelBuilder);
            EntitySetup.ProductionStatistics.Setup(modelBuilder);
            EntitySetup.PlungerLiftDataHistory.Setup(modelBuilder);
            EntitySetup.Perforation.Setup(modelBuilder);
            EntitySetup.GLCurveSetAnnotation.Setup(modelBuilder);
            SavedParameter.Setup(modelBuilder);
            EntitySetup.POCTypeActions.Setup(modelBuilder);
            EntitySetup.HostAlarm.Setup(modelBuilder);
            EntitySetup.Tubings.Setup(modelBuilder);
            EntitySetup.GroupStatusTable.Setup(modelBuilder);
            EntitySetup.Company.Setup(modelBuilder);
            AnalyticsClassifications.Setup(modelBuilder);
            EntitySetup.CameraAlarm.Setup(modelBuilder);
            EntitySetup.ESPWellDetails.Setup(modelBuilder);
            GraphViewTrend.Setup(modelBuilder);
            GraphViewSettings.Setup(modelBuilder);
            EntitySetup.ESPWellCables.Setup(modelBuilder);
            ESPWellMotorLeads.Setup(modelBuilder);
            EntitySetup.ESPWellMotors.Setup(modelBuilder);
            EntitySetup.ESPWellSeals.Setup(modelBuilder);

            modelBuilder.Entity<POCTypeEntity>(entity =>
            {
                entity.Property(e => e.PocType).ValueGeneratedNever();
            });

            modelBuilder.Entity<ExceptionEntity>(entity =>
            {
                entity.HasKey(e => new { e.NodeId, e.ExceptionGroupName }).IsClustered(false);
            });

            EntitySetup.Rod.Setup(modelBuilder);
            EntitySetup.RodGrade.Setup(modelBuilder);
            UserDefault.Setup(modelBuilder);

            modelBuilder.HasDbFunction(typeof(XspocDbContext).GetMethod(nameof(TvfGetLatestValuesByParamStandardType), new[] { typeof(int) }));
            modelBuilder.HasDbFunction(typeof(XspocDbContext).GetMethod(nameof(TvfGetLatestValuesByParamStandardTypeWithStateIdText), new[] { typeof(int) }));
            //modelBuilder.Entity<AppUser>().ToTable(nameof(UserSecurity));

            modelBuilder.ApplySQLDateTimeConstraints(_dateTimeConverter, LoggingModel.SqlDateTime);
        }

        #endregion

    }
}