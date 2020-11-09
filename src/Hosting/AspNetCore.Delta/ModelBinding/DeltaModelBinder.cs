using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Delta;

namespace Stize.Hosting.AspNetCore.Delta.ModelBinding
{
    public class DeltaModelBinder : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (typeof(IDelta).IsAssignableFrom(bindingContext.ModelType))
            {
                var delta = (IDelta)Activator.CreateInstance(bindingContext.ModelType);

                var jsonStream = bindingContext.HttpContext.Request.BodyReader;
                using (var document = JsonDocument.Parse(jsonStream.AsStream()))
                {
                    if (document != null)
                    {
                        if (document.RootElement.ValueKind == JsonValueKind.Object)
                        {
                            var properties = document.RootElement.EnumerateObject();
                            this.FillDelta(delta, properties);
                        }
                    }
                }




                if (bindingContext.ModelType.GetGenericTypeDefinition() == typeof(Delta<>))
                {
                    var validator = bindingContext.HttpContext.RequestServices?.GetService<IObjectModelValidator>();
                    if (validator != null)
                    {
                        var deltaOfT = (dynamic)delta;
                        var instanceOfT = deltaOfT.GetInstance();
                        deltaOfT.Patch(instanceOfT);
                        validator.Validate(bindingContext.ActionContext, null, string.Empty, instanceOfT);
                        foreach (var key in bindingContext.ActionContext.ModelState.Keys)
                        {
                            if (!delta.GetChangedPropertyNames().Contains(key))
                            {
                                bindingContext.ActionContext.ModelState.Remove(key);
                            }
                        }


                    }
                }

                bindingContext.Model = delta;
                bindingContext.Result = ModelBindingResult.Success(delta);
            }


            return Task.CompletedTask;
        }

        protected virtual void FillDelta(IDelta model, JsonElement.ObjectEnumerator properties)
        {

            foreach (var property in properties)
            {
                var propertyName = property.Name.FirstCharacterToUpper();
                if (model.TryGetPropertyType(propertyName, out var propertyType))
                {
                    if (this.TryGetTargetPropertyValue(propertyType, property, out var value))
                    {
                        model.TrySetPropertyValue(propertyName, value);
                    }
                }
            }


        }

        private bool TryGetTargetPropertyValue(Type propertyType, JsonProperty property, out object value)
        {
            switch (property.Value.ValueKind)
            {
                case JsonValueKind.Object:
                    var delta = this.GetValueFromJObject(property.Value, propertyType);
                    value = delta;
                    return true;
                case JsonValueKind.Array:
                    if (propertyType.IsCollection(out var elementType))
                    {
                        if (this.GetValueFromJArray(property.Value, propertyType, elementType, out var collection))
                        {
                            value = collection;
                            return true;
                        }
                    }
                    break;
                case JsonValueKind.Number:
                case JsonValueKind.String:
                case JsonValueKind.Null:
                    value = this.GetValueFromJValue(property.Value, propertyType);
                    return true;
                case JsonValueKind.Undefined:
                case JsonValueKind.False:
                case JsonValueKind.True:
                default:
                    break;
            }

            value = null;
            return false;
        }

        private object GetValueFromJValue(JsonElement jvalue, Type propertyType)
        {
            if (jvalue.ValueKind == JsonValueKind.Number)
            {
                //TODO: could be optimized if we use the type of the property
                //for now reproduce newtonsoft that reads all numbers as long
                return jvalue.GetInt64();
            }
            return jvalue.GetString();
        }

        private IDelta GetValueFromJObject(JsonElement elementObject, Type targetObjectType)
        {
            var deltaType = typeof(Delta<>).MakeGenericType(targetObjectType);
            var delta = Activator.CreateInstance(deltaType) as IDelta;
            this.FillDelta(delta, elementObject.EnumerateObject());
            return delta;
        }

        private bool GetValueFromJArray(JsonElement jarray, Type collectionType, Type collectionElementType, out IEnumerable collection)
        {
            if (CollectionDeserialization.TryCreateInstance(collectionType, collectionElementType, out collection))
            {
                foreach (var element in jarray.EnumerateArray())
                {
                    if (element.ValueKind == JsonValueKind.Number || element.ValueKind == JsonValueKind.String)
                    {
                        var item = this.GetValueFromJValue(element, collectionElementType);
                        var value = PrimitiveConverter.ConvertPrimitiveValue(item, collectionElementType);
                        new[] { value }.AddToCollection(collection, collectionElementType);
                    }
                    else if (element.ValueKind == JsonValueKind.Object)
                    {
                        if (!DotNet.Delta.Delta.IsDeltaCollection(collection.GetType()))
                        {
                            collection = new List<IDelta>(collection.OfType<IDelta>());
                        }
                        var obj = this.GetValueFromJObject(element, collectionElementType);
                        new[] { obj }.AddToCollection(collection, collectionElementType);
                    }
                    else if (element.ValueKind == JsonValueKind.Array)
                    {
                        if (collectionElementType.IsCollection(out var elementType))
                        {
                            if (this.GetValueFromJArray(element, collectionElementType, elementType, out var childCollection))
                            {
                                new[] { childCollection }.AddToCollection(collection, collectionElementType);
                            }

                        }
                    }
                }

                if (collectionType.IsArray)
                {
                    collection = CollectionDeserialization.ToArray(collection, collectionElementType);
                }

                return true;
            }

            collection = null;
            return false;
        }


    }
}