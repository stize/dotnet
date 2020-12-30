using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Stize.DotNet.Delta;

namespace Stize.DotNet.Json.Converter
{
    public class DeltaOfJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType &&
                   typeToConvert.GetGenericTypeDefinition() == typeof(Delta<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var elementType = typeToConvert.GetGenericArguments()[0];
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(DeltaConverter<>)
                    .MakeGenericType(new Type[] { elementType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null);

            return converter;
        }
    }

    public class DeltaConverter<T> : JsonConverter<Delta<T>>
        where T : class
    {
        public override Delta<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject || !reader.Read())
            {
                throw new JsonException();
            }

            var targetType = typeof(T);
            var deltaType = typeof(Delta<T>);
            var delta = (Delta<T>)Activator.CreateInstance(deltaType);
            var propertyNames = delta.GetUnchangedPropertyNames()
                                     .ToDictionary(x => options.PropertyNamingPolicy?.ConvertName(x) ?? x, x => x);

            while (reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName= reader.GetString();
                    var effectivePropertyName = propertyNames[propertyName];
                    if (delta.TryGetPropertyType(effectivePropertyName, out var propertyType))
                    {
                        var propertyValue =JsonSerializer.Deserialize(ref reader, propertyType, options);
                        delta.TrySetPropertyValue(effectivePropertyName, propertyValue);
                    }
                }
               
                if (!reader.Read())
                {
                    throw new JsonException();
                }
            }


            return delta;
        }

        public override void Write(Utf8JsonWriter writer, Delta<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            var changedPropertyNames = value.GetChangedPropertyNames();
            foreach (var propertyName in changedPropertyNames)
            {
                if (value.TryGetPropertyValue(propertyName, out var propertyValue ) && 
                    value.TryGetPropertyType(propertyName, out var propertyType ))
                {
                    var effectivePropertyName = options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;
                    writer.WritePropertyName(effectivePropertyName);
                    JsonSerializer.Serialize(writer, propertyValue, propertyType, options);
                }
            }
            writer.WriteEndObject();
        }

    }
}