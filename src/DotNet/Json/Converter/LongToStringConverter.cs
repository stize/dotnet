using System;
using System.Buffers.Text;
using System.Text.Json;

namespace Stize.DotNet.Json.Converter
{
    public class LongToStringConverter : NumberToStringConverter<long>
    {

        protected override bool TryParse(in ReadOnlySpan<byte> span, out long number, out int bytesConsumed)
        {
            return Utf8Parser.TryParse(span, out number, out bytesConsumed) && span.Length == bytesConsumed;
        }

        protected override bool TryParse(string value, out long number)
        {
            return long.TryParse(value, out number);
        }

        protected override long GetValue(Utf8JsonReader reader)
        {
            return reader.GetInt64();
        }

    }
}