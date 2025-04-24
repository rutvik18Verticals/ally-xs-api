using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Mappers
{
    /// <summary>
    /// Maps CardResponseValuesModel to CardResponseValues object.
    /// </summary>
    public class CardCoordinateDataMapper
    {

        #region Public Methods

        /// <summary>
        /// Maps the <paramref name="cardData"/> to a <seealso cref="CardCoordinateDataOutput"/> domain object.
        /// </summary>
        /// <param name="cardData">The entity to map from.</param>
        /// <param name="commonService">The <see cref="ICommonService"/>.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <seealso cref="CardCoordinateDataOutput"/> representing the provided <paramref name="cardData"/> 
        /// in the domain.</returns>
        public static CardCoordinateDataOutput MapToDomainObject(CardCoordinateModel cardData, ICommonService commonService, string correlationId)
        {
            if (cardData == null)
            {
                throw new ArgumentNullException(nameof(cardData));
            }

            if (commonService == null)
            {
                throw new ArgumentNullException(nameof(commonService));
            }

            var response = new CardCoordinateDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            var responseValues = new List<CardResponseValuesOutput>();
            var coordinates = new List<CoordinatesData<float>>();

            // View Surface Card
            var points = GetLoadsPositions(cardData.SurfaceCardBinary);
            var surfaceCardMaxValue = 0f;
            var surfaceCardMinValue = 0f;
            var surfaceCardMaxPositionValue = 0f;
            var surfaceCardMinPositionValue = 0f;

            if (points.Count == 0)
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.SurfaceCard,
                    Name = "Surface Card",
                    CoordinatesOutput = new List<CoordinatesData<float>>(),
                });
            }
            else
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.SurfaceCard,
                    Name = "Surface Card",
                    CoordinatesOutput = MapToCoordinates(points, true, commonService, correlationId),
                });

                surfaceCardMinValue = 99999f;
                surfaceCardMinPositionValue = 99999f;

                foreach (var item in points)
                {
                    if (item.Load > surfaceCardMaxValue)
                    {
                        surfaceCardMaxValue = item.Load;
                    }

                    if (item.Load < surfaceCardMinValue)
                    {
                        surfaceCardMinValue = item.Load;
                    }

                    if (item.Position > surfaceCardMaxPositionValue)
                    {
                        surfaceCardMaxPositionValue = item.Position;
                    }

                    if (item.Position < surfaceCardMinPositionValue)
                    {
                        surfaceCardMinPositionValue = item.Position;
                    }
                }
            }

            // View POC Downhole Card
            points = GetLoadsPositions(cardData.PocDownHoleCardBinary);

            var downholeCardMaxValue = 0f;
            var downholeCardMinValue = 0f;
            var downholeCardMaxPositionValue = 0f;
            var downholeCardMinPositionValue = 0f;
            if (points.Count == 0)
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.POCDownholeCard,
                    Name = "POC Downhole Card",
                    CoordinatesOutput = new List<CoordinatesData<float>>(),
                });
            }
            else
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.POCDownholeCard,
                    Name = "POC Downhole Card",
                    CoordinatesOutput = MapToCoordinates(points, true, commonService, correlationId),
                });

                downholeCardMinValue = 99999f;
                downholeCardMinPositionValue = 99999f;

                foreach (var item in points)
                {
                    if (item.Load > downholeCardMaxValue)
                    {
                        downholeCardMaxValue = item.Load;
                    }

                    if (item.Load < downholeCardMinValue)
                    {
                        downholeCardMinValue = item.Load;
                    }

                    if (item.Position > downholeCardMaxPositionValue)
                    {
                        downholeCardMaxPositionValue = item.Position;
                    }

                    if (item.Position < downholeCardMinPositionValue)
                    {
                        downholeCardMinPositionValue = item.Position;
                    }
                }
            }

            // View Downhole Card
            points = GetLoadsPositions(cardData.DownHoleCardBinary);

            if (points.Count == 0)
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.DownholeCard,
                    Name = "Downhole Card",
                    CoordinatesOutput = new List<CoordinatesData<float>>(),
                });
            }
            else
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.DownholeCard,
                    Name = "Downhole Card",
                    CoordinatesOutput = MapToCoordinates(points, true, commonService, correlationId),
                });
            }

            // View Predicted Card
            points = GetLoadsPositions(cardData.PredictedCardBinary);

            if (points.Count == 0)
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.PredictedCard,
                    Name = "Predicted Card",
                    CoordinatesOutput = new List<CoordinatesData<float>>(),
                });
            }
            else
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.PredictedCard,
                    Name = "Predicted Card",
                    CoordinatesOutput = MapToCoordinates(points, true, commonService, correlationId),
                });
            }

            // View Permissible Load Up Card
            points = GetLoadsPositions(cardData.PermissibleLoadUpBinary);

            if (points.Count == 0)
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.PermissibleLoadUp,
                    Name = "Permissible Load",
                    CoordinatesOutput = new List<CoordinatesData<float>>(),
                });
            }
            else
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.PermissibleLoadUp,
                    Name = "Permissible Load",
                    CoordinatesOutput = MapToCoordinates(points, false, commonService, correlationId),
                });
            }

            // View Permissible Load Up Card
            points = GetLoadsPositions(cardData.PermissibleLoadDownBinary);

            if (points.Count == 0)
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.PermissibleLoadDown,
                    Name = "Permissible Load",
                    CoordinatesOutput = new List<CoordinatesData<float>>(),
                });
            }
            else
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.PermissibleLoadDown,
                    Name = "Permissible Load",
                    CoordinatesOutput = MapToCoordinates(points, false, commonService, correlationId),
                });
            }

            // View Load Limits
            var highLoadLimit = cardData.HiLoadLimit;

            if (highLoadLimit.HasValue == false)
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.HiLoadLimit,
                    Name = "High Load Limit",
                    CoordinatesOutput = new List<CoordinatesData<float>>(),
                });
            }
            else
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.HiLoadLimit,
                    Name = "High Load Limit",
                    CoordinatesOutput = new List<CoordinatesData<float>>()
                    {
                        new CoordinatesData<float>()
                        {
                            X = 0,
                            Y = FormatValue(highLoadLimit.Value, commonService, correlationId),
                        },
                    },
                });
            }

            var loLoadLimit = cardData.LowLoadLimit;

            if (loLoadLimit.HasValue == false)
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.LoLoadLimit,
                    Name = "Low Load Limit",
                    CoordinatesOutput = new List<CoordinatesData<float>>(),
                });
            }
            else
            {
                responseValues.Add(new CardResponseValuesOutput()
                {
                    Id = (int)CardId.LoLoadLimit,
                    Name = "Low Load Limit",
                    CoordinatesOutput = new List<CoordinatesData<float>>()
                    {
                        new CoordinatesData<float>()
                        {
                            X = 0,
                            Y = FormatValue(loLoadLimit.Value, commonService, correlationId),
                        },
                    },
                });
            }

            // View Set Points
            loLoadLimit = cardData.LowLoadLimit ?? 0;
            var hiLoadLimit = cardData.HiLoadLimit ?? 0;
            var areaLimit = cardData.AreaLimit;
            var fillBasePct = cardData.FillBasePercent ?? 0;
            var unicoFillBase = cardData.SecondaryPumpFillage ?? 0;
            var malfunctionPositionLimitPercent = cardData.MalfunctionPositionLimit ?? 0;
            var malfunctionLoadLimitPercent = cardData.MalfunctionLoadLimit ?? 0;
            var samFillage = cardData.Fillage ?? 0;
            var fillageSetpointPercent = cardData.AreaLimit;
            var secondaryFillageSetpointPercent = cardData.SecondaryPumpFillage ?? 0;
            var pocPositionLimitPercent = cardData.PositionLimit ?? 0;
            var pocLoadLimitPercent = cardData.LoadLimit ?? 0;
            var positionLimit = (cardData.PositionLimit ?? 0) * (cardData.StrokeLength ?? 0) / 100;
            var positionLimit2 = (cardData.PositionLimit2 ?? 0) * (cardData.StrokeLength ?? 0) / 100;
            var loadLimit = cardData.LoadLimit ?? 0;
            var loadLimit2 = cardData.LoadLimit2 ?? 0;

            if (cardData.PocType == (short)DeviceType.RPC_BMTS)
            {
                malfunctionPositionLimitPercent = cardData.AreaLimit;
                malfunctionLoadLimitPercent = cardData.FillBasePercent ?? 0;
            }

            // AddSetpointSurface
            switch (cardData.PocType)
            {
                case (short)DeviceType.RPC_BMTS:
                    responseValues.Add(new CardResponseValuesOutput()
                    {
                        Id = (int)CardId.MalfunctionPoint,
                        Name = "Malfunction Point",
                        CoordinatesOutput = new List<CoordinatesData<float>>()
                        {
                            new CoordinatesData<float>()
                            {
                                X = FormatValue(surfaceCardMinPositionValue +
                                    (surfaceCardMaxPositionValue - surfaceCardMinPositionValue) *
                                    malfunctionPositionLimitPercent / 100, commonService, correlationId),
                                Y = FormatValue(surfaceCardMinValue + (surfaceCardMaxValue - surfaceCardMinValue) *
                                    malfunctionLoadLimitPercent / 100, commonService, correlationId),
                            },
                        },
                    });

                    break;
                case (short)DeviceType.RPC_Unico:
                case >= 501 and <= 520:

                    break;
                case (short)DeviceType.RPC_AE:
                    responseValues.Add(new CardResponseValuesOutput()
                    {
                        Id = (int)CardId.AreaPoint1,
                        Name = "Area Point",
                        CoordinatesOutput = new List<CoordinatesData<float>>()
                        {
                            new CoordinatesData<float>()
                            {
                                X = FormatValue(positionLimit, commonService, correlationId),
                                Y = FormatValue(loadLimit, commonService, correlationId),
                            },
                        },
                    });

                    responseValues.Add(new CardResponseValuesOutput()
                    {
                        Id = (int)CardId.AreaPoint2,
                        Name = "Area Point",
                        CoordinatesOutput = new List<CoordinatesData<float>>()
                        {
                            new CoordinatesData<float>()
                            {
                                X = FormatValue(positionLimit2, commonService, correlationId),
                                Y = FormatValue(loadLimit2, commonService, correlationId),
                            },
                        },
                    });

                    break;
                default:
                    if (malfunctionPositionLimitPercent > 0 || malfunctionLoadLimitPercent > 0)
                    {
                        responseValues.Add(new CardResponseValuesOutput()
                        {
                            Id = (int)CardId.MalfunctionPoint,
                            Name = "Malfunction Point",
                            CoordinatesOutput = new List<CoordinatesData<float>>()
                            {
                                new CoordinatesData<float>()
                                {
                                    X = FormatValue(surfaceCardMinPositionValue +
                                        (surfaceCardMaxPositionValue - surfaceCardMinPositionValue) *
                                        malfunctionPositionLimitPercent / 100, commonService, correlationId),
                                    Y = FormatValue(surfaceCardMinValue + (surfaceCardMaxValue - surfaceCardMinValue) *
                                        malfunctionLoadLimitPercent / 100, commonService, correlationId),
                                },
                            },
                        });
                    }

                    if ((positionLimit != 0 || loadLimit != 0) && ControlSurface(cardData.PocType, cardData.Area))
                    {
                        responseValues.Add(new CardResponseValuesOutput()
                        {
                            Id = (int)CardId.POCSetpoint,
                            Name = "POC Setpoint",
                            CoordinatesOutput = new List<CoordinatesData<float>>()
                            {
                                new CoordinatesData<float>()
                                {
                                    X = FormatValue(surfaceCardMinPositionValue +
                                        (surfaceCardMaxPositionValue - surfaceCardMinPositionValue) *
                                        pocPositionLimitPercent / 100, commonService, correlationId),
                                    Y = FormatValue(surfaceCardMinValue + (surfaceCardMaxValue - surfaceCardMinValue) *
                                        pocLoadLimitPercent / 100, commonService, correlationId),
                                },
                            },
                        });
                    }

                    break;
            }

            // AddSetpointDownhole
            if (responseValues.First(x => x.Id == (int)CardId.POCDownholeCard).CoordinatesOutput.Count > 0
                && (cardData != null && ControlSurface(cardData.PocType, cardData.Area)) == false)
            {
                switch (cardData.PocType)
                {
                    case (short)DeviceType.RPC_Well_Pilot:
                        responseValues.Add(new CardResponseValuesOutput()
                        {
                            Id = (int)CardId.FillageSetpoint,
                            Name = "Fillage Setpoint",
                            CoordinatesOutput = new List<CoordinatesData<float>>()
                            {
                                new CoordinatesData<float>()
                                {
                                    X = FormatValue(downholeCardMinPositionValue +
                                        (downholeCardMaxPositionValue - downholeCardMinPositionValue) *
                                        fillageSetpointPercent / 100, commonService, correlationId),
                                    Y = 0, // TODO Need to investigate some more
                                },
                            },
                        });

                        break;
                    case (short)DeviceType.RPC_Unico:
                        responseValues.Add(new CardResponseValuesOutput()
                        {
                            Id = (int)CardId.LimitLine,
                            Name = "Limit Line",
                            CoordinatesOutput = new List<CoordinatesData<float>>()
                            {
                                new CoordinatesData<float>()
                                {
                                    X = 0,
                                    Y = FormatValue(unicoFillBase, commonService, correlationId),
                                },
                            },
                        });

                        responseValues.Add(new CardResponseValuesOutput()
                        {
                            Id = (int)CardId.FillageSetpoint,
                            Name = "Fillage Setpoint",
                            CoordinatesOutput = new List<CoordinatesData<float>>()
                            {
                                new CoordinatesData<float>()
                                {
                                    X = FormatValue(downholeCardMinPositionValue +
                                        (downholeCardMaxPositionValue - downholeCardMinPositionValue) *
                                        fillageSetpointPercent / 100, commonService, correlationId),
                                    Y = FormatValue(unicoFillBase, commonService, correlationId),
                                },
                            },
                        });

                        break;
                    default:
                        responseValues.Add(new CardResponseValuesOutput()
                        {
                            Id = (int)CardId.LimitLine,
                            Name = "Limit Line",
                            CoordinatesOutput = new List<CoordinatesData<float>>()
                            {
                                new CoordinatesData<float>()
                                {
                                    X = 0,
                                    Y = FormatValue(downholeCardMinValue +
                                        (downholeCardMaxValue - downholeCardMinValue) * fillBasePct / 100, commonService, correlationId),
                                },
                            },
                        });

                        responseValues.Add(new CardResponseValuesOutput()
                        {
                            Id = (int)CardId.FillageSetpoint,
                            Name = "Fillage Setpoint",
                            CoordinatesOutput = new List<CoordinatesData<float>>()
                            {
                                new CoordinatesData<float>()
                                {
                                    X = FormatValue(downholeCardMinPositionValue +
                                        (downholeCardMaxPositionValue - downholeCardMinPositionValue) *
                                        fillageSetpointPercent / 100, commonService, correlationId),
                                    Y = FormatValue(downholeCardMinValue +
                                        (downholeCardMaxValue - downholeCardMinValue) * fillBasePct / 100, commonService, correlationId),
                                },
                            },
                        });

                        if (secondaryFillageSetpointPercent != 0)
                        {
                            responseValues.Add(new CardResponseValuesOutput()
                            {
                                Id = (int)CardId.SecondaryFillageSetpoint,
                                Name = "Secondary Fillage Setpoint",
                                CoordinatesOutput = new List<CoordinatesData<float>>()
                                {
                                    new CoordinatesData<float>()
                                    {
                                        X = FormatValue(downholeCardMinPositionValue +
                                            (downholeCardMaxPositionValue - downholeCardMinPositionValue) *
                                            fillageSetpointPercent / 100, commonService, correlationId),
                                        Y = FormatValue(downholeCardMinValue + (downholeCardMaxValue - downholeCardMinValue) *
                                            secondaryFillageSetpointPercent / 100, commonService, correlationId),
                                    },
                                },
                            });
                        }

                        break;
                }
            }

            response.Id = correlationId;
            response.DateCreated = DateTime.UtcNow;
            response.Values = responseValues;

            return response;
        }

        #endregion

        #region PrivateMethods

        // This is used for setting constraint type and corresponds to the VB6 enum eControlMode used to check the value of the Area field in tblCardData 
        // SAM controller 0,1 - 0=Surface Control; VSP controller 3,4 - 3=Surface Control
        private static bool ControlSurface(short pocType, int area)
        {
            return pocType switch
            {
                (short)DeviceType.RPC_Lufkin_SAM => area == 0 || area == 3,
                (short)DeviceType.RPC_Rockwell_OptiLift => area == 0,
                (short)DeviceType.RPC_Well_Pilot => area == 0,
                (short)DeviceType.RPC_Spirit_SMARTEN => area == 5,
                (short)DeviceType.RPC_Unico => false,
                (short)DeviceType.RPC_WellLynx or (short)DeviceType.RPC_WellWorx => area != 1,
                _ => true,
            };
        }

        private static float FormatValue(float value, ICommonService commonService, string correlationId)
        {
            var digits = commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            if (!double.TryParse(value.ToString(), out var valueDouble))
            {
                return value;
            }

            var yDoubleRounded = MathUtility.RoundToSignificantDigits(valueDouble, digits);

            if (float.TryParse(yDoubleRounded.ToString(), out var yFloatRounded))
            {
                return yFloatRounded;
            }

            return value;
        }

        private static List<CoordinatesData<float>> MapToCoordinates(IList<LoadPositionPoint> loadPositionPoints,
            bool shouldConnectFirstAndLastPoint, ICommonService commonService, string correlationId)
        {
            var coordinates = new List<CoordinatesData<float>>();

            foreach (var point in loadPositionPoints)
            {
                if ((point.Load == 0 && point.Position == 0) == false)
                {
                    coordinates.Add(new CoordinatesData<float>()
                    {
                        X = FormatValue(point.Position, commonService, correlationId),
                        Y = FormatValue(point.Load, commonService, correlationId),
                    });
                }
            }

            if (coordinates.Count > 0 && shouldConnectFirstAndLastPoint)
            {
                if (coordinates.First().X != coordinates.Last().X || coordinates.First().Y != coordinates.Last().Y)
                {
                    coordinates.Add(new CoordinatesData<float>()
                    {
                        X = FormatValue(coordinates.First().X, commonService, correlationId),
                        Y = FormatValue(coordinates.First().Y, commonService, correlationId),
                    });
                }
            }

            return coordinates;
        }

        // TODO this is no where in XSPOC, something new.
        private enum CardId
        {

            SurfaceCard = 1,
            POCDownholeCard = 2,
            DownholeCard = 3,
            PredictedCard = 4,
            PermissibleLoadUp = 5,
            PermissibleLoadDown = 6,
            HiLoadLimit = 7,
            LoLoadLimit = 8,
            MalfunctionPoint = 9,
            AreaPoint1 = 10,
            AreaPoint2 = 11,
            POCSetpoint = 12,
            FillageSetpoint = 13,
            LimitLine = 14,
            SecondaryFillageSetpoint = 15,

        }

        private struct LoadPositionPoint
        {

            public float Load { get; set; }
            public float Position { get; set; }

            public LoadPositionPoint(float load, float position)
            {
                Load = load;
                Position = position;
            }

        }

        private static IList<LoadPositionPoint> GetLoadsPositions(byte[] bytes)
        {
            // This function is used on saved cards stored in the fields of tblCardData
            // bytes should have a length of 1600 bytes:
            //     Byte Description
            // -------- ------------------------------------------------
            //    0-799 Load data, 4 bytes each 
            // 800-1599 Position data, 4 bytes each 

            var points = new List<LoadPositionPoint>();

            if (bytes == null || bytes.Length <= 0)
            {
                return points;
            }

            for (var i = 0; i <= (bytes.Length / 2) - 1; i += 4)
            {
                var loads = new byte[4];

                loads[0] = (bytes[i]);
                loads[1] = (bytes[i + 1]);
                loads[2] = (bytes[i + 2]);
                loads[3] = (bytes[i + 3]);

                var positions = new byte[4];

                positions[0] = (bytes[(bytes.Length / 2) + i]);
                positions[1] = (bytes[(bytes.Length / 2) + i + 1]);
                positions[2] = (bytes[(bytes.Length / 2) + i + 2]);
                positions[3] = (bytes[(bytes.Length / 2) + i + 3]);

                points.Add(new LoadPositionPoint(BitConverter.ToSingle(loads, 0), BitConverter.ToSingle(positions)));
            }

            return points;
        }

        #endregion

    }
}
