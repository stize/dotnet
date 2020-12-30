using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Reflection
{
    public static class PropertyCache<T> where T : class
    {
        private static readonly Lazy<Dictionary<string, PropertyAccessor<T>>> Lazy = new Lazy<Dictionary<string, PropertyAccessor<T>>>(GetDictionary);

        private static Dictionary<string, PropertyAccessor<T>> GetDictionary()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => (p.GetSetMethod() != null || p.PropertyType.IsCollection()) &&
                            p.GetGetMethod() != null)
                .Select<PropertyInfo, PropertyAccessor<T>>(p => new FastPropertyAccessor<T>(p))
                .ToDictionary(p => p.Property.Name);
        }

        public static IReadOnlyDictionary<string, PropertyAccessor<T>> Get()
        {
            var current = Lazy.Value;
            return new ReadOnlyDictionary<string, PropertyAccessor<T>>(current);
        }
    }
}
