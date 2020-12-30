using System;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Stize.DotNet.Json.Converter
{
    public abstract class NumberToStringConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (this.TryParse(span, out T number, out var bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                if (this.TryParse(reader.GetString(), out number))
                {
                    return number;
                }
            }

            return this.GetValue(reader);
        }

        protected abstract bool TryParse(in ReadOnlySpan<byte> span, out T number, out int bytesConsumed);
        protected abstract bool TryParse(string value, out T number);
        protected abstract T GetValue(Utf8JsonReader reader);


        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}