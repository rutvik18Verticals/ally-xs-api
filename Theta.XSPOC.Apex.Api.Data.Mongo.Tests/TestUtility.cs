using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.RodPump;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Camera;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Ports;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Strings;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using MongoLookup = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    public class TestUtility
    {
        public static IAsyncCursor<T> GetMockMongoData<T>(string collectionName, int lookupType = 0)
        {
            var result = new Mock<IAsyncCursor<T>>();
            switch (collectionName)
            {
                case "Lookup":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetLookupData(lookupType));
                    break;
                case "Asset":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetAssetModelData());
                    break;
                case "MasterVariables":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetMasterVariablesData());
                    break;
                case "Port":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetPortData());
                    break;
                case "Customers":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetCustomerData());
                    break;
                case "HostAlarm":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetHostAlarm());
                    break;
                case "Route":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetRouteData());
                    break;
                case "RTUAlarm":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetRTUAlarm());
                    break;
                case "FacilityTagAlarm":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetFacilityTagAlarm());
                    break;
                case "Notification":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetNotification());
                    break;
                case "Cameras":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetCameras());
                    break;
                case "CameraAlarms":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetCameraAlarms());
                    break;
                case "AlarmEvents":
                    result.Setup(m => m.Current).Returns((IEnumerable<T>)GetAlarmEvents());
                    break;
                default:
                    break;
            }
            result.SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

            return result.Object;
        }

        private static IList<MongoLookup.Lookup> GetLookupData(int lookupType)
        {
            var lookupData = new List<Lookup>()
            {
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "DataTypeId", "1" },
                    },
                    LookupType = LookupTypes.DataTypes.ToString(),
                    LookupDocument = new MongoLookup.DataTypes()
                    {
                        Comment = "Comment",
                        DataTypeId = 1,
                        Description = "Description",
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "DataTypeId", "1" },
                    },
                    LookupType = LookupTypes.DataTypes.ToString(),
                    LookupDocument = new MongoLookup.DataTypes()
                    {
                        Comment = "Comment 2",
                        DataTypeId = 2,
                        Description = "Description 2",
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCTypesId", "1" },
                    },
                    LookupType = LookupTypes.POCTypes.ToString(),
                    LookupDocument = new MongoLookup.POCTypes()
                    {
                        POCType = 1,
                        Description = "RPC Baker/Weatherford"
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCTypesId", "8" },
                    },
                    LookupType = LookupTypes.POCTypes.ToString(),
                    LookupDocument = new MongoLookup.POCTypes()
                    {
                        POCType = 8,
                        Description = "RPC Lufkin SAM"
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "PumpingUnitManufacturerId", "1" },
                    },
                    LookupType = LookupTypes.PumpingUnitManufacturer.ToString(),
                    LookupDocument = new PumpingUnitManufacturer()
                    {
                        Abbrev = "CS",
                        Manuf = "A. SMACO Conventional",
                        UnitTypeId = 1,
                        RequiredRotation = 0,
                        SortByName = false,
                        ShowNames = false
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "UnitId", "AC1" },
                    },
                    LookupType = LookupTypes.PumpingUnit.ToString(),
                    LookupDocument = new MongoLookup.PumpingUnit()
                    {
                        APIDesignation = "A-320-270-86",
                        UnitId = "AC1",
                        ManufId = "AC",
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "UnitId", "AC12" },
                    },
                    LookupType = LookupTypes.PumpingUnit.ToString(),
                    LookupDocument = new MongoLookup.PumpingUnit()
                    {
                        APIDesignation = "A-320-270-100",
                        UnitId = "AC12",
                        ManufId = "AC",
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "ManufacturerId", "1" },
                    },
                    LookupType = LookupTypes.GLManufacturers.ToString(),
                    LookupDocument = new MongoLookup.GLManufacturers()
                    {
                        ManufacturerId = 1,
                        Manufacturer = "PCS",
                        Locked = false,
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "PhraseId", "20082" },
                    },
                    LookupType = LookupTypes.LocalePhrases.ToString(),
                    LookupDocument = new MongoLookup.LocalePhrases()
                    {
                        PhraseId = 20082,
                        English = "Plunger Runtimes"
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "PhraseId", "20083" },
                    },
                    LookupType = LookupTypes.LocalePhrases.ToString(),
                    LookupDocument = new MongoLookup.LocalePhrases()
                    {
                        PhraseId = 20083,
                        English = "Total On"
                    }
                },
                new() {
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "PhraseId", "57" },
                    },
                    LookupType = LookupTypes.LocalePhrases.ToString(),
                    LookupDocument = new MongoLookup.LocalePhrases()
                    {
                        PhraseId = 57,
                        English = "Idle, Fault Low Freq"
                    }
                },
                new() {
                    LookupType = LookupTypes.POCTypeAction.ToString(),
                    LookupDocument = new MongoLookup.POCTypeAction()
                    {
                        POCType = 8,
                        ControlActionId = 1
                    }
                },
                new() {
                    LookupType = LookupTypes.POCTypeAction.ToString(),
                    LookupDocument = new MongoLookup.POCTypeAction()
                    {
                        POCType = 8,
                        ControlActionId = 2
                    }
                },
                new() {
                    LookupType = LookupTypes.ControlActions.ToString(),
                    LookupDocument = new MongoLookup.ControlActions()
                    {
                        ControlActionId = 1,
                        Description = "Start"
                    }
                },
                new() {
                    LookupType = LookupTypes.ControlActions.ToString(),
                    LookupDocument = new MongoLookup.ControlActions()
                    {
                        ControlActionId = 2,
                        Description = "Stop"
                    }
                },
                new() {
                    LookupType = LookupTypes.States.ToString(),
                    LookupDocument = new MongoLookup.States()
                    {
                        StatesId = 308,
                        Value = 57,
                        Text = "Idle, Fault Low Freq",
                        PhraseId = 20082
                    }
                },
                new() {
                    LookupType = LookupTypes.States.ToString(),
                    LookupDocument = new MongoLookup.States()
                    {
                        StatesId = 308,
                        Value = 58,
                        Text = "Running, Hydraulic Lift PID",
                        PhraseId = null
                    }
                },
                new() {
                    LookupType = LookupTypes.RodGrade.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "RodGradeId", "1" }
                    },
                    LookupDocument = new RodGrade()
                    {
                        Name = "D",
                        RodGradeId = 1,
                    }
                },new() {
                    LookupType = LookupTypes.RodGrade.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "RodGradeId", "1" }
                    },
                    LookupDocument = new RodGrade()
                    {
                        Name = "D",
                        RodGradeId = 1,
                    }
                },new() {
                    LookupType = LookupTypes.States.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "StatesId", "8" },
                        { "Value", "0" }
                    },
                    LookupDocument = new States()
                    {
                        StatesId = 8,
                        Value = 0,
                        Text = "Disabled",
                        PhraseId = 356
                    }
                },new() {
                    LookupType = LookupTypes.States.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "StatesId", "8" },
                        { "Value", "1" }
                    },
                    LookupDocument = new States()
                    {
                        StatesId = 8,
                        Value = 1,
                        Text = "Enabled",
                        PhraseId = 355
                    }
                },new() {
                    LookupType = LookupTypes.XDiagOutputs.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "XDiagOutputsId", "1" }
                    },
                    LookupDocument = new XDiagOutputs()
                    {
                        XDiagOutputsId = 1,
                        Name = "GrossPumpStroke",
                        PhraseId = 587
                    }
                },new() {
                    LookupType = LookupTypes.CameraAlarmTypes.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "CameraAlarmTypesId", "1" }
                    },
                    LookupDocument = new CameraAlarmTypes()
                    {
                        Name = "Image Analysis",
                        PhraseId = 20082
                    }
                },new() {
                    LookupType = LookupTypes.CameraAlarmTypes.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "CameraAlarmTypesId", "2" }
                    },
                    LookupDocument = new CameraAlarmTypes()
                    {
                        Name = "Environment",
                        PhraseId = 6020
                    }
                },new() {
                    LookupType = LookupTypes.FacilityTagGroups.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "FacilityTagGroupsId", "1" }
                    },
                    LookupDocument = new FacilityTagGroups()
                    {
                        Name = "Tag XS-1353",
                        DisplayOrder = 0
                    }
                },new() {
                    LookupType = LookupTypes.FacilityTagGroups.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "FacilityTagGroupsId", "2" }
                    },
                    LookupDocument = new FacilityTagGroups()
                    {
                        Name = "tanks",
                        DisplayOrder = 0
                    }
                },new() {
                    LookupType = LookupTypes.ParamStandardTypes.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "ParamStandardTypesId", "1" }
                    },
                    LookupDocument = new ParamStandardTypes()
                    {
                        ParamStandardType = 1,
                        Description = "Pump Intake Pressure (Measured)",
                        PhraseId = 0,
                        UnitTypeId = 3
                    }
                },new() {
                    LookupType = LookupTypes.ParamStandardTypes.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "ParamStandardTypesId", "2" }
                    },
                    LookupDocument = new ParamStandardTypes()
                    {
                        ParamStandardType = 2,
                        Description = "Frequency",
                        PhraseId = 0,
                        UnitTypeId = 14
                    }
                },new() {
                    LookupType = LookupTypes.States.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "StatesId", "731" },
                        { "Value", "10" }
                    },
                    LookupDocument = new States()
                    {
                        StatesId = 731,
                        Value = 10,
                        Text = "Foss and Gaul Method"
                    }
                },new() {
                    LookupType = LookupTypes.States.ToString(),
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "StatesId", "4054" },
                        { "Value", "32874" }
                    },
                    LookupDocument = new States()
                    {
                        StatesId = 4054,
                        Value = 32874,
                        Text = "DI Process 1 Alarm - Set"
                    }
                }
            };
            LookupTypes lt = (LookupTypes)lookupType;

            return lookupData.Where(l => l.LookupType == lt.ToString()).ToList();
        }

        private static IList<MongoAssetCollection.Asset> GetAssetModelData()
        {
            return new List<MongoAssetCollection.Asset>()
            {
                new() {
                    Name = "001 DigiUltra",
                    ArtificialLiftType = "RodLift",
                    CustomerId = "66d74f86c66365161d7ca943",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "NodeId", "001 DigiUltra" },
                        { "AssetGUID", "61e72096-72d4-4878-afb7-f042e0a30118" }
                    },
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                    },
                    AssetConfig = new AssetConfig()
                    {
                        RunStatus = "Running",
                        TimeInState = 6,
                        TodayCycles = 4,
                        TodayRuntime = 5,
                        InferredProduction = 4,
                        IsEnabled = true,
                        PortId = "1",
                    },
                    AssetDetails = new RodPumpDetail()
                    {
                        PumpingUnit = new Lookup()
                        {
                            LookupType = LookupTypes.PumpingUnit.ToString(),
                            LegacyId = new Dictionary<string, string>()
                            {
                                { "UnitId", "AC1" }
                            },
                            LookupDocument = new PumpingUnit()
                            {
                                 APIDesignation = "A-320-270-100",
                                 UnitId = "AC1",
                                 ManufId = "AC",
                            }
                        },
                        Rods = new List<Rod>()
                        {
                            new()
                            {
                                RodNumber = 1,
                                RodGrade = new Lookup()
                                {
                                    LookupType = LookupTypes.RodGrade.ToString(),
                                    LegacyId = new Dictionary<string, string>()
                                    {
                                        { "RodGradeId", "1" }
                                    },
                                    LookupDocument = new RodGrade()
                                    {
                                        Name = "D",
                                        RodGradeId = 1,
                                    }
                                }
                            },
                            new()
                            {
                                RodNumber = 2,
                                RodGrade = new Lookup()
                                {
                                    LookupType = LookupTypes.RodGrade.ToString(),
                                    LegacyId = new Dictionary<string, string>()
                                    {
                                        { "RodGradeId", "2" }
                                    },
                                    LookupDocument = new RodGrade()
                                    {
                                        Name = "D",
                                        RodGradeId = 2,
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private static IList<Parameters> GetMasterVariablesData()
        {
            return new List<Parameters>()
            {
                new(){
                    Name = "SPM",
                    Address = 1001,
                    Description = "SPM",
                    ChannelId = "C1",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new MongoLookup.DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "1001" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new MongoLookup.UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new(){
                    Name = "Comm Pct",
                    Address = 1002,
                    Description = "Comm Pct",
                    ChannelId = "C2",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "2" }
                        },
                        LookupDocument = new MongoLookup.DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 2,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "1002" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new MongoLookup.UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new(){
                    Name = "Pump fillage",
                    Address = 1003,
                    Description = "SPM",
                    ChannelId = "C3",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "3" }
                        },
                        LookupDocument = new MongoLookup.DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 3,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "1003" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new MongoLookup.UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new(){
                    Name = "",
                    Address = 10001,
                    Description = "Tank #1 Low Alarm",
                    ChannelId = "C1006",
                    State = 8,
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new MongoLookup.DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "10001" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new MongoLookup.UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                }
            };
        }

        private static IEnumerable<Ports> GetPortData()
        {
            return new List<Ports>()
            {
                new()
                {
                   PortID = 0,
                   ServerName = "ng-hosting",
                   CommPort = 0,
                   Enabled = true,
                   BaudRate = 0,
                   PortType = 6,
                },
                new()
                {
                   PortID = 1,
                   ServerName = "hosting",
                   CommPort = 0,
                   Enabled = true,
                   BaudRate = 0,
                   PortType = 1,
                }
            };
        }

        private static IEnumerable<Customer> GetCustomerData()
        {
            return new List<Customer>()
            {
                new()
                {
                    Name = "Customer 1",
                    LegacyId =  new Dictionary<string, string>()
                    {
                        { "CustomerGUID", "a5fa13b2-56e8-4aaa-a2d5-61375609de9e" },
                        { "CustomerId", "1"}
                    }
                },
            };
        }

        private static IEnumerable<object> GetRouteData()
        {
            return new List<Route>()
            {
                new()
                {
                    StringID = 66,
                    StringName = "PCS",
                    ContactListID = 84,
                    ResponderListID = null,
                },
                new()
                {
                    StringID = 67,
                    StringName = "Theta",
                    ContactListID = null,
                    ResponderListID = 41,
                },
                new()
                {
                    StringID = 4,
                    StringName = "Kindersley",
                    ContactListID = null,
                    ResponderListID = null,
                }
            };
        }

        private static IEnumerable<object> GetHostAlarm()
        {
            return new List<AlarmConfiguration>()
            {
                new()
                {
                    Register=2005,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "Address", "2005" },
                        { "AlarmType", "2" },
                        { "NodeId", "001 DigiUltra" },
                        { "ID", "2" }
                    },
                    LoLimit = 23.8,
                    HiLimit = 24.2,
                    AlarmType = "HostAlarm",
                    Document = new HostAlarm()
                    {
                        HostAlarmTypeId = 2,
                        LoLoLimit = 23.8,
                        HiHiLimit = 24.2,
                        AlarmState = 3,
                    }
                },

                new()
                {
                    Register=2008,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "Address", "2008" },
                        { "AlarmType", "2" },
                        { "NodeId", "001 DigiUltra" },
                        { "ID", "4" }
                    },
                    LoLimit = 21.8,
                    HiLimit = 22.2,
                    AlarmType = "HostAlarm",
                    Document = new HostAlarm()
                    {
                        HostAlarmTypeId = 2,
                        LoLoLimit = 23.8,
                        HiHiLimit = 24.2,
                        AlarmState = 3,
                    }
                }
            };
        }

        private static IEnumerable<object> GetRTUAlarm()
        {
            return new List<AlarmConfiguration>()
            {
                new()
                {
                    Register=10001,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "Register", "10001" },
                        { "POCType", "8" },
                        { "Bit", "0" }
                    },
                    LoLimit = 0,
                    HiLimit = 0,
                    AlarmType = "RTUAlarm",
                    Document = new RTU()
                    {
                        CalloutEnabled = false,
                        NormalState = false,
                        Locked = false
                    }
                },

                new()
                {
                    Register=10003,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "Register", "10003" },
                        { "POCType", "8" },
                        { "Bit", "0" }
                    },
                    LoLimit = 0,
                    HiLimit = 0,
                    AlarmType = "RTUAlarm",
                    Document = new RTU()
                    {
                        CalloutEnabled = false,
                        NormalState = false,
                        Locked = false
                    }
                }
            };
        }

        private static IEnumerable<object> GetFacilityTagAlarm()
        {
            return new List<AlarmConfiguration>()
            {
                new()
                {
                    Register=1830,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "Address", "10001" },
                        { "NodeId", "001 DigiUltra" },
                        { "Bit", "0" }
                    },
                    LoLimit = 0,
                    HiLimit = 0,
                    AlarmType = "FacilityTagAlarm",
                    Document = new FacilityTagAlarm()
                    {
                        AlarmState = 2,
                        GroupNodeId = "001 DigiUltra",
                        NodeId = "66d74ffac66365161d7d8f11"
                    }
                },
                new()
                {
                    Register=1510370,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "Address", "1510370" },
                        { "NodeId", "001 DigiUltra" },
                        { "Bit", "0" }
                    },
                    LoLimit = 0,
                    HiLimit = 0,
                    AlarmType = "FacilityTagAlarm",
                    Document = new FacilityTagAlarm()
                    {
                        AlarmState = 0,
                        GroupNodeId = "001 DigiUltra",
                        NodeId = "66d74ffac66365161d7d8f1d"
                    }
                }
            };
        }

        private static IEnumerable<object> GetNotification()
        {
            return new List<Notification>()
            {
                new()
                {
                    AlaramId="66d74fffc66365161d7d9d2b",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "HostAlarmId", "1583" },
                        { "NodeId", "001 DigiUltra" }
                    }
                },
                new()
                {
                    AlaramId="66d74fffc66365161d7d9d2b",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "HostAlarmId", "1621" },
                        { "NodeId", "001 DigiUltra" }
                    }
                }
            };
        }

        private static IEnumerable<object> GetCameras()
        {
            return new List<Camera>()
            {
                new()
                {
                    Name = "Camera 1 nowmobotix",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "CameraId", "01" },
                        { "NodeId", "001 DigiUltra" }
                    },
                    CameraType = new Lookup()
                    {
                        LookupType = CameraTypesEnum.CameraTypes.ToString(),
                        LookupDocument = new CameraTypes()
                        {
                            Name = "Mobotix",
                            PhraseId = 7303
                        }
                    }
                },
                new()
                {
                    Name = "Camera 2 nowmobotix",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "CameraId", "02" },
                        { "NodeId", "001 DigiUltra" }
                    },
                    CameraType = new Lookup()
                    {
                        LookupType = CameraTypesEnum.CameraTypes.ToString(),
                        LookupDocument = new CameraTypes()
                        {
                            Name = "Mobotix",
                            PhraseId = 7303
                        }
                    }
                }
            };
        }

        private static IEnumerable<object> GetCameraAlarms()
        {
            return new List<CameraAlarms>()
            {
                new()
                {
                    CameraID = 01,
                    AlarmType = 1,
                    Enabled = 1,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "CameraAlarmID", "6" }
                    }
                },
                new()
                {
                    CameraID = 02,
                    AlarmType = 2,
                    Enabled = 1,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "CameraAlarmID", "7" }
                    }
                }
            };
        }

        private static IEnumerable<object> GetAlarmEvents()
        {
            return new List<AlarmEvents>()
            {
                new()
                {
                    AlarmID = 6,
                    EventType = 2,
                    AlarmType = 1,
                    Value = 2,
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "AlarmEventsId", "2" }
                    }
                }
            };
        }
    }
}
