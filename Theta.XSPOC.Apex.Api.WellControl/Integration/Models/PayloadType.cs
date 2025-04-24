namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Defines the payload type.
    /// </summary>
    public enum PayloadType
    {

        /// <summary>
        /// The payload type is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Device scan payload.
        /// </summary>
        DeviceScanData = 1,

        /// <summary>
        /// Well action payload.
        /// </summary>
        WellAction = 2,

        /// <summary>
        /// Update poc data payload.
        /// </summary>
        UpdatePocData = 3,

        /// <summary>
        /// Get card payload.
        /// </summary>
        GetCard = 4,

        /// <summary>
        /// Download equipment payload.
        /// </summary>
        DownloadEquipment = 5,

        /// <summary>
        /// Upload equipment payload.
        /// </summary>
        UploadEquipment = 6,

        /// <summary>
        /// Get data payload.
        /// </summary>
        GetData = 9,

        /// <summary>
        /// Get Card Direct
        /// </summary>
        GetCardDirect = 10,

        /// <summary>
        /// Communication Log
        /// </summary>
        CommunicationLog = 11,

        /// <summary>
        /// Capture Register Log
        /// </summary>
        CaptureRegisterLog = 12,

        /// <summary>
        /// On off cycles payload
        /// </summary>
        OnOffCycles = 13,

    }
}