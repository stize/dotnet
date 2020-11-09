using System;
using System.Buffers.Text;
using System.Text.Json;

namespace Stize.DotNet.Json.Converter
{
    public class DecimalToStringConverter : NumberToStringConverter<decimal>
    {

        protected override bool TryParse(in ReadOnlySpan<byte> span, out decimal number, out int bytesConsumed)
        {
            return Utf8Parser.TryParse(span, out number, out bytesConsumed) && span.Length == bytesConsumed;
        }

        protected override bool TryParse(string value, out decimal number)
        {
            return decimal.TryParse(value, out number);
        }

        protected override decimal GetValue(Utf8JsonReader reader)
        {
            return reader.GetDecimal();
        }

    }
}