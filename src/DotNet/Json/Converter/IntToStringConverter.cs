using System;
using System.Buffers.Text;
using System.Text.Json;

namespace Stize.DotNet.Json.Converter
{
    public class IntToStringConverter : NumberToStringConverter<int>
    {

        protected override bool TryParse(in ReadOnlySpan<byte> span, out int number, out int bytesConsumed)
        {
            return Utf8Parser.TryParse(span, out number, out bytesConsumed) && span.Length == bytesConsumed;
        }

        protected override bool TryParse(string value, out int number)
        {
            return int.TryParse(value, out number);
        }

        protected override int GetValue(Utf8JsonReader reader)
        {
            return reader.GetInt32();
        }

    }
}