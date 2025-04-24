using System.Collections.Generic;
using System;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the Rod Stress Trend Data.
    /// </summary>
    public class RodStressTrendData : TrendData
    {

        #region Private Members

        private const int BOTTOM_MIN_PHRASEID = 544;
        private const int TOP_MIN_PHRASEID = 545;
        private const int TOP_MAX_PHRASEID = 546;
        private const string BOTTOM_MIN_COLUMN = "BottomMinStress";
        private const string TOP_MIN_COLUMN = "TopMinStress";
        private const string TOP_MAX_COLUMN = "TopMaxStress";

        #endregion

        /// <summary>
        /// Gets or sets the rod number.
        /// </summary>
        public short? RodNumber { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the stress column.
        /// </summary>
        public string StressColumn { get; set; }

        /// <summary>
        /// Gets or sets the stress type id.
        /// </summary>
        public int StressTypeId { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RodStressTrendData" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public RodStressTrendData(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RodStressTrendData" /> class
        /// with RodStressTrendItemModel.
        /// </summary>
        /// <param name="record">The <seealso cref="RodStressTrendItemModel"/> data got from query.</param>
        /// <param name="stressDescription">The stress description.</param>
        /// <param name="stressColumn">The stress column.</param>
        /// <param name="stressTypeId">The stress type id.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="record"/> is null.
        /// </exception>
        public RodStressTrendData(RodStressTrendItemModel record, int stressTypeId, string stressDescription, string stressColumn) :
            base(string.Empty)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.Name = record.Description + stressDescription;
            this.Key = this.Name;
            this.Description = record.Description + stressDescription;
            this.RodNumber = record.RodNum;
            this.Grade = record.Grade;
            this.Diameter = record.Diameter;
            this.StressColumn = stressColumn;
            this.StressTypeId = stressTypeId;
        }

        #endregion

        /// <summary>
        /// Get the trend data for the current Node, RodNumber, Grade, and Diameter within the given date range.
        /// </summary>
        /// <param name="startDate">The beginning date used to read records from tblXDIAGRodResults.</param>
        /// <param name="endDate">The ending date used to read records from tblXDIAGRodResults.</param>
        /// <remarks>
        /// The private variables Node, RodNumber, Grade, and Diameter must be set before this method is called.
        /// </remarks>
        public override IList<DataPoint> GetData(DateTime startDate, DateTime endDate)
        {
            IList<DataPoint> data = new List<DataPoint>();

            return data;
        }

        /// <summary>
        /// Gets the rod stress trend data items.
        /// </summary>
        /// <param name="rodStressTrendData">List of <seealso cref="IList{ControllerTrendItemModel}"/> model object.</param>
        /// <param name="phrases">The locale phrase dictionary.</param>
        /// <returns>An array of <seealso cref="RodStressTrendData"/> items.</returns>
        public static RodStressTrendData[] GetItems(IList<RodStressTrendItemModel> rodStressTrendData,
            IDictionary<int, string> phrases)
        {
            if (phrases == null)
            {
                return null;
            }

            Stress stressBottomMin = new Stress();
            Stress stressTopMin = new Stress();
            Stress stressTopMax = new Stress();

            InitializeStress(ref stressBottomMin, BOTTOM_MIN_PHRASEID, BOTTOM_MIN_COLUMN, phrases);
            InitializeStress(ref stressTopMin, TOP_MIN_PHRASEID, TOP_MIN_COLUMN, phrases);
            InitializeStress(ref stressTopMax, TOP_MAX_PHRASEID, TOP_MAX_COLUMN, phrases);

            var result = new List<RodStressTrendData>();

            if (rodStressTrendData != null && rodStressTrendData.Count > 0)
            {
                foreach (var item in rodStressTrendData)
                {
                    result.Add(new RodStressTrendData(item, stressTopMax.PhraseId, stressTopMax.ColumnName,
                        stressTopMax.ColumnPhrase));
                    result.Add(new RodStressTrendData(item, stressTopMin.PhraseId, stressTopMin.ColumnName,
                        stressTopMin.ColumnPhrase));
                    result.Add(new RodStressTrendData(item, stressBottomMin.PhraseId, stressBottomMin.ColumnName,
                        stressBottomMin.ColumnPhrase));
                }
            }

            return result.ToArray();
        }

        #region Private Methods

        /// <summary>
        /// Initialize the members of the given structure, using the appropriate PhraseID and Column.
        /// </summary>
        /// <param name="stress">The structure to initialize.</param>
        /// <param name="phraseId">The PhraseID associated with the stress type.</param>
        /// <param name="columnName">The tblXDIAGRodResults column associated with the stress type.</param>
        /// <param name="phrases">The locale phrases data.</param>
        /// <remarks>
        /// stress is a Static structure passed byRef, so this should be executed only once per instance.
        /// </remarks>
        private static void InitializeStress(ref Stress stress, int phraseId, string columnName, IDictionary<int, string> phrases)
        {
            if (string.IsNullOrWhiteSpace(stress.ColumnPhrase))
            {
                return;
            }

            stress.PhraseId = phraseId;
            stress.ColumnPhrase = phrases[phraseId];
            stress.ColumnName = columnName;
        }

        #endregion

    }
}