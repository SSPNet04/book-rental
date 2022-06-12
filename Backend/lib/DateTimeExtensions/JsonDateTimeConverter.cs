using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DateTimeExtensions
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        private string _dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
        private string _culture = "en-US";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var enUS = new CultureInfo(_culture);
            writer.WriteStringValue(((DateTime)value).ToString(_dateFormat, enUS));
        }
    }
}
