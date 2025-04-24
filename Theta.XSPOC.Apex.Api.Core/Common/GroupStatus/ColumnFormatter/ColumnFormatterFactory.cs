using System;
using System.Collections.Generic;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Factory class for creating column formatters.
    /// </summary>
    public class ColumnFormatterFactory : IColumnFormatterFactory
    {

        #region Private Fields

        private readonly IEnumerable<IColumnFormatter> _groupStatusColumnFormat;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnFormatterFactory"/> class.
        /// </summary>
        /// <param name="groupStatusColumnFormat">The collection of column formatters.</param>
        public ColumnFormatterFactory(IEnumerable<IColumnFormatter> groupStatusColumnFormat)
        {
            _groupStatusColumnFormat =
                groupStatusColumnFormat ?? throw new ArgumentNullException(nameof(groupStatusColumnFormat));
        }

        #endregion

        #region IColumnFormatterFactory Members

        /// <summary>
        /// Creates a column formatter based on the source Id and name.
        /// </summary>
        /// <param name="sourceId">The source Id.</param>
        /// <param name="name">The name of the column formatter.</param>
        /// <returns>The created column formatter.</returns>
        public IColumnFormatter Create(int sourceId, string name)
        {
            switch (sourceId)
            {
                case (int)GroupStatusColumns.SourceType.Common:

                    return CreateForCommonAndFormat(name);

                //switch (name.ToUpper())
                //{
                //    case "%COM":
                //    case "TBLNODEMASTER.COMMSUCCESS":
                //        break;
                //    case "%FILL":
                //        break;
                //    case "%RT":
                //        break;
                //    case "%RTY":
                //        break;
                //    case "ALARMS":
                //        break;
                //    case "CAMERAALARMS":
                //        break;
                //    case "COMM":
                //        break;
                //    case "CONTROLLER":
                //    case "POCTYPE":
                //    case "POC TYPE":
                //        break;
                //    case "DRC":
                //        break;
                //    case "ENBLD":
                //        break;
                //    case "EXCEPTIONGROUPNAME":
                //        break;
                //    case "HOSTALARMS":
                //        break;
                //    case "FACILITYTAGALARMS":
                //        break;
                //    case "PUMPING UNIT":
                //        break;
                //    case "PUMPING UNIT MANUFACTURER":
                //        break;
                //    case "ROD GRADE":
                //        break;
                //    case "RUN STATUS":
                //        break;
                //    case "STRINGID":
                //        break;
                //    case "TIS":
                //        break;
                //    case "WELL":
                //        break;
                //    default:
                //        break;
                //}
                case (int)GroupStatusColumns.SourceType.Parameter:
                    return CreateForParameterAndFormat();
                case (int)GroupStatusColumns.SourceType.Facility:
                    return CreateForFacilityAndFormat();
                case (int)GroupStatusColumns.SourceType.ParamStandard:
                    return CreateForParamStandardAndFormat();
                case (int)GroupStatusColumns.SourceType.Formula:
                    break;
                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// Creates a column formatter based on the source Id, name, and conditional formats.
        /// </summary>
        /// <param name="sourceId">The source Id.</param>
        /// <param name="name">The name of the column formatter.</param>
        /// <param name="conditionalFormats">The list of conditional formats.</param>
        /// <returns>The created column formatter.</returns>
        public IColumnFormatter Create(int sourceId, string name, IList<ConditionalFormat> conditionalFormats)
        {
            if (conditionalFormats?.Count > 0)
            {
                return CreateForCommonAndFormat("CONDITIONAL");
            }

            return Create(sourceId, name);
        }

        #endregion

        #region Private Methods

        private IColumnFormatter CreateForCommonAndFormat(string name)
        {
            var columnFormat = _groupStatusColumnFormat.ToList().FirstOrDefault(x => x.Responsibilities.Contains(name.ToUpper()));

            return columnFormat;
        }

        private IColumnFormatter CreateForParamStandardAndFormat()
        {
            var columnFormat = _groupStatusColumnFormat.FirstOrDefault(x => x.Responsibilities.Contains("PARAMSTANDARD"));
            return columnFormat;
        }

        private IColumnFormatter CreateForFacilityAndFormat()
        {
            var columnFormat = _groupStatusColumnFormat.FirstOrDefault(x => x.Responsibilities.Contains("FACILITY"));

            return columnFormat;
        }

        private IColumnFormatter CreateForParameterAndFormat()
        {
            var columnFormat = _groupStatusColumnFormat.ToList().FirstOrDefault(x => x.Responsibilities.Contains("PARAMETER"));

            return columnFormat;
        }

        #endregion

    }
}
