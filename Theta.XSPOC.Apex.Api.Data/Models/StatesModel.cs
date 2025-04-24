namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The represents states model.
    /// </summary>
    public class StatesModel
    {

        /// <summary>
        /// The state id.
        /// </summary>
        public int StateId { get; set; }

        /// <summary>
        /// The value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// The text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The back color.
        /// </summary>
        public int? BackColor { get; set; }

        /// <summary>
        /// The alarm priority.
        /// </summary>
        public int? AlarmPriority { get; set; }

        /// <summary>
        /// The locked.
        /// </summary>
        public bool? Locked { get; set; }

        /// <summary>
        /// The fore color.
        /// </summary>
        public int? ForeColor { get; set; }

        /// <summary>
        /// The phrase id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}
