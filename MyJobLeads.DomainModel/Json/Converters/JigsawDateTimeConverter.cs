using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MyJobLeads.DomainModel.Json.Converters
{
    /// <summary>
    /// Class to return Jigsaw dates (2009-09-12 14:37:49 PDT) into a DateTime
    /// </summary>
    public class JigsawDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType != typeof(DateTime))
                return false;

            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Format expected is 2009-09-12 14:37:49 PDT

            // Remove letters at the end
            string strValue = (string)reader.Value;
            for (int x = strValue.Length - 1; x >= 0; x--)
            {
                if (!char.IsLetter(strValue[x]))
                    break;

                strValue = strValue.Substring(0, x);
            }

            DateTime value;
            if (!DateTime.TryParse(strValue, out value))
                throw new InvalidOperationException(
                    string.Format("The json \"{0}\" could not be converted to a DateTime", reader.Value));

            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
