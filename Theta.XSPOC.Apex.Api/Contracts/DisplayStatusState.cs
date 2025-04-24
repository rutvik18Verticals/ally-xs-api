namespace Theta.XSPOC.Apex.Api.Contracts
{
    /// <summary>
    /// This will describe the display status state for the UI to determine the color scheme to use.
    /// </summary>
    public enum DisplayStatusState
    {

        /// <summary>
        /// Normal status state to display normal color scheme.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Ok status status state to display the ok color scheme.
        /// </summary>
        Ok = 1,

        /// <summary>
        /// Warning status state to display the warning color scheme.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Error status state to display the error color scheme.
        /// </summary>
        Error = 3,

        /// <summary>
        /// Fatal status state to display the fatal color scheme.
        /// </summary>
        Fatal = 4,

        /// <summary>
        /// Emphasis status state to display the emphasis color scheme.
        /// </summary>
        Emphasis = 5,

    }
}