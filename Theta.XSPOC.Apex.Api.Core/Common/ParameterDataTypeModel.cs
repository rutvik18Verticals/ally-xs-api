namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the ParameterDataTypeModel.
    /// </summary>
    public class ParameterDataTypeModel
    {

        /// <summary>
        /// ParameterDataTypeModel constructor.
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="description"></param>
        public ParameterDataTypeModel(byte dataType, string description)
        {
            DataType = dataType;
            Description = description;
        }

        /// <summary>
        /// Gets and sets the DataType.
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// Gets and sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets and sets the IsNumeric.
        /// </summary>
        public bool IsNumeric => DataType switch
        {
            (int)UnitCategory.Type.ABTimer or (int)UnitCategory.Type.BCD or (int)UnitCategory.Type.Discrete
                or (int)UnitCategory.Type.FloatEnron or (int)UnitCategory.Type.FloatIEEE or (int)UnitCategory.Type.FloatIEEE_Rev
                or (int)UnitCategory.Type.FloatModicon or (int)UnitCategory.Type.Integer or (int)UnitCategory.Type.LongEnron
                or (int)UnitCategory.Type.LongModicon or (int)UnitCategory.Type.LongPickford or (int)UnitCategory.Type.LongUnico
                or (int)UnitCategory.Type.SignedInteger or (int)UnitCategory.Type.Byte_Int8 => true,
            (int)UnitCategory.Type.BakerDate or (int)UnitCategory.Type.BakerTime or (int)UnitCategory.Type.Date1970
                or (int)UnitCategory.Type.DateEnron or (int)UnitCategory.Type.DJAXDate or (int)UnitCategory.Type.DJAXTime
                or (int)UnitCategory.Type.TimeEnron or (int)UnitCategory.Type.ROC_TLP or (int)UnitCategory.Type.StringType
                or (int)UnitCategory.Type.TotalflowRegister => false,
            _ => false,
        };

    }
}
