using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Contains measurements phrases for a given user.
    /// </summary>
    /// <remarks>
    /// If no user is given, it will attempt to use the system measurements,
    /// otherwise it will assign the measurement type, ie. 'Length', instead of 'Feet' or 'Meters'.
    /// </remarks>
    public class MeasurementUtility
    {

        #region Constants

        private const int NONE = 731; // None
        private const int FLUID_RATE = 20026; // Power
        private const int GAS_RATE = 1251; // Gas Rate
        private const int PRESSURE = 754; // Pressure
        private const int TEMPERATURE = 752; // Temperature
        private const int SHORT_LENGTH = 756; // Short Length
        private const int LENGTH = 755; // Length
        private const int WEIGHT = 3632; // Weight
        private const int POWER = 4025; // Power
        private const int BARRELS_PER_DAY_ABBR = 725; // bpd
        private const int CUBIC_METERS_ABBR = 555; // m3d
        private const int BARRELS_PER_DAY = 757; // Barrels per day (bpd)
        private const int CUBIC_METERS = 758; // Cubic meters (m3)
        private const int FARENHEIT = 759; // Fahrenheit (F)
        private const int CELSIUS = 760; // Celsius (C)
        private const int THOUSAND_CUBIC_FEET = 761; // Thousand Cubic Feet (mcf)
        private const int THOUSAND_CUBIC_FEET_ABBR = 4856; // mscfd
        private const int POUNDS_PER_SQUARE_INCH = 762; // Pounds per Square Inch (psi)
        private const int KILO_PASCALS = 763; // Kilo-Pascals (kPa)
        private const int FEET = 764; // Feet (ft)
        private const int METERS = 765; // Meters (m)
        private const int INCHES = 766; // Inches (in)
        private const int CENTIMETERS = 767; // Centimeters (cm)
        private const int HORSEPOWER = 4026; // Horsepower (hp)
        private const int KILOWATTS = 4027; // Kilowatts (kW)
        private const int POUNDS = 4028; // Pounds (lbs)
        private const int KILOGRAMS = 4029; // Kilograms (kg)
        private const int POUNDS_PER_SQUARE_INCH_ABBR = 1644; // psi
        private const int FARENHEIT_ABBR = 3138; // °F
        private const int CELSIUS_ABBR = 556; // °C
        private const int KILO_PASCALS_ABBR = 557; // kPa
        private const int FEET_ABBR = 1643; // ft
        private const int METERS_ABBR = 558; // m
        private const int INCHES_ABBR = 289; // in
        private const int CENTIMETERS_ABBR = 559; // cm
        private const int HORSEPOWER_ABBR = 2968; // hp
        private const int KILOWATTS_ABBR = 3137; // kW
        private const int POUNDS_ABBR = 284; // lbs
        private const int KILOGRAMS_ABBR = 5080; // kg
        private const int GAS_VOLUME = 4658; // Gas Volume
        private const int CHEMICAL_RATE = 5983; // Chemical Rate
        private const int CHEMICAL_VOLUME = 5984; // Chemical Volume
        private const int FLUID_VOLUME = 6032; // Fluid Volume
        private const int THOUSAND_CUBIC_METERS = 6496; // Thousand Cubic Meters (Mm3)
        private const int THOUSAND_CUBIC_METERS_ABBR = 6497; // Mm3
        private const int OIL_GRAVITY = 101402; // Oil Gravity
        private const int SPECIFIC_GRAVITY = 2277; // Specific Gravity
        private const int GCM = 5076; // g/cm³
        private const int API = 5075; // API °
        private const int OIL_GRAVITY_API = 268; // Oil Gravity (API°)
        private const int CURRENT = 142; // Current
        private const int TORQUE = 604; // Torque
        private const int FREQUENCY = 1414; // Frequency
        private const int ENERGY = 5722; // Energy
        private const int HEAD = 2967; // Head
        private const int DENSITY = 6278; // Density
        private const int MINUTES = 7052; // Minutes
        private const int DAYS = 6093; // Days
        private const int VOLTAGE = 7053; // Voltage
        private const int RELATIVE_DENSITY = 6744; // Relative Density
        private const int MONEY = 6745; // MONEY

        #endregion

        #region Private members

        private readonly ILocalePhrases _phraseStore;
        private IList<int> _phraseIDs;
        private IDictionary<int, string> _phrases;
        private SortedDictionary<int, string> _measurements;
        private SortedDictionary<int, string> _abbreviatedMeasurements;

        #endregion

        /// <summary>
        /// Gets the measurements-related phraseIDs.
        /// </summary>
        public List<int> PhraseIDs
        {
            get
            {
                _phraseIDs = new List<int>();
                _phraseIDs.Add(NONE);
                _phraseIDs.Add(FLUID_RATE);
                _phraseIDs.Add(GAS_RATE);
                _phraseIDs.Add(PRESSURE);
                _phraseIDs.Add(TEMPERATURE);
                _phraseIDs.Add(SHORT_LENGTH);
                _phraseIDs.Add(LENGTH);
                _phraseIDs.Add(WEIGHT);
                _phraseIDs.Add(POWER);
                _phraseIDs.Add(BARRELS_PER_DAY);
                _phraseIDs.Add(BARRELS_PER_DAY_ABBR);
                _phraseIDs.Add(CUBIC_METERS);
                _phraseIDs.Add(CUBIC_METERS_ABBR);
                _phraseIDs.Add(FARENHEIT);
                _phraseIDs.Add(CELSIUS);
                _phraseIDs.Add(THOUSAND_CUBIC_FEET);
                _phraseIDs.Add(THOUSAND_CUBIC_FEET_ABBR);
                _phraseIDs.Add(POUNDS_PER_SQUARE_INCH);
                _phraseIDs.Add(POUNDS_PER_SQUARE_INCH_ABBR);
                _phraseIDs.Add(KILO_PASCALS);
                _phraseIDs.Add(KILO_PASCALS_ABBR);
                _phraseIDs.Add(FEET);
                _phraseIDs.Add(FEET_ABBR);
                _phraseIDs.Add(METERS);
                _phraseIDs.Add(METERS_ABBR);
                _phraseIDs.Add(INCHES);
                _phraseIDs.Add(INCHES_ABBR);
                _phraseIDs.Add(CENTIMETERS);
                _phraseIDs.Add(CENTIMETERS_ABBR);
                _phraseIDs.Add(HORSEPOWER);
                _phraseIDs.Add(HORSEPOWER_ABBR);
                _phraseIDs.Add(KILOWATTS);
                _phraseIDs.Add(KILOWATTS_ABBR);
                _phraseIDs.Add(POUNDS);
                _phraseIDs.Add(POUNDS_ABBR);
                _phraseIDs.Add(KILOGRAMS);
                _phraseIDs.Add(KILOGRAMS_ABBR);
                _phraseIDs.Add(GAS_VOLUME);
                _phraseIDs.Add(CHEMICAL_RATE);
                _phraseIDs.Add(CHEMICAL_VOLUME);
                _phraseIDs.Add(FLUID_VOLUME);
                _phraseIDs.Add(OIL_GRAVITY);
                _phraseIDs.Add(SPECIFIC_GRAVITY);
                _phraseIDs.Add(GCM);
                _phraseIDs.Add(API);
                _phraseIDs.Add(OIL_GRAVITY_API);
                _phraseIDs.Add(CELSIUS_ABBR);
                _phraseIDs.Add(FARENHEIT_ABBR);
                _phraseIDs.Add(THOUSAND_CUBIC_METERS);
                _phraseIDs.Add(THOUSAND_CUBIC_METERS_ABBR);
                _phraseIDs.Add(CURRENT);
                _phraseIDs.Add(TORQUE);
                _phraseIDs.Add(FREQUENCY);
                _phraseIDs.Add(ENERGY);
                _phraseIDs.Add(HEAD);
                _phraseIDs.Add(DENSITY);
                _phraseIDs.Add(MINUTES);
                _phraseIDs.Add(DAYS);
                _phraseIDs.Add(VOLTAGE);
                _phraseIDs.Add(RELATIVE_DENSITY);
                _phraseIDs.Add(MONEY);

                return (List<int>)_phraseIDs;
            }
        }

        /// <summary>
        /// Contructs a new instance of the <seealso cref="MeasurementUtility"/> class.
        /// </summary>
        /// <param name="phraseStore">The phrase store.</param>
        public MeasurementUtility(ILocalePhrases phraseStore)
        {
            _phraseStore = phraseStore ?? throw new ArgumentNullException(nameof(phraseStore));
        }

        #region Public Methods

        /// <summary>
        /// Gets the measurement phrases.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A dictionary of int to string containing unit type ids mapped to their measurement phrase strings.</returns>
        public SortedDictionary<int, string> GetMeasurements(string correlationId)
        {
            PopulateMeasurements(correlationId);
            return _measurements;
        }

        /// <summary>
        /// Gets the abbreviated measurement phrases.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A dictionary of int to string containing unit type ids mapped to their abbreviated measurement phrase strings.</returns>
        public SortedDictionary<int, string> GetAbbreviatedMeasurements(string correlationId)
        {
            PopulateAbbreviatedMeasurements(correlationId);
            return _abbreviatedMeasurements;
        }

        #endregion

        private void PopulateAbbreviatedMeasurements(string correlationId)
        {
            _phrases ??= _phraseStore.GetAll(correlationId, PhraseIDs.ToArray());

            _abbreviatedMeasurements = new SortedDictionary<int, string>();

            _abbreviatedMeasurements.Add(0, _phrases[731]); // None

            _abbreviatedMeasurements.Add(1, _phrases[BARRELS_PER_DAY_ABBR]); // Fluid Rate
            _abbreviatedMeasurements.Add(2, _phrases[THOUSAND_CUBIC_FEET_ABBR]); // Gas Rate
            _abbreviatedMeasurements.Add(3, _phrases[POUNDS_PER_SQUARE_INCH_ABBR]); // Pressure
            _abbreviatedMeasurements.Add(4, _phrases[FARENHEIT_ABBR]); // Temperature
            _abbreviatedMeasurements.Add(5, _phrases[INCHES_ABBR]); // Short Length
            _abbreviatedMeasurements.Add(6, _phrases[FEET_ABBR]); // Length
            _abbreviatedMeasurements.Add(7, _phrases[POUNDS_ABBR]); // Weight
            _abbreviatedMeasurements.Add(16, _phrases[HORSEPOWER_ABBR]); // Power
            _abbreviatedMeasurements.Add(18, _phrases[OIL_GRAVITY]); // Oil Gravity

            _abbreviatedMeasurements.Add(8, _phrases[GAS_VOLUME]); // Gas Volume
            _abbreviatedMeasurements.Add(9, _phrases[CURRENT]); // Current
            _abbreviatedMeasurements.Add(10, _phrases[VOLTAGE]); // Voltage
            _abbreviatedMeasurements.Add(11, _phrases[RELATIVE_DENSITY]); // Relative Density
            _abbreviatedMeasurements.Add(12, _phrases[TORQUE]); // Torque
            _abbreviatedMeasurements.Add(13, _phrases[MONEY]); // Money
            _abbreviatedMeasurements.Add(14, _phrases[FREQUENCY]); // Frequency
            _abbreviatedMeasurements.Add(15, _phrases[ENERGY]); // Energy
            _abbreviatedMeasurements.Add(17, _phrases[FLUID_VOLUME]); // Fluid Volume
            _abbreviatedMeasurements.Add(19, _phrases[HEAD]); // Head
            _abbreviatedMeasurements.Add(20, _phrases[CHEMICAL_RATE]); // Chemical Rate
            _abbreviatedMeasurements.Add(21, _phrases[CHEMICAL_VOLUME]); // Chemical Volume
            _abbreviatedMeasurements.Add(22, _phrases[DENSITY]); // Density
            _abbreviatedMeasurements.Add(23, _phrases[MINUTES]); // Minutes
            _abbreviatedMeasurements.Add(24, _phrases[DAYS]); // Days
        }

        private void PopulateMeasurements(string correlationId)
        {
            _phrases ??= _phraseStore.GetAll(correlationId, PhraseIDs.ToArray());

            _measurements = new SortedDictionary<int, string>();

            _measurements.Add(0, _phrases[731]); // None

            _measurements.Add(1, _phrases[BARRELS_PER_DAY]); // Fluid Rate
            _measurements.Add(2, _phrases[THOUSAND_CUBIC_FEET]); // Gas Rate
            _measurements.Add(3, _phrases[POUNDS_PER_SQUARE_INCH]); // Pressure
            _measurements.Add(4, _phrases[FARENHEIT]); // Temperature
            _measurements.Add(5, _phrases[INCHES]); // Short Length
            _measurements.Add(6, _phrases[FEET]); // Length
            _measurements.Add(7, _phrases[POUNDS]); // Weight
            _measurements.Add(16, _phrases[HORSEPOWER]); // Power
            _measurements.Add(18, _phrases[OIL_GRAVITY]); // Oil Gravity

            _measurements.Add(8, _phrases[GAS_VOLUME]); // Gas Volume
            _measurements.Add(9, _phrases[CURRENT]); // Current
            _measurements.Add(10, _phrases[VOLTAGE]); // Voltage
            _measurements.Add(11, _phrases[RELATIVE_DENSITY]); // Relative Density
            _measurements.Add(12, _phrases[TORQUE]); // Torque
            _measurements.Add(13, _phrases[MONEY]); // Money
            _measurements.Add(14, _phrases[FREQUENCY]); // Frequency
            _measurements.Add(15, _phrases[ENERGY]); // Energy
            _measurements.Add(17, _phrases[FLUID_VOLUME]); // Fluid Volume
            _measurements.Add(19, _phrases[HEAD]); // Head
            _measurements.Add(20, _phrases[CHEMICAL_RATE]); // Chemical Rate
            _measurements.Add(21, _phrases[CHEMICAL_VOLUME]); // Chemical Volume
            _measurements.Add(22, _phrases[DENSITY]); // Density
            _measurements.Add(23, _phrases[MINUTES]); // Minutes
            _measurements.Add(24, _phrases[DAYS]); // Days
        }

    }
}
