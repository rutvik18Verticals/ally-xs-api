using System;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the pcsf datalog record class.
    /// </summary>
    public class PCSFDatalogRecord
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the datalog number.
        /// </summary>
        public int DatalogNumber { get; set; }

        /// <summary>
        /// Gets or sets the log date time.
        /// </summary>
        public DateTime LogDateTime { get; set; }

        /// <summary>
        /// Gets or sets the record index.
        /// </summary>
        public int? RecordIndex { get; set; }

        /// <summary>
        /// Gets or sets the value 2.
        /// </summary>
        public float? Value2 { get; set; }

        /// <summary>
        /// Gets or sets the value 3.
        /// </summary>
        public float? Value3 { get; set; }

        /// <summary>
        /// Gets or sets the value 4.
        /// </summary>
        public float? Value4 { get; set; }

        /// <summary>
        /// Gets or sets the value 5.
        /// </summary>
        public float? Value5 { get; set; }

        /// <summary>
        /// Gets or sets the value 6.
        /// </summary>
        public float? Value6 { get; set; }

        /// <summary>
        /// Gets or sets the value 7.
        /// </summary>
        public float? Value7 { get; set; }

        /// <summary>
        /// Gets or sets the value 8.
        /// </summary>
        public float? Value8 { get; set; }

        /// <summary>
        /// Gets or sets the value 9.
        /// </summary>
        public float? Value9 { get; set; }

        /// <summary>
        /// Gets or sets the value 10.
        /// </summary>
        public float? Value10 { get; set; }

        /// <summary>
        /// Gets or sets the value 11.
        /// </summary>
        public float? Value11 { get; set; }

        /// <summary>
        /// Gets or sets the value 12.
        /// </summary>
        public float? Value12 { get; set; }

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="PCSFDatalogRecord"/>.
        /// </summary>
        /// <param name="record">The <seealso cref="PCSFDatalogRecordModel"/> object.</param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="record"/> is null.
        /// </exception>
        public PCSFDatalogRecord(PCSFDatalogRecordModel record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.NodeId = record.NodeId;
            this.DatalogNumber = record.DatalogNumber;
            this.LogDateTime = record.LogDateTime;
            this.RecordIndex = record.RecordIndex.GetValueOrDefault();
            this.Value2 = (float)record.Value2.GetValueOrDefault();
            this.Value3 = (float)record.Value3.GetValueOrDefault();
            this.Value4 = (float)record.Value4.GetValueOrDefault();
            this.Value5 = (float)record.Value5.GetValueOrDefault();
            this.Value6 = (float)record.Value6.GetValueOrDefault();
            this.Value7 = (float)record.Value7.GetValueOrDefault();
            this.Value8 = (float)record.Value8.GetValueOrDefault();
            this.Value9 = (float)record.Value9.GetValueOrDefault();
            this.Value10 = (float)record.Value10.GetValueOrDefault();
            this.Value11 = (float)record.Value11.GetValueOrDefault();
            this.Value12 = (float)record.Value12.GetValueOrDefault();
        }

        #endregion

    }
}
