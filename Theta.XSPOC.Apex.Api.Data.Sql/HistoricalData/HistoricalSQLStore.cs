using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.HistoricalData;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.UnitConversion;

namespace Theta.XSPOC.Apex.Api.Data.Sql.HistoricalData
{
    /// <summary>
    /// This is the SQL implementation that defines the methods for the historical store.
    /// </summary>
    public class HistoricalSQLStore : IHistoricalStore
    {

        #region Private Fields

        private readonly IThetaDbContextFactory<XspocDbContext> _thetaDbContextFactory;
        private readonly IUnitConversion _unitConversion;
        private readonly IConfiguration _configuration;
        private readonly ICurrentRawScansStore _currentRawScansStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="HistoricalSQLStore"/> using the provided
        /// <paramref name="thetaDbContextFactory"/>.
        /// </summary>
        /// <param name="thetaDbContextFactory">The theta db context factory used to get a db context.</param>
        /// <param name="unitConversion">The unit conversion library.</param>
        /// <param name="currentRawScansStore"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="thetaDbContextFactory"/> is null
        /// or
        /// when <paramref name="unitConversion"/> is null.
        /// </exception>
        public HistoricalSQLStore(IThetaDbContextFactory<XspocDbContext> thetaDbContextFactory,
            IUnitConversion unitConversion, ICurrentRawScansStore currentRawScansStore, IConfiguration configuration)
        {
            _thetaDbContextFactory =
                thetaDbContextFactory ?? throw new ArgumentNullException(nameof(thetaDbContextFactory));
            _unitConversion = unitConversion ?? throw new ArgumentNullException(nameof(unitConversion));
            _currentRawScansStore = currentRawScansStore ?? throw new ArgumentNullException(nameof(currentRawScansStore));
            _configuration = configuration;
        }

        #endregion

        #region IHistoricalRepository Implementation

        /// <summary>
        /// This method will retrieve the most recent historical data for the defined registers for the provided
        /// <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to retrieve the data.</param>
        /// <param name="customerId">This is not used in the SQL implementation.</param>
        /// <returns>
        /// A <seealso cref="IList{RegisterData}"/> that represents the most recent historical data for the defined
        /// registers for the provided <paramref name="assetId"/>. An empty list is returned when no data is found. If
        /// assetId is the default GUID then null is returned.
        /// </returns>
        public async Task<IList<RegisterData>> GetAssetStatusRegisterDataAsync(Guid assetId, Guid customerId)
        {
            bool isInfluxEnabled = _configuration.GetValue("EnableInflux", false);
            if (isInfluxEnabled)
            {
                return await GetAssetStatusRegisterDataUsingMongoInflux(assetId, customerId);
            }
            else
            {
                return await GetAssetStatusRegisterDataUsingSQL(assetId);
            }
        }

        /// <summary>
        /// This method will retrieve the most recent param standard data for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to retrieve the data.</param>
        /// <param name="customerId">The customer id used to verify customer has access to the data.</param>
        /// <returns>
        /// A <seealso cref="IList{ParamStandardData}"/> that represents the most recent historical data
        /// for the param standard data for the provided <paramref name="assetId"/>.
        /// </returns>
        public async Task<IList<ParamStandardData>> GetParamStandardDataAsync(Guid assetId, Guid customerId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }
#pragma warning disable IDE0029
            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var node = await context.NodeMasters.Select(m => new
                {
                    m.AssetGuid,
                    m.NodeId,
                    m.PocType,
                }).FirstOrDefaultAsync(m => m.AssetGuid == assetId);

                if (node == null)
                {
                    return new List<ParamStandardData>();
                }

                var facilityTagData = context.NodeMasters.Join(context.FacilityTags, l => new
                {
                    Key1 = l.NodeId,
                }, r => new
                {
                    Key1 = r.GroupNodeId == null ? r.NodeId : r.GroupNodeId,
                }, (node, facilityTag) => new
                {
                    node,
                    facilityTag,
                }).Where(m => m.node.AssetGuid == assetId)
                    .Select(m => new ParamStandardRecordData()
                    {
                        NodeId = m.facilityTag.NodeId,
                        Address = m.facilityTag.Address,
                        Value = m.facilityTag.CurrentValue,
                        ParamStandardType = m.facilityTag.ParamStandardType,
                        Decimals = m.facilityTag.Decimals,
                        StringValue = null,
                        DataType = m.facilityTag.DataType,
                        UnitTypeId = m.facilityTag.UnitType,
                    }).ToList();

                var paramStandardData = context.Parameters.Join(context.NodeMasters, l => new
                {
                    Key1 = node.NodeId,
                    Key2 = l.Poctype == 99 ? node.PocType : l.Poctype,
                }, r => new
                {
                    Key1 = r.NodeId,
                    Key2 = (int)r.PocType,
                }, (left, right) => new
                {
                    left,
                    right,
                }).GroupJoin(context.CurrentRawScans, l => new
                {
                    Key1 = l.right.NodeId,
                    Key2 = l.left.Address,
                }, r => new
                {
                    Key1 = r.NodeId,
                    Key2 = r.Address,
                }, (left, right) => new
                {
                    left,
                    right,
                })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.FacilityTags, l => new
                    {
                        Key1 = node.NodeId,
                        Key2 = l.left.left.left.ParamStandardType,
                    }, r => new
                    {
                        Key1 = r.NodeId,
                        Key2 = r.ParamStandardType,
                    }, (left, right) => new
                    {
                        left,
                        right,
                    })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.CurrentRawScans, l => new
                    {
                        Key1 = node.NodeId,
                        Key2 = l.right.Address,
                    }, r => new
                    {
                        Key1 = r.NodeId,
                        Key2 = r.Address,
                    }, (left, right) => new
                    {
                        left,
                        right,
                    })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    }).Where(m =>
                        m.left.left.left.left.left.left.right.NodeId == node.NodeId &&
                        m.left.left.left.left.left.left.left.ParamStandardType != null)
                    .Select(m => new ParamStandardRecordData()
                    {
                        NodeId = m.left.left.left.left.left.left.right.NodeId,
                        Address = m.left.left.right.ParamStandardType != null
                            ? m.left.left.right.Address
                            : m.left.left.left.left.left.left.left.Address,
                        Value = m.left.left.right.ParamStandardType != null
                            ? m.right.Value
                            : m.left.left.left.left.right.Value,
                        ParamStandardType = m.left.left.left.left.left.left.left.ParamStandardType,
                        Decimals = m.left.left.right.ParamStandardType != null
                            ? m.left.left.right.Decimals
                            : m.left.left.left.left.left.left.left.Decimals,
                        StringValue = m.left.left.right.ParamStandardType != null
                            ? m.right.StringValue
                            : m.left.left.left.left.right.StringValue,
                        DataType = m.left.left.left.left.left.left.left.DataType,
                        UnitTypeId = m.left.left.left.left.left.left.left.UnitType,
                    }).ToList();

                var result = facilityTagData.Union(paramStandardData).Select(m => new ParamStandardData()
                {
                    Address = m.Address,
                    DataType = m.DataType,
                    Decimals = m.Decimals,
                    NodeId = m.NodeId,
                    ParamStandardType = m.ParamStandardType,
                    StringValue = m.StringValue,
                    Value = m.Value == null ? null : _unitConversion.CreateUnitObject(m.UnitTypeId, m.Value.Value),
                    UnitTypeId = m.UnitTypeId,
                }).ToList();

                return result;
            }
#pragma warning restore IDE0029
        }

        #endregion

        #region Private Records

        /// <summary>
        /// Defines register data for the most recent received value.
        /// </summary>
        private record RegisterRecordData
        {

            /// <summary>
            /// Gets or sets the address.
            /// </summary>
            public int? Address { get; set; }

            /// <summary>
            /// Gets or sets the register description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the register data type.
            /// </summary>
            public string DataType { get; set; }

            /// <summary>
            /// Gets or sets the register string value.
            /// </summary>
            public string StringValue { get; set; }

            /// <summary>
            /// Gets or sets the register value.
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// Gets or sets the register format.
            /// </summary>
            public string Format { get; set; }

            /// <summary>
            /// Gets or sets the register display order.
            /// </summary>
            public int? Order { get; set; }

            /// <summary>
            /// Gets or sets the register units.
            /// </summary>
            public string Units { get; set; }

            /// <summary>
            /// Gets or sets the register bit location, this is leverage for facility tags only.
            /// </summary>
            public string Bit { get; set; }

            /// <summary>
            /// Gets or sets the register unit type.
            /// </summary>
            public int? UnitType { get; set; }

            /// <summary>
            /// Gets or sets the register phrase id.
            /// </summary>
            public string PhraseId { get; set; }

            /// <summary>
            /// Gets or sets the register state id for state lookup.
            /// </summary>
            public string StateId { get; set; }

            /// <summary>
            /// Gets or sets the register decimal places.
            /// </summary>
            public int Decimals { get; set; }

        }

        /// <summary>
        /// This is a record that represents the param standard data.
        /// </summary>
        private record ParamStandardRecordData
        {

            /// <summary>
            /// Gets or sets the node id.
            /// </summary>
            public string NodeId { get; set; }

            /// <summary>
            /// Gets or sets the address.
            /// </summary>
            public int Address { get; set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            public float? Value { get; set; }

            /// <summary>
            /// Gets or sets the param standard type.
            /// </summary>
            public int? ParamStandardType { get; set; }

            /// <summary>
            /// Gets or sets the decimals.
            /// </summary>
            public int? Decimals { get; set; }

            /// <summary>
            /// Gets or sets the string value.
            /// </summary>
            public string StringValue { get; set; }

            /// <summary>
            /// Gets or sets the data type.
            /// </summary>
            public int? DataType { get; set; }

            /// <summary>
            /// Gets or sets the unit type id.
            /// </summary>
            public int UnitTypeId { get; set; }

        }

        #endregion

        private async Task<IList<RegisterData>> GetAssetStatusRegisterDataUsingSQL(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
#pragma warning disable IDE0004, IDE0029
                var facilityTags = context.NodeMasters.Join(context.StatusRegisters, l => l.PocType, r => r.PocType,
                        (node, statusRegister) => new
                        {
                            node,
                            statusRegister,
                        })
                    .Join(context.FacilityTags, l => new
                    {
                        key1 = (int?)l.statusRegister.RegisterAddress,
                        key2 = l.node.NodeId,
                    },
                    r => new
                    {
                        key1 = (int?)r.Address,
                        key2 = r.NodeId,
                    }, (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.ParamStandardTypes, l => l.right.ParamStandardType, r => (int?)r.ParamStandardType,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.LocalePhrases, l => (int?)l.right.PhraseId, r => (int?)r.PhraseId,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.States, l => new
                    {
                        Key1 = l.left.left.left.left.right.StateId,
                        Key2 = l.left.left.left.left.right.CurrentValue,
                    }, r => new
                    {
                        Key1 = (int?)r.StateId,
                        Key2 = (float?)r.Value,
                    },
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .Where(m => m.left.left.left.left.left.left.left.node.AssetGuid == assetId)
                    .Select(m => new RegisterRecordData()
                    {
                        Address = m.left.left.left.left.left.left.left.statusRegister.RegisterAddress,
                        Description = m.left.left.left.left.left.left.right.ParamStandardType != null
                            ? m.left.left.left.left.right.Description
                            : m.left.left.left.left.left.left.right.Description,
                        DataType = m.left.left.left.left.left.left.right.DataType.HasValue
                            ? m.left.left.left.left.left.left.right.DataType.ToString()
                            : null,
                        Decimals = m.left.left.left.left.left.left.right.Decimals.Value,
                        StringValue = m.right.Text == null ? m.right.Text : "",
                        Value = m.left.left.left.left.left.left.right.CurrentValue.HasValue
                            ? m.left.left.left.left.left.left.right.CurrentValue.ToString()
                            : null,
                        Format = m.left.left.left.left.left.left.left.statusRegister.Format,
                        Order = m.left.left.left.left.left.left.left.statusRegister.Order,
                        Units = m.left.left.left.left.left.left.left.statusRegister.Units,
                        UnitType = m.left.left.left.left.left.left.right.ParamStandardType != null
                            ? m.left.left.left.left.right.UnitTypeId
                            : m.left.left.left.left.left.left.right.UnitType,
                        StateId = m.right.StateId.ToString(),
                    });

                var parameters = context.NodeMasters.Join(context.StatusRegisters, l => l.PocType, r => r.PocType,
                        (node, statusRegister) => new
                        {
                            node,
                            statusRegister,
                        }).Join(context.Parameters, l => new
                        {
                            Key1 = (int?)l.statusRegister.RegisterAddress,
                            Key2 = l.statusRegister.PocType,
                        },
                        r => new
                        {
                            Key1 = (int?)r.Address,
                            Key2 = r.Poctype,
                        }, (left, right) => new
                        {
                            left,
                            right,
                        })
                    .Join(context.CurrentRawScans, l => new
                    {
                        Key1 = (int?)l.left.statusRegister.RegisterAddress,
                        Key2 = l.left.node.NodeId,
                    }, r => new
                    {
                        Key1 = (int?)r.Address,
                        Key2 = r.NodeId,
                    }, (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.ParamStandardTypes, l => l.left.right.ParamStandardType,
                        r => (int?)r.ParamStandardType,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.LocalePhrases, l => (int?)l.right.PhraseId, r => (int?)r.PhraseId,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.LocalePhrases, l => (int?)l.left.left.left.left.left.right.PhraseId, r => (int?)r.PhraseId,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.States, l => new
                    {
                        Key1 = (int?)l.left.left.left.left.left.left.left.right.StateId,
                        Key2 = l.left.left.left.left.left.left.right.Value,
                    }, r => new
                    {
                        Key1 = (int?)r.StateId,
                        Key2 = (float?)r.Value,
                    },
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .Where(m => m.left.left.left.left.left.left.left.left.left.left.node.AssetGuid == assetId)
                    .Select(m => new RegisterRecordData()
                    {
                        Address = m.left.left.left.left.left.left.left.left.left.left.statusRegister.RegisterAddress,
                        Description = m.left.left.left.left.left.left.left.left.left.right.ParamStandardType != null
                            ? m.left.left.left.left.left.left.right.Description
                            : m.left.left.left.left.left.left.left.left.left.right.Description,
                        DataType = m.left.left.left.left.left.left.left.left.left.right.DataType.HasValue
                            ? m.left.left.left.left.left.left.left.left.left.right.DataType.ToString()
                            : null,
                        Decimals = m.left.left.left.left.left.left.left.left.left.right.Decimals,
                        StringValue = m.right.Text != null
                            ? m.right.Text
                            : m.left.left.left.left.left.left.left.left.right.StringValue != null
                                ? m.left.left.left.left.left.left.left.left.right.StringValue
                                : "",
                        Value = m.left.left.left.left.left.left.left.left.right.Value.HasValue
                            ? m.left.left.left.left.left.left.left.left.right.Value.ToString()
                            : null,
                        Format = m.left.left.left.left.left.left.left.left.left.left.statusRegister.Format,
                        Order = m.left.left.left.left.left.left.left.left.left.left.statusRegister.Order,
                        Units = m.left.left.left.left.left.left.left.left.left.left.statusRegister.Units,
                        UnitType = m.left.left.left.left.left.left.left.left.left.right.ParamStandardType != null
                            ? m.left.left.left.left.left.left.right.UnitTypeId
                            : m.left.left.left.left.left.left.left.left.left.right.UnitType,
                        StateId = m.left.left.left.left.left.left.left.left.left.right.PhraseId.HasValue
                            ? m.left.left.left.left.left.left.left.left.left.right.PhraseId.ToString()
                            : null,
                    });

                var result = context.StatusRegisters.GroupJoin(facilityTags, l => (int?)l.RegisterAddress, r => (int?)r.Address,
                        (left, right) => new
                        {
                            left,
                            right,
                        })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(parameters, l => (int?)l.left.left.RegisterAddress, r => (int?)r.Address,
                        (left, right) => new
                        {
                            left,
                            right,
                        })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    }).Where(m => m.right.Address != null || m.left.left.right.Address != null)
                    .Select(m => new RegisterRecordData()
                    {
                        Address = m.left.left.right.Address != null
                            ? m.left.left.right.Address
                            : m.right.Address,
                        Description = m.left.left.right.Address != null
                            ? m.left.left.right.Description
                            : m.right.Description,
                        DataType = m.left.left.right.Address != null
                            ? m.left.left.right.DataType
                            : m.right.DataType,
                        Decimals = m.left.left.right.Address != null
                            ? m.left.left.right.Decimals
                            : m.right.Decimals,
                        StringValue = m.left.left.right.Address != null
                            ? m.left.left.right.StringValue
                            : m.right.StringValue,
                        Value = m.left.left.right.Address != null
                            ? m.left.left.right.Value
                            : m.right.Value,
                        Format = m.left.left.right.Address != null
                            ? m.left.left.right.Format
                            : m.right.Format,
                        Order = m.left.left.right.Address != null
                            ? m.left.left.right.Order
                            : m.right.Order,
                        Units = m.left.left.right.Address != null
                            ? m.left.left.right.Units
                            : m.right.Units,
                        UnitType = m.left.left.right.Address != null
                            ? m.left.left.right.UnitType
                            : m.right.UnitType,
                    }).Distinct().OrderBy(m => m.Order);

                return result.ToList().Select(m => new RegisterData()
                {
                    Description = m.Description,
                    Value = m.UnitType != null ? _unitConversion.CreateUnitObject((int)m.UnitType,
                        float.TryParse(m.Value, out var parsedFloat) ? parsedFloat : 0f) : null,
                    Address = m.Address,
                    Decimals = m.Decimals,
                    Format = m.Format,
                    StringValue = m.StringValue,
                    Bit = m.Bit,
                    DataType = m.DataType,
                    PhraseId = m.PhraseId,
                    Order = m.Order,
                    UnitType = m.UnitType,
                    StateId = m.StateId,
                    Units = m.Units,
                }).ToList();
            }
#pragma warning restore IDE0029
        }

        private async Task<IList<RegisterData>> GetAssetStatusRegisterDataUsingMongoInflux(Guid assetId, Guid customerId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var nodeId = context.NodeMasters.Where(nm => nm.AssetGuid == assetId).Select(nm => nm.NodeId).FirstOrDefaultAsync().ToString();

                var influxData = await _currentRawScansStore.GetCurrentRawScanDataFromInflux(assetId, customerId, nodeId);

#pragma warning disable IDE0004, IDE0029
                var facilityTags = context.NodeMasters.Join(context.StatusRegisters, l => l.PocType, r => r.PocType,
                        (node, statusRegister) => new
                        {
                            node,
                            statusRegister,
                        })
                    .Join(context.FacilityTags, l => new
                    {
                        key1 = (int?)l.statusRegister.RegisterAddress,
                        key2 = l.node.NodeId,
                    },
                    r => new
                    {
                        key1 = (int?)r.Address,
                        key2 = r.NodeId,
                    }, (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.ParamStandardTypes, l => l.right.ParamStandardType, r => (int?)r.ParamStandardType,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.LocalePhrases, l => (int?)l.right.PhraseId, r => (int?)r.PhraseId,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.States, l => new
                    {
                        Key1 = l.left.left.left.left.right.StateId,
                        Key2 = l.left.left.left.left.right.CurrentValue,
                    }, r => new
                    {
                        Key1 = (int?)r.StateId,
                        Key2 = (float?)r.Value,
                    },
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .Where(m => m.left.left.left.left.left.left.left.node.AssetGuid == assetId)
                    .Select(m => new RegisterRecordData()
                    {
                        Address = m.left.left.left.left.left.left.left.statusRegister.RegisterAddress,
                        Description = m.left.left.left.left.left.left.right.ParamStandardType != null
                            ? m.left.left.left.left.right.Description
                            : m.left.left.left.left.left.left.right.Description,
                        DataType = m.left.left.left.left.left.left.right.DataType.HasValue
                            ? m.left.left.left.left.left.left.right.DataType.ToString()
                            : null,
                        Decimals = m.left.left.left.left.left.left.right.Decimals.Value,
                        StringValue = m.right.Text == null ? m.right.Text : "",
                        Value = m.left.left.left.left.left.left.right.CurrentValue.HasValue
                            ? m.left.left.left.left.left.left.right.CurrentValue.ToString()
                            : null,
                        Format = m.left.left.left.left.left.left.left.statusRegister.Format,
                        Order = m.left.left.left.left.left.left.left.statusRegister.Order,
                        Units = m.left.left.left.left.left.left.left.statusRegister.Units,
                        UnitType = m.left.left.left.left.left.left.right.ParamStandardType != null
                            ? m.left.left.left.left.right.UnitTypeId
                            : m.left.left.left.left.left.left.right.UnitType,
                        StateId = m.right.StateId.ToString(),
                    });

                var parameters = context.NodeMasters.Join(context.StatusRegisters, l => l.PocType, r => r.PocType,
                        (node, statusRegister) => new
                        {
                            node,
                            statusRegister,
                        }).Join(context.Parameters, l => new
                        {
                            Key1 = (int?)l.statusRegister.RegisterAddress,
                            Key2 = l.statusRegister.PocType,
                        },
                        r => new
                        {
                            Key1 = (int?)r.Address,
                            Key2 = r.Poctype,
                        }, (left, right) => new
                        {
                            left,
                            right,
                        })
                        .Join(influxData, l => new
                        {
                            Key1 = (int?)l.left.statusRegister.RegisterAddress,
                            Key2 = l.left.node.NodeId,
                        }, r => new
                        {
                            Key1 = (int?)r.Address,
                            Key2 = r.NodeId,
                        }, (left, right) => new
                        {
                            left,
                            right,
                        })
                    .GroupJoin(context.ParamStandardTypes, l => l.left.right.ParamStandardType,
                        r => (int?)r.ParamStandardType,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.LocalePhrases, l => (int?)l.right.PhraseId, r => (int?)r.PhraseId,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.LocalePhrases, l => (int?)l.left.left.left.left.left.right.PhraseId, r => (int?)r.PhraseId,
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(context.States, l => new
                    {
                        Key1 = (int?)l.left.left.left.left.left.left.left.right.StateId,
                        Key2 = l.left.left.left.left.left.left.right.Value,
                    }, r => new
                    {
                        Key1 = (int?)r.StateId,
                        Key2 = (float?)r.Value,
                    },
                        (left, right) =>
                            new
                            {
                                left,
                                right,
                            })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .Where(m => m.left.left.left.left.left.left.left.left.left.left.node.AssetGuid == assetId)
                    .Select(m => new RegisterRecordData()
                    {
                        Address = m.left.left.left.left.left.left.left.left.left.left.statusRegister.RegisterAddress,
                        Description = m.left.left.left.left.left.left.left.left.left.right.ParamStandardType != null
                            ? m.left.left.left.left.left.left.right.Description
                            : m.left.left.left.left.left.left.left.left.left.right.Description,
                        DataType = m.left.left.left.left.left.left.left.left.left.right.DataType.HasValue
                            ? m.left.left.left.left.left.left.left.left.left.right.DataType.ToString()
                            : null,
                        Decimals = m.left.left.left.left.left.left.left.left.left.right.Decimals,
                        StringValue = m.right.Text != null
                            ? m.right.Text
                            : m.left.left.left.left.left.left.left.left.right.StringValue != null
                                ? m.left.left.left.left.left.left.left.left.right.StringValue
                                : "",
                        Value = m.left.left.left.left.left.left.left.left.right.Value.HasValue
                            ? m.left.left.left.left.left.left.left.left.right.Value.ToString()
                            : null,
                        Format = m.left.left.left.left.left.left.left.left.left.left.statusRegister.Format,
                        Order = m.left.left.left.left.left.left.left.left.left.left.statusRegister.Order,
                        Units = m.left.left.left.left.left.left.left.left.left.left.statusRegister.Units,
                        UnitType = m.left.left.left.left.left.left.left.left.left.right.ParamStandardType != null
                            ? m.left.left.left.left.left.left.right.UnitTypeId
                            : m.left.left.left.left.left.left.left.left.left.right.UnitType,
                        StateId = m.left.left.left.left.left.left.left.left.left.right.PhraseId.HasValue
                            ? m.left.left.left.left.left.left.left.left.left.right.PhraseId.ToString()
                            : null,
                    });

                var result = context.StatusRegisters.GroupJoin(facilityTags, l => (int?)l.RegisterAddress, r => (int?)r.Address,
                        (left, right) => new
                        {
                            left,
                            right,
                        })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .GroupJoin(parameters, l => (int?)l.left.left.RegisterAddress, r => (int?)r.Address,
                        (left, right) => new
                        {
                            left,
                            right,
                        })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    }).Where(m => m.right.Address != null || m.left.left.right.Address != null)
                    .Select(m => new RegisterRecordData()
                    {
                        Address = m.left.left.right.Address != null
                            ? m.left.left.right.Address
                            : m.right.Address,
                        Description = m.left.left.right.Address != null
                            ? m.left.left.right.Description
                            : m.right.Description,
                        DataType = m.left.left.right.Address != null
                            ? m.left.left.right.DataType
                            : m.right.DataType,
                        Decimals = m.left.left.right.Address != null
                            ? m.left.left.right.Decimals
                            : m.right.Decimals,
                        StringValue = m.left.left.right.Address != null
                            ? m.left.left.right.StringValue
                            : m.right.StringValue,
                        Value = m.left.left.right.Address != null
                            ? m.left.left.right.Value
                            : m.right.Value,
                        Format = m.left.left.right.Address != null
                            ? m.left.left.right.Format
                            : m.right.Format,
                        Order = m.left.left.right.Address != null
                            ? m.left.left.right.Order
                            : m.right.Order,
                        Units = m.left.left.right.Address != null
                            ? m.left.left.right.Units
                            : m.right.Units,
                        UnitType = m.left.left.right.Address != null
                            ? m.left.left.right.UnitType
                            : m.right.UnitType,
                    }).Distinct().OrderBy(m => m.Order);

                return result.ToList().Select(m => new RegisterData()
                {
                    Description = m.Description,
                    Value = m.UnitType != null ? _unitConversion.CreateUnitObject((int)m.UnitType,
                        float.TryParse(m.Value, out var parsedFloat) ? parsedFloat : 0f) : null,
                    Address = m.Address,
                    Decimals = m.Decimals,
                    Format = m.Format,
                    StringValue = m.StringValue,
                    Bit = m.Bit,
                    DataType = m.DataType,
                    PhraseId = m.PhraseId,
                    Order = m.Order,
                    UnitType = m.UnitType,
                    StateId = m.StateId,
                    Units = m.Units,
                }).ToList();
            }
#pragma warning restore IDE0029
        }
    }
}