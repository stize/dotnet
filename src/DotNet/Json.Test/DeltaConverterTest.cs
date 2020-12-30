using System.Collections.Generic;
using System.Text.Json;
using Stize.DotNet.Delta;
using Stize.DotNet.Json.Converter;
using Xunit;

namespace Stize.DotNet.Json.Test
{
    public class DeltaConverterTest
    {
        private readonly Delta<TargetType> delta;
        private readonly string deltaJson;
        private readonly JsonSerializerOptions options;

        private class TargetType
        {
            public string StringProperty { get; set; }
            public int IntProperty { get; set; }
            public double DoubleProperty { get; set; }
            public IEnumerable<int> EnumerableIntProperty { get; set; }
            public TargetType ComplexObjectProperty { get; set; }

        }

        public DeltaConverterTest()
        {
            this.options = new JsonSerializerOptions()
            {
                Converters = { new DeltaOfJsonConverterFactory() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            this.delta = new Delta<TargetType>();
            delta.TrySetPropertyValue(nameof(TargetType.StringProperty), "This is a string value");
            delta.TrySetPropertyValue(nameof(TargetType.IntProperty), 1);
            delta.TrySetPropertyValue(nameof(TargetType.DoubleProperty), 1.1);
            delta.TrySetPropertyValue(nameof(TargetType.EnumerableIntProperty), new[] { 1, 2, 3 });
            delta.TrySetPropertyValue(nameof(TargetType.ComplexObjectProperty), new TargetType
            {
                StringProperty = "String property from a child object"
            });
            this.deltaJson = "{\"stringProperty\":\"This is a string value\",\"intProperty\":1,\"doubleProperty\":1.1,\"enumerableIntProperty\":[1,2,3],\"complexObjectProperty\":{\"stringProperty\":\"String property from a child object\",\"intProperty\":0,\"doubleProperty\":0,\"enumerableIntProperty\":null,\"complexObjectProperty\":null}}";
        }

        [Fact]
        public void MustReadTest()
        {
            var json = JsonSerializer.Serialize(delta, typeof(Delta<TargetType>), this.options);
            Assert.Equal(this.deltaJson, json);
        }

        [Fact]
        public void MustWriteTest()
        {
            var deltaParsed = JsonSerializer.Deserialize(this.deltaJson, typeof(Delta<TargetType>), this.options) as Delta<TargetType>;
            Assert.NotNull(deltaParsed);

            Assert.True(this.delta.TryGetPropertyValue(nameof(TargetType.StringProperty), out var stringValue));
            Assert.True(deltaParsed.TryGetPropertyValue(nameof(TargetType.StringProperty), out var stringValueParsed));
            Assert.Equal(stringValue, stringValueParsed);
        }
    }
}
