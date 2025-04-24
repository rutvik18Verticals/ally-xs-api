namespace Theta.XSPOC.Apex.Api.Core.Models
{
    /// <summary>
    /// Represents a Device Type
    /// </summary>
    public enum DeviceType
    {

        /// <summary>
        /// RPC_Baker
        /// </summary>
        RPC_Baker = 1,

        /// <summary>
        /// RPC_EDI_860_435
        /// </summary>
        RPC_EDI_860_435 = 2,

        /// <summary>
        /// RPC_Lufkin_MPC
        /// </summary>
        RPC_Lufkin_MPC = 3,

        /// <summary>
        /// RPC_Lufkin_Sys60
        /// </summary>
        RPC_Lufkin_Sys60 = 4,

        /// <summary>
        /// RPC_Lufkin_PCM
        /// </summary>
        RPC_Lufkin_PCM = 5,

        /// <summary>
        /// RPC_Pickford
        /// </summary>
        RPC_Pickford = 6,

        /// <summary>
        /// RPC_Texaco
        /// </summary>
        RPC_Texaco = 7,

        /// <summary>
        /// RPC_Lufkin_SAM
        /// </summary>
        RPC_Lufkin_SAM = 8,

        /// <summary>
        /// RPC_EDI_700_800
        /// </summary>
        RPC_EDI_700_800 = 9,

        /// <summary>
        /// RPC_AE
        /// </summary>
        RPC_AE = 10,

        /// <summary>
        /// RPC_TundraSSi
        /// </summary>
        RPC_TundraSSi = 11,

        /// <summary>
        /// RPC_Unico
        /// </summary>
        RPC_Unico = 12,

        /// <summary>
        /// RPC_Trienertech
        /// </summary>
        RPC_Trienertech = 13,

        /// <summary>
        /// RPC_DJAX
        /// </summary>
        RPC_DJAX = 14,

        /// <summary>
        /// RPC_SPOC_Ironhorse
        /// </summary>
        RPC_SPOC_Ironhorse = 15,

        /// <summary>
        /// RPC_Well_Pilot
        /// </summary>
        RPC_Well_Pilot = 16,

        /// <summary>
        /// RPC_Spirit_SMARTEN
        /// </summary>
        RPC_Spirit_SMARTEN = 17,

        /// <summary>
        /// RPC_PMC_SALT
        /// </summary>
        RPC_PMC_SALT = 18,

        /// <summary>
        /// RPC_AE901
        /// </summary>
        RPC_AE901 = 19,

        /// <summary>
        /// RPC_BMTS
        /// </summary>
        RPC_BMTS = 20,

        /// <summary>
        /// ESP_Vortex
        /// </summary>
        ESP_Vortex = 21,

        /// <summary>
        /// ESP_Keltronics
        /// </summary>
        ESP_Keltronics = 22,

        /// <summary>
        /// ESP_CTI_1600_Alt
        /// </summary>
        ESP_CTI_1600_Alt = 23,

        /// <summary>
        /// ESP_Reda_Speedstar
        /// </summary>
        ESP_Reda_Speedstar = 24,

        /// <summary>
        /// ESP_Centrilift_GCS
        /// </summary>
        ESP_Centrilift_GCS = 25,

        /// <summary>
        /// ESP_CTI_1800_Alt
        /// </summary>
        ESP_CTI_1800_Alt = 26,

        /// <summary>
        /// ESP_CTI_1600_Std
        /// </summary>
        ESP_CTI_1600_Std = 27,

        /// <summary>
        /// ESP_Centrilift_ICM
        /// </summary>
        ESP_Centrilift_ICM = 28,

        /// <summary>
        /// ESP_CTI_1800_Std
        /// </summary>
        ESP_CTI_1800_Std = 29,

        /// <summary>
        /// ESP_Wood_Group_Vector_VII
        /// </summary>
        ESP_Wood_Group_Vector_VII = 30,

        /// <summary>
        /// ESP_Schlumberger_Uniconn
        /// </summary>
        ESP_Schlumberger_Uniconn = 31,

        /// <summary>
        /// ESP_GE_Appolo
        /// </summary>
        ESP_GE_Appolo = 32,

        /// <summary>
        /// ESP_Summit
        /// </summary>
        ESP_Summit = 33,

        /// <summary>
        /// ESP_Novomet_VSD
        /// </summary>
        ESP_Novomet_VSD = 35,

        /// <summary>
        /// ESP_Dover_AL_SPOC
        /// </summary>
        ESP_Dover_AL_SPOC = 36,

        /// <summary>
        /// ESP_Borets_Axiom_2
        /// </summary>
        ESP_Borets_Axiom_2 = 37,

        /// <summary>
        /// ESP_Dover_Smarten_IAM
        /// </summary>
        ESP_Dover_Smarten_IAM = 38,

        /// <summary>
        /// Original Modbus Implmentation (Minimum EFM POCType)
        /// </summary>
        EFM_Total_Flow = 41,

        /// <summary>
        /// Uses Totalflow Communiations Interface
        /// </summary>
        EFM_Totalflow_TCI = 43,

        /// <summary>
        /// Totalflow Master Device
        /// </summary>
        EFM_Totalflow_Master = 44,

        /// <summary>
        /// Uses Fisher ROC Protocol
        /// </summary>
        EFM_FisherROC = 45,

        /// <summary>
        /// Uses Fisher ROC Plus Protocol
        /// </summary>
        EFM_FisherROC_Master = 46,

        /// <summary>
        /// Maximum Reserved EFM POCType
        /// </summary>
        EFM_MaxReservedPOCType = 50,

        /// <summary>
        /// ProdMeter_Virtual
        /// </summary>
        ProdMeter_Virtual = 51,

        /// <summary>
        /// PCP_Unico
        /// </summary>
        PCP_Unico = 61,

        /// <summary>
        /// PCP_Lufkin_SAM
        /// </summary>
        PCP_Lufkin_SAM = 62,

        /// <summary>
        /// PCP_Wfd_ePacII
        /// </summary>
        PCP_Wfd_ePacII = 63,

        /// <summary>
        /// PCP_PMC_SALT
        /// </summary>
        PCP_PMC_SALT = 64,

        /// <summary>
        /// PCP_Kudu_Well_Manager
        /// </summary>
        PCP_Kudu_Well_Manager = 65,

        /// <summary>
        /// PCP_ePacII_Yaskagawa_F7
        /// </summary>
        PCP_ePacII_Yaskagawa_F7 = 66,

        /// <summary>
        /// PCP_SMARTEN
        /// </summary>
        PCP_SMARTEN = 67,

        /// <summary>
        /// PCS_Ferguson_AutoLift_Master
        /// </summary>
        PCS_Ferguson_AutoLift_Master = 71,

        /// <summary>
        /// PCS_Ferguson_AutoLift_Well
        /// </summary>
        PCS_Ferguson_AutoLift_Well = 72,

        /// <summary>
        /// PCS_Ferguson_MWM_Master
        /// </summary>
        PCS_Ferguson_MWM_Master = 73,

        /// <summary>
        /// PCS_Ferguson_MWM_Well
        /// </summary>
        PCS_Ferguson_MWM_Well = 74,

        /// <summary>
        /// PL_Lufkin_ILS_Lift_Manager
        /// </summary>
        PL_Lufkin_ILS_Lift_Manager = 75,

        /// <summary>
        /// PL_IPS_Plunger_4
        /// </summary>
        PL_IPS_Plunger_4 = 76,

        /// <summary>
        /// PCS_Ferguson_4000_Single_Well
        /// </summary>
        PCS_Ferguson_4000_Single_Well = 77,

        /// <summary>
        /// PL_OKC_Pump_Mate
        /// </summary>
        PL_OKC_Pump_Mate = 78,

        /// <summary>
        /// PlungerLift_FB_ACiC
        /// </summary>
        PlungerLift_FB_ACiC = 79,

        /// <summary>
        /// PlungerLift_FB_RTU5000
        /// </summary>
        PlungerLift_FB_RTU5000 = 80,

        /// <summary>
        /// INJ_Lufkin_SAM
        /// </summary>
        INJ_Lufkin_SAM = 81,

        /// <summary>
        /// INJ_eP
        /// </summary>
        INJ_eP = 82,

        /// <summary>
        /// INJ_AE
        /// </summary>
        INJ_AE = 83,

        /// <summary>
        /// INJ_Lufkin_MPC
        /// </summary>
        INJ_Lufkin_MPC = 84,

        /// <summary>
        /// INJ_Virtual
        /// </summary>
        INJ_Virtual = 85,

        /// <summary>
        /// INJ_AE350
        /// </summary>
        INJ_AE350 = 86,

        /// <summary>
        /// PCS_Ferguson_8000_Single_Well
        /// </summary>
        PCS_Ferguson_8000_Single_Well = 87,

        /// <summary>
        /// Facility_Modbus
        /// </summary>
        Facility_Modbus = 101,

        /// <summary>
        /// Facility_DDE
        /// </summary>
        Facility_DDE = 102,

        /// <summary>
        /// Facility_EDI
        /// </summary>
        Facility_EDI = 103,

        /// <summary>
        /// Facility_AB_DF1_SLC
        /// </summary>
        Facility_AB_DF1_SLC = 104,

        /// <summary>
        /// Facility_Virtual
        /// </summary>
        Facility_Virtual = 105,

        /// <summary>
        /// Facility_OPC
        /// </summary>
        Facility_OPC = 106,

        /// <summary>
        /// Totalflow Device w/ G4 protocol but not no slaves automatically created
        /// </summary>
        Facility_Totalflow_G4 = 107,

        /// <summary>
        /// Fisher ROC Device w/ ROCLink Plus protocol but no slaves automatically created
        /// </summary>
        Facility_Fisher_ROC800 = 108,

        /// <summary>
        /// Totalflow Device w/ G3 protocol but no tblParameters pre-existing
        /// </summary>
        Facility_Totalflow_G3 = 109,

        /// <summary>
        /// Fisher ROC Device w/ ROCLink protocol but no tblParameters pre-existing
        /// </summary>
        Facility_Fisher_ROCLink = 110,

        /// <summary>
        /// Allen-Bradley w/Net.Logix or Net.ABLink driver
        /// </summary>
        Facility_AB_Logix_ABLink = 111,

        /// <summary>
        /// PCS_Ferguson_GLM_Master
        /// </summary>
        PCS_Ferguson_GLM_Master = 218,

        /// <summary>
        /// PCS_Ferguson_GLM_Well
        /// </summary>
        PCS_Ferguson_GLM_Well = 219,

        /// <summary>
        /// PCS_Ferguson_Gas_Lift
        /// </summary>
        PCS_Ferguson_Gas_Lift = 220,

        /// <summary>
        /// First of reserverd POCTypes for Fisher ROC Plus Slave Devices
        /// </summary>
        EFM_FisherROC_SlavePOCType_First = 281,

        /// <summary>
        /// EFM_FisherROC_GasOrifice
        /// </summary>
        EFM_FisherROC_GasOrifice = 281,

        /// <summary>
        /// EFM_FisherROC_GasTurbine
        /// </summary>
        EFM_FisherROC_GasTurbine = 282,

        /// <summary>
        /// EFM_FisherROC_LiquidTurbine
        /// </summary>
        EFM_FisherROC_LiquidTurbine = 283,

        /// <summary>
        /// Last of reserved POCTypes for Fisher ROC Slave Devices
        /// </summary>
        EFM_FisherROC_SlavePOCType_Last = 300,

        /// <summary>
        /// First of reserverd POCTypes for Totalflow Slave Devices
        /// </summary>
        TF_SlavePOCType_First = 301,

        /// <summary>
        /// TF LevelMaster Tank (Levelmaster App 15)
        /// </summary>
        TF_LevelMaster_Tank = 301,

        /// <summary>
        /// TF Gas Lift Well (Gas Lift App 68)
        /// </summary>
        TF_GasLift_Well = 302,

        /// <summary>
        /// TF Orifice Gas Meter (AGA3 Tube App 4)
        /// </summary>
        TF_Orifice_Gas_Meter = 303,

        /// <summary>
        /// TF Plunger Control Well (Plunger Lift Control App 56)
        /// </summary>
        TF_Plunger_Control_Well = 304,

        /// <summary>
        /// TF V-Cone Gas Meter (V-Cone Tube App 23)
        /// </summary>
        TF_VCone_Gas_Meter = 305,

        /// <summary>
        /// TF Corolis Gas Meter (Coriolis Tube App 55)
        /// </summary>
        TF_Coriolis_Gas_Meter = 306,

        /// <summary>
        /// TF PID Controller Valve (PID Controller App 63)
        /// </summary>
        TF_PID_Controller = 307,

        /// <summary>
        /// TF Liquid Tube (Liquid Tube App 6)
        /// </summary>
        TF_Liquid_Tube = 308,

        /// <summary>
        /// TF Turbine Gas Meter (Enron AGA7 Pulse Input Tube App 28)
        /// </summary>
        TF_Turbine_Gas_Meter = 309,

        /// <summary>
        /// TF Pulse Accumulator (Pulse Accumulator 32)
        /// </summary>
        TF_Pulse_Accumulator = 310,

        /// <summary>
        /// TF Valve Control (Valve Control App 9)
        /// </summary>
        TF_Valve_Control = 311,

        /// <summary>
        /// TF Well Test (?)
        /// </summary>
        TF_Well_Test = 312,

        /// <summary>
        /// TF TFIO Module (TFIO Interface App 101)
        /// </summary>
        TF_TFIO_Module = 313,

        /// <summary>
        /// TF Oil Custody Transfer (Oil custody Transfer App 37)
        /// </summary>
        TF_Oil_Custody_Transfer = 314,

        /// <summary>
        /// TF LACT Test (IEC Interface App 13 at AppSlot 91)
        /// </summary>
        TF_LACT_Test = 315,

        /// <summary>
        /// TF Operations (Operations App 18)
        /// </summary>
        TF_Operations = 316,

        /// <summary>
        /// TF Therms Master (Therms Master App 11)
        /// </summary>
        TF_Therms_Master = 317,

        /// <summary>
        /// TF Therms Slave (Therms Slave App 14)
        /// </summary>
        TF_Therms_Slave = 318,

        /// <summary>
        /// TF PAD Controller (PAD Scheduler App 62)
        /// </summary>
        TF_PAD_Controller = 319,

        /// <summary>
        /// TF Pump Controller (Pump Interface App 16)
        /// </summary>
        TF_Pump_Controller = 320,

        /// <summary>
        /// TF G4 XFC IO Module (IOS App 1 for XFC)
        /// </summary>
        TF_XFC_IO_Module = 321,

        /// <summary>
        /// TF G4 XRC IO MOdule (IOS App 1 for XRC)
        /// </summary>
        TF_XRC_IO_Module = 322,

        /// <summary>
        /// TF G4 Shutdown Controller (Safety App 59)
        /// </summary>
        TF_Shutdown_Controller = 323,

        /// <summary>
        /// TF Holding Registers (Holding Registers App 10)
        /// </summary>
        TF_Holding_Registers = 324,

        /// <summary>
        /// IOS_Interface_XSeries
        /// </summary>
        IOS_Interface_XSeries = 325,

        /// <summary>
        /// Last of reserved POCTypes for Totalflow Slave Devices
        /// </summary>
        TF_SlavePOCType_Last = 350,

        /// <summary>
        /// Chem_Pump_iChem_Revolution
        /// </summary>
        Chem_Pump_iChem_Revolution = 390,

        /// <summary>
        /// Wellmark_DigiUltra
        /// </summary>
        Wellmark_DigiUltra = 391,

        /// <summary>
        /// RPC_Bright
        /// </summary>
        RPC_Bright = 401,

        /// <summary>
        /// RPC_Unico_LRP
        /// </summary>
        RPC_Unico_LRP = 402,

        /// <summary>
        /// RPC_Schneider
        /// </summary>
        RPC_Schneider = 403,

        /// <summary>
        /// RPC_Naftamatika_WellSim
        /// </summary>
        RPC_Naftamatika_WellSim = 404,

        /// <summary>
        /// RPC_Rockwell_OptiLift
        /// </summary>
        RPC_Rockwell_OptiLift = 405,

        /// <summary>
        /// RPC_BEPOC
        /// </summary>
        RPC_BEPOC = 408,

        /// <summary>
        /// RPC_WellLynx
        /// </summary>
        RPC_WellLynx = 415,

        /// <summary>
        /// RPC_Kudu_HPU_A
        /// </summary>
        RPC_Kudu_HPU_A = 416,

        /// <summary>
        /// RPC_Kudu_HPU_B
        /// </summary>
        RPC_Kudu_HPU_B = 417,

        /// <summary>
        /// RPC_Spoc_Rev
        /// </summary>
        RPC_Spoc_Rev = 419,

        /// <summary>
        /// RPC_WellWorx
        /// </summary>
        RPC_WellWorx = 420,

    }
}
