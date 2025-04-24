namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a category of supported units
    /// </summary>
    public enum UnitCategory
    {

        /// <summary>
        /// Gets the unsupported value
        /// </summary>
        Unsupported = -1,

        /// <summary>
        /// Specifies that the data has no unit
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the data represents a fluid rate
        /// </summary>
        FluidRate = 1,

        /// <summary>
        /// Specifies that the data represents a gas rate
        /// </summary>
        GasRate = 2,

        /// <summary>
        /// Specifies that the data represents a pressure
        /// </summary>
        Pressure = 3,

        /// <summary>
        /// Specifies that the data represents a temperature
        /// </summary>
        Temperature = 4,

        /// <summary>
        /// Specifies that the data represents a shorter length
        /// </summary>
        ShortLength = 5,

        /// <summary>
        /// Specifies that the data represents a longer length
        /// </summary>
        LongLength = 6,

        /// <summary>
        /// Specifies that the data represents a weight
        /// </summary>
        Weight = 7,

        /// <summary>
        /// Specifies that the data represents a gas volume
        /// </summary>
        GasVolume = 8,

        /// <summary>
        /// Specifies that the data represents a current
        /// </summary>
        Current = 9,

        /// <summary>
        /// Specifies that the data represents a voltage
        /// </summary>
        Voltage = 10,

        /// <summary>
        /// Specifies that the data represents a relative density
        /// </summary>
        RelativeDensity = 11,

        /// <summary>
        /// Specifies that the data represents torque
        /// </summary>
        Torque = 12,

        /// <summary>
        /// Specifies that the data represents money
        /// </summary>
        Money = 13,

        /// <summary>
        /// Specifies that the data represents a frequency
        /// </summary>
        Frequency = 14,

        /// <summary>
        /// Specifies that the data represents energy
        /// </summary>
        Energy = 15,

        /// <summary>
        /// Specifies that the data represents power
        /// </summary>
        Power = 16,

        /// <summary>
        /// Specifies that the data represents a fluid volume
        /// </summary>
        FluidVolume = 17,

        /// <summary>
        /// Specifies that the data represents oil relative density
        /// </summary>
        OilRelativeDensity = 18,

        /// <summary>
        /// Specifies that the data represents head
        /// </summary>
        Head = 19,

        /// <summary>
        /// Specifies that the data represents a chemical rate
        /// </summary>
        ChemicalRate = 20,

        /// <summary>
        /// Specifies that the data represents a Chemical volume
        /// </summary>
        ChemicalVolume = 21,

        /// <summary>
        /// Specifies that the data represents density
        /// </summary>
        Density = 22,

        /// <summary>
        /// Specifies that the data represents minute
        /// </summary>
        Minute = 23,

        /// <summary>
        /// Specifies that the data represents day
        /// </summary>
        Day = 24,
    }
}