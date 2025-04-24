using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Theta.XSPOC.Apex.Api.Common.Converters
{

    /// <summary>
    /// DateTime Format .
    /// </summary>
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {

        /// <summary>
        /// To Customize dateTime format .
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            var date = (DateTime)value;
            if (date.TimeOfDay == TimeSpan.Zero)
            {
                writer.WriteValue(date.ToString("MM/dd/yyyy"));
            }
            else
            {
                writer.WriteValue(date.ToString("MM/dd/yyyy h:mm:ss tt"));
            }
        }
    }
}
