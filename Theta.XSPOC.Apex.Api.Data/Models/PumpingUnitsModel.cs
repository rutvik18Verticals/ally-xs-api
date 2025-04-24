namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents pumping units model.
    /// </summary>
    public class PumpingUnitsModel
    {

        /// <summary>
        /// Gets or sets the unique numeric identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unit id. This is maintained for backward compatibility, the Id should be considered the
        /// primary key.
        /// </summary>
        public string UnitID { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer id. 
        /// </summary>
        public string ManufacturerID { get; set; }

        /// <summary>
        /// Gets or sets the API designation. 
        /// </summary>
        public string APIDesignation { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit name. 
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// Gets or sets the OtherInfo value. 
        /// </summary>
        public string OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the number of crank holes
        /// </summary>
        public int? CrankHoles { get; set; }

        /// <summary>
        /// Gets or sets the  stroke 1 value.
        /// </summary>
        public float? Stroke1 { get; set; }

        /// <summary>
        /// Gets or sets the  stroke 2 value.
        /// </summary>
        public float? Stroke2 { get; set; }

        /// <summary>
        /// Gets or sets the  stroke 3 value.
        /// </summary>
        public float? Stroke3 { get; set; }

        /// <summary>
        /// Gets or sets the stroke 4 value. 
        /// </summary>
        public float? Stroke4 { get; set; }

        /// <summary>
        /// Gets or sets the stroke 5 value.
        /// </summary>
        public float? Stroke5 { get; set; }

        /// <summary>
        /// Gets or sets the structural rating.
        /// </summary>
        public float? StructuralRating { get; set; }

        /// <summary>
        /// Gets or sets the gearbox rating.
        /// </summary>
        public float? GearboxRating { get; set; }

        /// <summary>
        /// Gets or sets the maximum stroke.
        /// </summary>
        public float? MaxStroke { get; set; }

        /// <summary>
        /// Gets or sets the WV_Type.
        /// </summary>
        public string WV_Type { get; set; }

        /// <summary>
        /// Gets or sets the WV_Make.
        /// </summary>
        public string WV_Make { get; set; }

        /// <summary>
        /// Gets or sets the WV_Model.
        /// </summary>
        public string WV_Model { get; set; }

        /// <summary>
        /// Gets or sets the WV_OtherInfo.
        /// </summary>
        public string WV_OtherInfo { get; set; }

        /// <summary>
        /// Gets or sets the Dimensions. 
        /// </summary>
        public string Dimensions { get; set; }

    }
}
