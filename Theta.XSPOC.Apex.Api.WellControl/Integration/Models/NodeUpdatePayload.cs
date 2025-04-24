using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// The contract to update node data.
    /// </summary>
    public class NodeUpdatePayload
    {
        /// <summary>
        /// The UTCTimeStamp..
        /// </summary>
        public DateTime UTCTimeStamp { get; set; }

        /// <summary>
        /// The Key.
        /// </summary>
        public List<Key> Key { get; set; }

        /// <summary>
        /// The Data.
        /// </summary>
        public List<Datum> Data { get; set; }

    }

    /// <summary>
    /// The Key update node data.
    /// </summary>
    public class Key
    {
        /// <summary>
        /// The Column.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// The Value.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// The Datum to update node data.
    /// </summary>
    public class Datum
    {
        /// <summary>
        /// The Column.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// The Value.
        /// </summary>
        public object Value { get; set; }
    }
}
