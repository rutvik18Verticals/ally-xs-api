using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{

    /// <summary>
    /// Represents the Transactions.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class TransactionsModel
    {

        /// <summary>
        /// Gets or sets the unique identifier for the transaction.
        /// </summary>
        public int TransactionID { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the transaction was requested.
        /// </summary>
        public DateTime DateRequest { get; set; }

        /// <summary>
        /// Gets or sets the PortID associated with the transaction.
        /// </summary>
        public int PortID { get; set; }

        /// <summary>
        /// Gets or sets the task description for the transaction.
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Gets or sets the priority of the transaction.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the source of the transaction (if applicable).
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the NodeID associated with the transaction (if applicable).
        /// </summary>
        public string NodeID { get; set; }

        /// <summary>
        /// Gets or sets the binary data representing the input for the transaction.
        /// </summary>
        public byte[] Input { get; set; }

        /// <summary>
        /// Gets or sets the binary data representing the output of the transaction.
        /// </summary>
        public byte[] Output { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the transaction was processed (if processed).
        /// </summary>
        public DateTime? DateProcess { get; set; }

        /// <summary>
        /// Gets or sets the result of the transaction.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets the textual representation of the input for the transaction.
        /// </summary>
        public string InputText { get; set; }

        /// <summary>
        /// Gets or sets the number of tries attempted for the transaction (if applicable).
        /// </summary>
        public int? Tries { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier used for correlating transactions (if applicable).
        /// </summary>
        public string CorrelationId { get; set; }

    }
}
