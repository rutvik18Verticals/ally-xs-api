using System;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Represents an on demand transaction generated in the system.
    /// </summary>
    public class OnDemandTransaction
    {

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the correlation id.
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Get or sets the date and time the transaction was requested.
        /// </summary>
        public DateTime Requested { get; set; }

        /// <summary>
        /// Gets or sets the port id.
        /// </summary>
        public int PortId { get; set; }

        /// <summary>
        /// Gets or sets the task name.
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the transaction source, for example, the user or system component that initiated it.
        /// </summary>
        public string TransactionSource { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the input. This is typically base64 encoded.
        /// </summary>
        public string EncodedInput { get; set; }

        /// <summary>
        /// Gets or sets the output. This is typically base64 encoded.
        /// </summary>
        public string EncodedOutput { get; set; }

        /// <summary>
        /// Gets or sets the date and time the transaction was processed.
        /// </summary>
        public DateTime Processed { get; set; }

        /// <summary>
        /// Gets or sets the input text. Plaintext.
        /// </summary>
        public string InputText { get; set; }

        /// <summary>
        /// Gets or sets a string describing the result of the transaction, once it was processed.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets the number of retries for this transaction.
        /// </summary>
        public int Tries { get; set; }

    }
}
