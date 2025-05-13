using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Graph Data model
    /// </summary>
    public class DataPointModelDto
    {
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or Sets UOM
        /// </summary>
        public string UOM { get; set; }

        /// <summary>
        /// Gets or Sets Short_UOM
        /// </summary>
        public string Short_UOM { get; set; }

        /// <summary>
        /// Gets or Sets ParamTypeId
        /// </summary>
        public string ParamTypeId { get; set; }

        /// <summary>
        /// Gets or Sets Address
        /// </summary>
        public string Address { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DataPointsModelDto
    {
        /// <summary>
        /// Gets or Sets TrendName
        /// </summary>
        public string TrendName { get; set; }

        /// <summary>
        /// Gets or Sets UnitOfMeasure
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Gets or Sets Short_UnitOfMeasure
        /// </summary>
        public string Short_UnitOfMeasure { get; set; }

        /// <summary>
        /// Gets or Sets ParamTypeId
        /// </summary>
        public string ParamTypeId { get; set; }

        /// <summary>
        /// Gets or Sets Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or Sets Max
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        /// Gets or Sets MaxThreshold
        /// </summary>
        public string MaxThreshold { get; set; }

        /// <summary>
        /// Gets or Sets MinThreshold
        /// </summary>
        public string MinThreshold { get; set; }

        /// <summary>
        /// Gets or Sets Min
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// Gets or Sets Median
        /// </summary>
        public string Median { get; set; }

        /// <summary>
        /// Gets or Sets Displayorder
        /// </summary>
        public int? Displayorder { get; set; }

        /// <summary>
        /// Gets or Sets DataPoints
        /// </summary>
        public List<DataPointModelDto> DataPoints { get; set; }

        /// <summary>
        /// Gets or Sets DataPoints
        /// </summary>
        public List<DataPointModelDto> MinThresholdValues { get; set; }

        /// <summary>
        /// Gets or Sets DataPoints
        /// </summary>
        public List<DataPointModelDto> MaxThresholdValues { get; set; }
    }
}
