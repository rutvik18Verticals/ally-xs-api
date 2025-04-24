using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a value of a finite set.
    /// </summary>
    public abstract class EnhancedEnumBase
    {

        #region Static Fields

        private static readonly IDictionary<Type, IDictionary<int, EnhancedEnumBase>> _dictionary =
            new Dictionary<Type, IDictionary<int, EnhancedEnumBase>>();

        private static readonly IDictionary<Type, IDictionary<string, PropertyInfo>> _properties =
            new Dictionary<Type, IDictionary<string, PropertyInfo>>();

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the name that represents an unsupported value.
        /// </summary>
        protected static Text UnsupportedName { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the key.
        /// </summary>
        public int Key { get; protected set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public Text Name { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the value is supported.
        /// </summary>
        public bool IsSupported { get; protected set; } = true;

        #endregion

        #region Constructors

        static EnhancedEnumBase()
        {
            UnsupportedName = new Text("Unsupported");
        }

        /// <summary>
        /// Initializes a new EnhancedEnumBase with a specified key and name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">name is null.</exception>
        public EnhancedEnumBase(int key, Text name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Key = key;
            Name = name;
        }

        #endregion

        #region Static Methods

        #region Public

        /// <summary>
        /// Sets the name that will be used to represent unsupported values.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">name is null.</exception>
        public static void SetUnsupportedName(Text name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            UnsupportedName = name;
        }

        /// <summary>
        /// Returns an IList containing all the values registered for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get values for; must be an EnhancedEnumBase.</typeparam>
        /// <returns>An IList containing all the values registered to type T.</returns>
        public static IList<T> GetValues<T>()
            where T : EnhancedEnumBase
        {
            IList<T> result = null;
            var values = GetDictionary<T>();

            if (values != null)
            {
                //synchronize access
                lock (values)
                {
                    result = values.Values.Cast<T>().ToList();
                }
            }

            return result ?? new List<T>();
        }

        /// <summary>
        /// Gets the value for the specifed type and key.
        /// </summary>
        /// <typeparam name="T">The type to get the value for; must be an EnhancedEnumBase.</typeparam>
        /// <param name="key">The key of the value.</param>
        /// <returns>
        /// The value found for the specified type and key; otherwise, a new value using UnsupportedName.
        /// </returns>
        /// <exception cref="MissingMethodException">
        /// The specified type does not contain a constructor that takes a key and a name.
        /// </exception>
        public static T GetValue<T>(int key)
            where T : EnhancedEnumBase
        {
            T result = null;
            var values = GetDictionary<T>();

            if (values != null)
            {
                EnhancedEnumBase temp = null;

                //synchronize access
                lock (values)
                {
                    if (values.TryGetValue(key, out temp))
                    {
                        result = (T)temp;
                    }
                }
            }

            return result ?? CreateInternal<T>(key, UnsupportedName, false);
        }

        /// <summary>
        /// Determines whether a specified key is defined.
        /// </summary>
        /// <typeparam name="T">The type that the key belongs to; must be an <see cref="EnhancedEnumBase"/>.</typeparam>
        /// <param name="key">The key to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is defined; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDefined<T>(int key)
            where T : EnhancedEnumBase
        {
            var result = false;

            var values = GetDictionary<T>();

            if (values != null)
            {
                //synchronize access
                lock (values)
                {
                    result = values.ContainsKey(key);
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the name for a specified type and key.
        /// </summary>
        /// <typeparam name="T">The type that the key belongs to; must be an EnhancedEnumBase.</typeparam>
        /// <param name="key">The key to set the name for.</param>
        /// <param name="name">The name to associate with the value.</param>
        /// <returns>true if the specified key was found; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">name is null.</exception>
        public static bool SetName<T>(int key, Text name)
            where T : EnhancedEnumBase
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var success = false;
            var values = GetDictionary<T>();

            if (values != null)
            {
                EnhancedEnumBase value = null;

                //synchronize access to the dictionary
                lock (values)
                {
                    success = values.TryGetValue(key, out value);
                }

                if (success)
                {
                    //synchronize access to the value
                    lock (value)
                    {
                        value.Name = name;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Converts the string representation of the name of a value to its equivalent value.
        /// </summary>
        /// <typeparam name="T">
        /// The type that the name belongs to; must be of type <see cref="EnhancedEnumBase"/>.
        /// </typeparam>
        /// <param name="name">The name of the value.</param>
        /// <returns>The value represented by the specifed name if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> is either an empty string or only contains white space.
        /// </exception>
        public static T Parse<T>(string name)
            where T : EnhancedEnumBase
        {
            return (T)Parse(typeof(T), name);
        }

        /// <summary>
        /// Converts the string representation of the name of a value to its equivalent value.
        /// </summary>
        /// <param name="type">The type that the name belongs to.</param>
        /// <param name="name">The name of the value.</param>
        /// <returns>The value represented by the specifed name if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is <c>null</c>
        /// OR
        /// <paramref name="name"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> is not a subclass of <see cref="EnhancedEnumBase"/>
        /// OR
        /// <paramref name="name"/> is either an empty string or only contains white space.
        /// </exception>
        public static EnhancedEnumBase Parse(Type type, string name)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!typeof(EnhancedEnumBase).IsAssignableFrom(type))
            {
                throw new ArgumentException($"{nameof(type)} must be a subclass of EnhancedEnumBase.", nameof(type));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} cannot be empty or only contain white space.", nameof(name));
            }

            EnhancedEnumBase result = null;
            GetProperties(type).TryGetValue(name, out var property);

            if (property != null)
            {
                result = (EnhancedEnumBase)property.GetValue(null);
            }

            return result;
        }

        /// <summary>
        /// Converts the string representation of the name of a value to its equivalent value.
        /// </summary>
        /// <param name="type">The type that the name belongs to.</param>
        /// <param name="name">The name of the value.</param>
        /// <param name="result">The return enum value.</param>
        /// <returns>The value represented by the specifed name if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is <c>null</c>
        /// OR
        /// <paramref name="name"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> is not a subclass of <see cref="EnhancedEnumBase"/>
        /// OR
        /// <paramref name="name"/> is either an empty string or only contains white space.
        /// </exception>
        public static bool TryParse(Type type, string name, out EnhancedEnumBase result)
        {
            bool returnValue = false;
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!typeof(EnhancedEnumBase).IsAssignableFrom(type))
            {
                throw new ArgumentException($"{nameof(type)} must be a subclass of EnhancedEnumBase.", nameof(type));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} cannot be empty or only contain white space.", nameof(name));
            }
            result = null;
            try
            {
                GetProperties(type).TryGetValue(name, out var property);

                if (property != null)
                {
                    result = (EnhancedEnumBase)property.GetValue(null);
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }

        /// <summary>
        /// Returns the string identifier of all values for a specified type.
        /// </summary>
        /// <typeparam name="T">
        /// The type to get the string identifiers for; must be of type <see cref="EnhancedEnumBase"/>.
        /// </typeparam>
        /// <returns>The string identifier of all values for a specified type.</returns>
        public static IList<string> GetStringIdentifiers<T>()
            where T : EnhancedEnumBase
        {
            return GetStringIdentifiers(typeof(T));
        }

        /// <summary>
        /// Returns the string identifier of all values for a specified type.
        /// </summary>
        /// <param name="type">The type to get the string identifiers for.</param>
        /// <returns>The string identifier of all values for a specified type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> is not a subclass of <see cref="EnhancedEnumBase"/>.
        /// </exception>
        public static IList<string> GetStringIdentifiers(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!typeof(EnhancedEnumBase).IsAssignableFrom(type))
            {
                throw new ArgumentException($"{nameof(type)} must be a subclass of EnhancedEnumBase.", nameof(type));
            }

            return GetProperties(type).Keys.ToList();
        }

        /// <summary>
        /// Gets the EnhancedEnumBase.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>A dictionary of Id and EnhancedEnumBase.</returns>
        public static IDictionary<int, EnhancedEnumBase> GetDictionary<T>()
        {
            return GetDictionary(typeof(T));
        }

        #endregion

        #region Protected

        /// <summary>
        /// Creates an instance of the specified type and registers it.
        /// </summary>
        /// <typeparam name="T">The type to create; must be an EnhancedEnumBase.</typeparam>
        /// <param name="key">The key to assign to the instance.</param>
        /// <param name="name">The name to assign to the instance.</param>
        /// <returns>A new instance of type <typeparamref name="T"/> with the Key and Name set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="MissingMethodException">
        /// The specified type does not contain a constructor that takes a key and a name.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// A value with the same key already exists for the specified type.
        /// </exception>
        protected static T Create<T>(int key, string name)
            where T : EnhancedEnumBase
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return Create<T>(key, Text.FromString(name));
        }

        /// <summary>
        /// Creates an instance of the specified type and registers it.
        /// </summary>
        /// <typeparam name="T">The type to create; must be an EnhancedEnumBase.</typeparam>
        /// <param name="key">The key to assign to the instance.</param>
        /// <param name="name">The name to assign to the instance.</param>
        /// <returns>A new instance of type T with the Key and Name set.</returns>
        /// <exception cref="ArgumentNullException">name is null.</exception>
        /// <exception cref="MissingMethodException">
        /// The specified type does not contain a constructor that takes a key and a name.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// A value with the same key already exists for the specified type.
        /// </exception>
        protected static T Create<T>(int key, Text name)
            where T : EnhancedEnumBase
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var result = CreateInternal<T>(key, name);

            Register(result);

            return result;
        }

        /// <summary>
        /// Registers a value to be managed by EnhancedEnumBase.
        /// </summary>
        /// <param name="value">The value to register.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentException">
        /// A value with the same key already exists for the value's type.
        /// </exception>
        protected static void Register(EnhancedEnumBase value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var type = value.GetType();
            var values = GetDictionary(type);

            if (values == null)
            {
                values = new SortedDictionary<int, EnhancedEnumBase>(); //sort based on keys

                //synchronize access
                lock (_dictionary)
                {
                    _dictionary[type] = values;
                }
            }

            //synchronize access
            lock (values)
            {
                values.Add(value.Key, value);
            }
        }

        #endregion

        #region Private

        private static IDictionary<int, EnhancedEnumBase> GetDictionary(Type type)
        {
            IDictionary<int, EnhancedEnumBase> result = null;

            //synchronize access
            lock (_dictionary)
            {
                _dictionary.TryGetValue(type, out result);
            }

            if (result == null)
            {
                //Create an instance in case the static constructor wasn't called yet
                CreateInternal(type, 0, Text.Empty);

                //synchronize access
                lock (_dictionary)
                {
                    _dictionary.TryGetValue(type, out result);
                }
            }

            return result;
        }

        private static T CreateInternal<T>(int key, Text name)
        {
            return CreateInternal<T>(key, name, true);
        }

        private static T CreateInternal<T>(int key, Text name, bool isSupported)
        {
            return (T)CreateInternal(typeof(T), key, name, isSupported);
        }

        private static object CreateInternal(Type type, int key, Text name)
        {
            return CreateInternal(type, key, name, true);
        }

        private static object CreateInternal(Type type, int key, Text name, bool isSupported)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var value = Activator.CreateInstance(type, flags, null, new object[]
            {
                key, name
            }, null);

            ((EnhancedEnumBase)value).IsSupported = isSupported;

            return value;
        }

        private static IDictionary<string, PropertyInfo> GetProperties(Type type)
        {
            IDictionary<string, PropertyInfo> result = null;

            //synchronize access
            lock (_properties)
            {
                _properties.TryGetValue(type, out result);
            }

            if (result == null)
            {
                result = new SortedDictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static))
                {
                    if (!Regex.IsMatch(property.Name, "^Default$", RegexOptions.IgnoreCase))
                    {
                        result[property.Name] = property;
                    }
                }

                //synchronize access
                lock (_properties)
                {
                    _properties[type] = result;
                }
            }

            return result;
        }

        #endregion

        #endregion

        #region Overridden Object Members

        /// <summary>
        /// Returns the string representation of the value of the instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return Name.ToString();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>true if this instance is equal to obj; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            var value = obj as EnhancedEnumBase;

            if (value == null)
            {
                return false;
            }

            return GetType().Equals(value.GetType()) && Key.Equals(value.Key);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code generated.</returns>
        public override int GetHashCode()
        {
            return MathUtility.GenerateHashCode(GetType(), Key);
        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Implicitly casts an EnhancedEnumBase to a string
        /// </summary>
        /// <param name="enumValue">The enumValue to cast</param>
        /// <returns>The string representation of enumValue</returns>
        public static implicit operator string(EnhancedEnumBase enumValue)
        {
            if (enumValue is null)
            {
                return null;
            }

            return enumValue.ToString();
        }

        /// <summary>
        /// Indicates whether two enhanced enums are equal.
        /// </summary>
        /// <param name="value1">A value.</param>
        /// <param name="value2">Another value.</param>
        /// <returns>true if value1 equals value2; otherwise, false.</returns>
        public static bool operator ==(EnhancedEnumBase value1, EnhancedEnumBase value2)
        {
            return Equals(value1, value2);
        }

        /// <summary>
        /// Indicates whether two enhanced enums are not equal.
        /// </summary>
        /// <param name="value1">A value.</param>
        /// <param name="value2">Another value.</param>
        /// <returns>true if value1 does not equal value2; otherwise, false.</returns>
        public static bool operator !=(EnhancedEnumBase value1, EnhancedEnumBase value2)
        {
            return !Equals(value1, value2);
        }

        #endregion

    }
}
