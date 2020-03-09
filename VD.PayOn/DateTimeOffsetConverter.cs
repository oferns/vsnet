namespace VD.PayOn {

    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal class DateTimeOffsetConverter: JsonConverter<DateTimeOffset> {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            if (typeToConvert != typeof(DateTimeOffset)) { return default; };
            return DateTimeOffset.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.ToString()); // TODO: Check this
        }

    }
}
