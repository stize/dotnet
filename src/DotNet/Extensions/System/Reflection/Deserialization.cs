using System.Collections;
using System.Dynamic;
using System.Runtime.Serialization;
using Stize.DotNet.Extensions.Common;

namespace System.Reflection
{
    internal class Deserialization
    {
        internal static void SetCollectionProperty(object resource, PropertyInfo property, object value, bool clearCollection)
        {
            if (value != null)
            {
                var collection = value as IEnumerable;
                var resourceType = resource.GetType();
                var propertyName = property.Name;
                var propertyType = property.PropertyType;

                if (!propertyType.IsCollection(out var elementType))
                {
                    var message = Error.Format(SRResources.PropertyIsNotCollection, propertyType.FullName, propertyName,
                        resourceType.FullName);
                    throw new SerializationException(message);
                }

                if (CanSetProperty(resource, propertyName) &&
                    CollectionDeserialization.TryCreateInstance(propertyType, elementType, out var newCollection)
                )
                {
                    // settable collections
                    collection.AddToCollection(newCollection, elementType);
                    if (propertyType.IsArray)
                    {
                        newCollection = CollectionDeserialization.ToArray(newCollection, elementType);
                    }

                    SetProperty(resource, propertyName, newCollection);
                }
                else
                {
                    // get-only collections.
                    newCollection = GetProperty(resource, propertyName) as IEnumerable;
                    if (newCollection == null)
                    {
                        var message = Error.Format(SRResources.CannotAddToNullCollection, propertyName, resourceType.FullName);
                        throw new SerializationException(message);
                    }

                    if (clearCollection)
                    {
                        newCollection.Clear();
                    }

                    collection.AddToCollection(newCollection, elementType);
                }
            }
        }

        private static object GetProperty(object resource, string propertyName)
        {
            if (resource is DynamicObject dynamic)
            {
                return ((dynamic)dynamic)[propertyName];
            }

            var property = resource.GetType().GetProperty(propertyName);
            return property?.GetValue(resource, null);
        }
        
        private static bool CanSetProperty(object resource, string propertyName)
        {
            if (resource is DynamicObject)
            {
                return true;
            }

            var property = resource.GetType().GetProperty(propertyName);
            return property != null && property.GetSetMethod() != null;
        }

        internal static void SetProperty(object resource, string propertyName, object value)
        {
            if (resource is DynamicObject dynamic)
            {
                ((dynamic)dynamic)[propertyName] = value;
            }
            else
            {
                resource.GetType().GetProperty(propertyName)?.SetValue(resource, value, null);
            }
        }
    }
}