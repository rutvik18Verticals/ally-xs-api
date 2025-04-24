using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a category of supported units
    /// </summary>
    public class UnitCategory : EnhancedEnumBase
    {

        /// <summary>
        /// Device Type Enum
        /// </summary>
        public enum Type
        {

            /// <summary>
            /// Specifies the type is None.
            /// </summary>
            None = 0,

            /// <summary>
            /// Specifies the type is Discrete.
            /// </summary>
            Discrete = 1, // 1 Discrete

            /// <summary>
            /// Specifies the type is Integer.
            /// </summary>
            Integer = 2, // 2 Integer

            /// <summary>
            /// Specifies the type is FloatIEEE.
            /// </summary>
            FloatIEEE = 3, // 3 Float (IEEE)

            /// <summary>
            /// Specifies the type is FloatModicon.
            /// </summary>
            FloatModicon = 4, // 4 Float (modicon)

            /// <summary>
            /// Specifies the type is BakerTime.
            /// </summary>
            BakerTime = 5, // 5 Baker Time

            /// <summary>
            /// Specifies the type is BakerDate.
            /// </summary>
            BakerDate = 6, // 6 Baker Date

            /// <summary>
            /// Specifies the type is LongModicon.
            /// </summary>
            LongModicon = 7, // 7 Long (modicon)

            /// <summary>
            /// Specifies the type is FloatIEEE_Rev.
            /// </summary>
            FloatIEEE_Rev = 8, // 8 Float (IEEE rev)

            /// <summary>
            /// Specifies the type is Date1970.
            /// </summary>
            Date1970 = 9, // 9	Date since '70

            /// <summary>
            /// Specifies the type is BCD.
            /// </summary>
            BCD = 10, // 10 BCD

            /// <summary>
            /// Specifies the type is LongUnico.
            /// </summary>
            LongUnico = 11, // 11 Long (unico)

            /// <summary>
            /// Specifies the type is LongPickford.
            /// </summary>
            LongPickford = 12, // 12 Long (pickford)

            /// <summary>
            /// Specifies the type is ABTimer.
            /// </summary>
            ABTimer = 13, // 13 AB Timer

            /// <summary>
            /// Specifies the type is SignedInteger.
            /// </summary>
            SignedInteger = 14, // 14 Signed Integer

            /// <summary>
            /// Specifies the type is Byte_Int8.
            /// </summary>
            Byte_Int8 = 15, // Int8 / TAI_CHAR

            /// <summary>
            /// Specifies the type is DJAXTime.
            /// </summary>
            DJAXTime = 16, // 16	DJAX Time

            /// <summary>
            /// Specifies the type is DJAXDate.
            /// </summary>
            DJAXDate = 17, // 17	DJAX Date

            /// <summary>
            /// Specifies the type is DateReverseWordOrder1970.
            /// </summary>
            DateReverseWordOrder1970 = 18,

            /// <summary>
            /// Specifies the type is LongEnron.
            /// </summary>
            LongEnron = 19, // 19	Long Enron

            /// <summary>
            /// Specifies the type is FloatEnron.
            /// </summary>
            FloatEnron = 20, // 20	Float (Enron)

            /// <summary>
            /// Specifies the type is DateEnron.
            /// </summary>
            DateEnron = 21, // 21	Date (Enron)

            /// <summary>
            /// Specifies the type is TimeEnron.
            /// </summary>
            TimeEnron = 22, // 22	Time (Enron)

            /// <summary>
            /// Specifies the type is ROC_TLP.
            /// </summary>
            ROC_TLP = 26, // Byte[3] TLP Pointer used by Fisher ROC

            /// <summary>
            /// Specifies the type is FloatDualSwap.
            /// </summary>
            FloatDualSwap = 27, // 27 Float (dual swap)

            /// <summary>
            /// Specifies the type is StringType.
            /// </summary>
            StringType = 28, // 28 String

            /// <summary>
            /// Specifies the type is TotalflowRegister.
            /// </summary>
            TotalflowRegister = 29, // TAI_REGISTER(4 byte struct; app, array, index)

            /// <summary>
            /// Specifies the type is Double.
            /// </summary>
            Double = 30, // TAI_DOUBLE (8 byte real)

        }

        #region Static Properties

        /// <summary>
        /// Gets the unsupported value
        /// </summary>
        public static UnitCategory Unsupported => GetValue<UnitCategory>(-1);

        /// <summary>
        /// Gets the default value
        /// </summary>
        public static UnitCategory Default { get; private set; }

        /// <summary>
        /// Specifies that the data has no unit
        /// </summary>
        public static UnitCategory None => Default;

        /// <summary>
        /// Specifies that the data represents a fluid rate
        /// </summary>
        public static UnitCategory FluidRate { get; private set; }

        /// <summary>
        /// Specifies that the data represents a chemical rate
        /// </summary>
        public static UnitCategory ChemicalRate { get; private set; }

        /// <summary>
        /// Specifies that the data represents a gas rate
        /// </summary>
        public static UnitCategory GasRate { get; private set; }

        /// <summary>
        /// Specifies that the data represents a pressure
        /// </summary>
        public static UnitCategory Pressure { get; private set; }

        /// <summary>
        /// Specifies that the data represents a temperatue
        /// </summary>
        public static UnitCategory Temperature { get; private set; }

        /// <summary>
        /// Specifies that the data represents a shorter length
        /// </summary>
        public static UnitCategory ShortLength { get; private set; }

        /// <summary>
        /// Specifies that the data represents a longer length
        /// </summary>
        public static UnitCategory LongLength { get; private set; }

        /// <summary>
        /// Specifies that the data represents a weight
        /// </summary>
        public static UnitCategory Weight { get; private set; }

        /// <summary>
        /// Specifies that the data represents a fluid volume
        /// </summary>
        public static UnitCategory FluidVolume { get; private set; }

        /// <summary>
        /// Specifies that the data represents a Chemical volume
        /// </summary>
        public static UnitCategory ChemicalVolume { get; private set; }

        /// <summary>
        /// Specifies that the data represents a gas volume
        /// </summary>
        public static UnitCategory GasVolume { get; private set; }

        /// <summary>
        /// Specifies that the data represents a current
        /// </summary>
        public static UnitCategory Current { get; private set; }

        /// <summary>
        /// Specifies that the data represents a voltage
        /// </summary>
        public static UnitCategory Voltage { get; private set; }

        /// <summary>
        /// Specifies that the data represents a relative density
        /// </summary>
        public static UnitCategory RelativeDensity { get; private set; }

        /// <summary>
        /// Specifies that the data represents torque
        /// </summary>
        public static UnitCategory Torque { get; private set; }

        /// <summary>
        /// Specifies that the data represents money
        /// </summary>
        public static UnitCategory Money { get; private set; }

        /// <summary>
        /// Specifies that the data represents a frequency
        /// </summary>
        public static UnitCategory Frequency { get; private set; }

        /// <summary>
        /// Specifies that the data represents energy
        /// </summary>
        public static UnitCategory Energy { get; private set; }

        /// <summary>
        /// Specifies that the data represents power
        /// </summary>
        public static UnitCategory Power { get; private set; }

        /// <summary>
        /// Specifies that the data represents oil relative density
        /// </summary>
        public static UnitCategory OilRelativeDensity { get; private set; }

        /// <summary>
        /// Specifies that the data represents head
        /// </summary>
        public static UnitCategory Head { get; private set; }

        /// <summary>
        /// Specifies that the data represents density
        /// </summary>
        public static UnitCategory Density { get; private set; }

        /// <summary>
        /// Specifies that the data represents minute
        /// </summary>
        public static UnitCategory Minute { get; private set; }

        /// <summary>
        /// Specifies that the data represents day
        /// </summary>
        public static UnitCategory Day { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Determines whether this instance is "rate".
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is rate; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRate
        {
            get
            {
                if (this == FluidRate)
                {
                    return true;
                }

                if (this == GasRate)
                {
                    return true;
                }

                if (this == ChemicalRate)
                {
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// The data type for the parameter.
        /// </summary>
        public class ParameterDataType
        {

            /// <summary>
            /// Gets or sets the data type.
            /// </summary>
            public int DataType { get; set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Determines whether the data type is numeric.<c>True</c> if the data type is numeric; otherwise <c>false</c>.
            /// </summary>
            public bool IsNumeric => DataType switch
            {
                (int)Type.ABTimer or (int)Type.BCD or (int)Type.Discrete or (int)Type.FloatEnron or (int)Type.FloatIEEE
                    or (int)Type.FloatIEEE_Rev or (int)Type.FloatModicon or (int)Type.Integer or (int)Type.LongEnron
                    or (int)Type.LongModicon or (int)Type.LongPickford or (int)Type.LongUnico or (int)Type.SignedInteger
                    or (int)Type.Byte_Int8 => true,
                (int)Type.BakerDate or (int)Type.BakerTime or (int)Type.Date1970 or (int)Type.DateEnron or (int)Type.DJAXDate
                    or (int)Type.DJAXTime or (int)Type.TimeEnron or (int)Type.ROC_TLP or (int)Type.StringType
                    or (int)Type.TotalflowRegister => false,
                _ => false,
            };

        }

        static UnitCategory()
        {
            Default = CreateValue(0, "None");
            FluidRate = CreateValue(1, "Fluid Rate");
            GasRate = CreateValue(2, "Gas Rate");
            Pressure = CreateValue(3, "Pressure");
            Temperature = CreateValue(4, "Temperature");
            ShortLength = CreateValue(5, "Short Length");
            LongLength = CreateValue(6, "Long Length");
            Weight = CreateValue(7, "Weight");
            GasVolume = CreateValue(8, "Gas Volume");
            Current = CreateValue(9, "Current");
            Voltage = CreateValue(10, "Voltage");
            RelativeDensity = CreateValue(11, "Relative Density");
            Torque = CreateValue(12, "Torque");
            Money = CreateValue(13, "Money");
            Frequency = CreateValue(14, "Frequency");
            Energy = CreateValue(15, "Energy");
            Power = CreateValue(16, "Power");
            FluidVolume = CreateValue(17, "Fluid Volume");
            OilRelativeDensity = CreateValue(18, "Oil Relative Density");
            Head = CreateValue(19, "Head");
            ChemicalRate = CreateValue(20, "Chemical Rate");
            ChemicalVolume = CreateValue(21, "Chemical Volume");
            Density = CreateValue(22, "Density");
            Minute = CreateValue(23, "Minute");
            Day = CreateValue(24, "Day");
        }

        /// <summary>
        /// Initializes a new UnitCategory with a specified key and name
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="name">The name</param>
        protected UnitCategory(int key, Text name)
            : base(key, name)
        {
        }

        #endregion

        #region Public Methods

        #endregion

        #region Static Methods

        private static UnitCategory CreateValue(int key, string name)
        {
            var value = new UnitCategory(key, new Text(name));

            Register(value);

            return value;
        }

        #endregion

    }
}
