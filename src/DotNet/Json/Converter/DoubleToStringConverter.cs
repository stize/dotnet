using System;
using System.Buffers.Text;
using System.Text.Json;

namespace Stize.DotNet.Json.Converter
{
    public class DoubleToStringConverter : NumberToStringConverter<double>
    {

        protected override bool TryParse(in ReadOnlySpan<byte> span, out double number, out int bytesConsumed)
        {
            return Utf8Parser.TryParse(span, out number, out bytesConsumed) && span.Length == bytesConsumed;
        }

        protected override bool TryParse(string value, out double number)
        {
            return double.TryParse(value, out number);
        }

        protected override double GetValue(Utf8JsonReader reader)
        {
            return reader.GetDouble();
        }

    }
}