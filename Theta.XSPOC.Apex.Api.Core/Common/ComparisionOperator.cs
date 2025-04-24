using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a comparison operator.
    /// </summary>
    public class ComparisonOperator : EnhancedEnumBase
    {

        #region Enums

        /// <summary>
        /// Represents a comparison operator.
        /// </summary>
        public enum ComparisonOperatorType
        {

            /// <summary>
            /// The numeric equal type field.
            /// </summary>
            Equal = 1,

            /// <summary>
            /// The numeric not equal type field.
            /// </summary>
            NotEqual = 2,

            /// <summary>
            /// The numeric less than type field.
            /// </summary>
            LessThan = 3,

            /// <summary>
            /// The numeric greater than type field.
            /// </summary>
            GreaterThan = 4,

            /// <summary>
            /// The numeric less than or equal type field.
            /// </summary>
            LessThanOrEqual = 5,

            /// <summary>
            /// The numeric greater than or equal type field.
            /// </summary>
            GreaterThanOrEqual = 6,

            /// <summary>
            /// The numeric between type field.
            /// </summary>
            Between = 7,

            /// <summary>
            /// The numeric not between type field.
            /// </summary>
            NotBetween = 8,

            /// <summary>
            /// The alpha equal type field.
            /// </summary>
            StringEqual = 9,

            /// <summary>
            /// The alpha contains type field.
            /// </summary>
            Contains = 10,

            /// <summary>
            /// The alpha starts with type field.
            /// </summary>
            StartsWith = 11,

            /// <summary>
            /// The alpha ends with type field.
            /// </summary>
            EndsWith = 12,

            /// <summary>
            /// The alpha does not equal type field.
            /// </summary>
            DoesNotEqual = 13,

            /// <summary>
            /// The alpha does not contain type field.
            /// </summary>
            DoesNotContain = 14,

            /// <summary>
            /// The alpha does not start with type field.
            /// </summary>
            DoesNotStartWith = 15,

            /// <summary>
            /// The alpha does not end with type field.
            /// </summary>
            DoesNotEndWith = 16,

        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Specifies that the comparison operator is equal.
        /// </summary>
        public static ComparisonOperator Equal { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.Equal, Text.FromString("="));

        /// <summary>
        /// Specifies that the comparison operator is not equal.
        /// </summary>
        public static ComparisonOperator NotEqual { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.NotEqual, Text.FromString("<>"));

        /// <summary>
        /// Specifies that the comparison operator is less than.
        /// </summary>
        public static ComparisonOperator LessThan { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.LessThan, Text.FromString("<"));

        /// <summary>
        /// Specifies that the comparison operator is greater than.
        /// </summary>
        public static ComparisonOperator GreaterThan { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.GreaterThan, Text.FromString(">"));

        /// <summary>
        /// Specifies that the comparison operator is less than or equal.
        /// </summary>
        public static ComparisonOperator LessThanOrEqual { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.LessThanOrEqual, Text.FromString("<="));

        /// <summary>
        /// Specifies that the comparison operator is greater than or equal.
        /// </summary>
        public static ComparisonOperator GreaterThanOrEqual { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.GreaterThanOrEqual, Text.FromString(">="));

        /// <summary>
        /// Specifies that the comparison operator is between.
        /// </summary>
        public static ComparisonOperator Between { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.Between, Text.FromString("Between"));

        /// <summary>
        /// Specifies that the comparison operator is not between.
        /// </summary>
        public static ComparisonOperator NotBetween { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.NotBetween, Text.FromString("Not between"));

        /// <summary>
        /// Specifies that the string comparison operator is equals.
        /// </summary>
        public static ComparisonOperator StringEqual { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.StringEqual, Text.FromString("Equals"));

        /// <summary>
        /// Specifies that the string comparison operator is contains.
        /// </summary>
        public static ComparisonOperator Contains { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.Contains, Text.FromString("Contains"));

        /// <summary>
        /// Specifies that the string comparison operator is starts with.
        /// </summary>
        public static ComparisonOperator StartsWith { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.StartsWith, Text.FromString("Starts with"));

        /// <summary>
        /// Specifies that the string comparison operator is ends with.
        /// </summary>
        public static ComparisonOperator EndsWith { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.EndsWith, Text.FromString("Ends with"));

        /// <summary>
        /// Specifies that the string comparison operator is not equals.
        /// </summary>
        public static ComparisonOperator DoesNotEqual { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.DoesNotEqual, Text.FromString("Does not equal"));

        /// <summary>
        /// Specifies that the string comparison operator is not contains.
        /// </summary>
        public static ComparisonOperator DoesNotContain { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.DoesNotContain, Text.FromString("Does not contain"));

        /// <summary>
        /// Specifies that the string comparison operator is not starts with.
        /// </summary>
        public static ComparisonOperator DoesNotStartWith { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.DoesNotStartWith, Text.FromString("Does not start with"));

        /// <summary>
        /// Specifies that the string comparison operator is not ends with.
        /// </summary>
        public static ComparisonOperator DoesNotEndWith { get; } =
            Create<ComparisonOperator>((int)ComparisonOperatorType.DoesNotEndWith, Text.FromString("Does not end with"));

        /// <summary>
        /// Gets the default value
        /// </summary>
        public static ComparisonOperator Default { get; } = Equal;

        /// <summary>
        /// Gets the count of values that this operator requires value.
        /// </summary>
        public static int ValueCount(int operatorKey)
        {
            var valueCount = operatorKey switch
            {
                (int)ComparisonOperatorType.StringEqual or (int)ComparisonOperatorType.Contains
                    or (int)ComparisonOperatorType.StartsWith or (int)ComparisonOperatorType.EndsWith
                    or (int)ComparisonOperatorType.DoesNotEqual or (int)ComparisonOperatorType.DoesNotContain
                    or (int)ComparisonOperatorType.DoesNotStartWith or (int)ComparisonOperatorType.DoesNotEndWith => 0,
                (int)ComparisonOperatorType.Between or (int)ComparisonOperatorType.NotBetween => 2,
                _ => 1,
            };
            return valueCount;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new ComparisonOperator with a specified key and name.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="name">The name</param>
        public ComparisonOperator(int key, Text name)
            : base(key, name)
        {
        }

        #endregion

    }
}
