using Stize.DotNet.Extensions.Common;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;

namespace System.Reflection
{
    public static class CollectionDeserialization
    {
        private static readonly Type[] EmptyTypeArray = new Type[0];
        private static readonly object[] EmptyObjectArray = new object[0];
        private static readonly MethodInfo ToArrayMethodInfo = typeof(Enumerable).GetMethod("ToArray");

        public static bool TryCreateInstance(Type collectionType, Type elementType, out IEnumerable instance)
        {
            if (collectionType.IsGenericType())
            {
                var genericDefinition = collectionType.GetGenericTypeDefinition();
                if (genericDefinition == typeof(IEnumerable<>) ||
                    genericDefinition == typeof(ICollection<>) ||
                    genericDefinition == typeof(IList<>))
                {
                    instance = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)) as IEnumerable;
                    return true;
                }
            }

            if (collectionType.IsArray)
            {
                // We don't know the size of the collection in advance. So, create a list and later call ToArray. 
                instance = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)) as IEnumerable;
                return true;
            }

            if (collectionType.GetConstructor(Type.EmptyTypes) != null && !collectionType.IsAbstract)
            {
                instance = Activator.CreateInstance(collectionType) as IEnumerable;
                return true;
            }

            instance = null;
            return false;
        }

        public static void AddToCollection(this IEnumerable items, IEnumerable collection, Type elementType)
        {
            MethodInfo addMethod = null;
            var list = collection as IList;

            if (list == null)
            {
                addMethod = collection.GetType().GetMethod("Add", new[] { elementType });
                if (addMethod == null)
                {
                    var message = Error.Format(SRResources.CollectionShouldHaveAddMethod, collection.GetType().FullName);
                    throw new SerializationException(message);
                }
            }
            else if (list.GetType().IsArray)
            {
                var message = Error.Format(SRResources.GetOnlyCollectionCannotBeArray, collection.GetType().FullName);
                throw new SerializationException(message);
            }

            items.AddToCollectionCore(collection, elementType, list, addMethod);
        }

        private static void AddToCollectionCore(this IEnumerable items, IEnumerable collection, Type elementType, IList list, MethodInfo addMethod)
        {
            foreach (var item in items)
            {
                var element = item;

                if (elementType.IsPrimitive && element != null)
                {
                    element = PrimitiveConverter.ConvertPrimitiveValue(element, elementType);
                }

                if (list != null)
                {
                    list.Add(element);
                }
                else
                {
                    addMethod.Invoke(collection, new[] { element });
                }
            }
        }

        public static void Clear(this IEnumerable collection)
        {
            Contract.Assert(collection != null);

            var clearMethod = collection.GetType().GetMethod("Clear", EmptyTypeArray);
            if (clearMethod == null)
            {
                var message = Error.Format(SRResources.CollectionShouldHaveClearMethod, collection.GetType().FullName);
                throw new SerializationException(message);
            }

            clearMethod.Invoke(collection, EmptyObjectArray);
        }

        public static IEnumerable ToArray(IEnumerable value, Type elementType)
        {
            return ToArrayMethodInfo.MakeGenericMethod(elementType).Invoke(null, new object[] { value }) as IEnumerable;
        }
    }
}