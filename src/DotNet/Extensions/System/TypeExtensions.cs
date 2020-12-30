using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    /// <summary>
    /// Extension of System.Type
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Set of numeric types
        /// </summary>
        private static readonly ISet<Type> NumericTypes = new HashSet<Type>(new[]
        {
            typeof(sbyte), typeof(byte),
            typeof(decimal), typeof(double), typeof(float),
            typeof(short), typeof(int), typeof(long),
            typeof(ushort), typeof(uint), typeof(ulong)
        });

        /// <summary>
        /// Determines whether an instance of a specified type can be assigned to a variable of the current type.
        /// </summary>
        /// <param name="type">Original type</param>
        /// <param name="target">Type to compare with <paramref name="type"/></param>
        /// <returns>True if is assignable, false if not</returns>
        public static bool IsAssignableFrom(this Type type, Type target)
        {
            return type.GetTypeInfo().IsAssignableFrom(target.GetTypeInfo());
        }

        /// <summary>
        /// Determine if a type is an enumeration.
        /// </summary>
        /// <param name="clrType">The type to test.</param>
        /// <returns>True if the type is an enumeration; false otherwise.</returns>
        public static bool IsEnum(this Type clrType)
        {
            var underlyingTypeOrSelf = GetUnderlyingTypeOrSelf(clrType);
            return underlyingTypeOrSelf.IsEnum;
        }

        public static Type GetUnderlyingTypeOrSelf(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// Determine if a type is a generic type.
        /// </summary>
        /// <param name="clrType">The type to test.</param>
        /// <returns>True if the type is a generic type; false otherwise.</returns>
        public static bool IsGenericType(this Type clrType)
        {
            return clrType.IsGenericType;
        }

        /// <summary>
        /// Gets a collection of the methods defined by <paramref name="type"/>
        /// </summary>
        /// <param name="type">type to get the methods</param>
        /// <returns>An array of MethodInfo objects representing all the global methods defined by <paramref name="type"/></returns>
        public static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            return type.GetTypeInfo().DeclaredMethods;
        }

        /// <summary>
        /// Gets the properties of <paramref name="type"/>
        /// </summary>
        /// <param name="type">type to get the properties</param>
        /// <returns>An array of PropertyInfo objects representing all public properties of <paramref name="type"/> or 
        /// An empty array of type PropertyInfo, <paramref name="type"/> does not have public properties.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var current = typeInfo.DeclaredProperties;
            if (typeInfo.BaseType != null)
            {
                var parent = typeInfo.BaseType.GetProperties();
                current = current.Union(parent).Distinct();
            }

            return current;
        }

        /// <summary>
        /// Returns an object that represents the specified public field declared by <paramref name="type"/> 
        /// </summary>
        /// <param name="type">Type to get the field</param>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>An object that represents the specified field, if found; otherwise, null</returns>
        public static FieldInfo GetField(this Type type, string fieldName)
        {
            return type.GetTypeInfo().GetDeclaredField(fieldName);
        }

        /// <summary>
        /// Gets a collection of the types defined in <paramref name="assembly"/> 
        /// </summary>
        /// <param name="assembly">Assembly to get the types</param>
        /// <returns>A collection of the types defined in <paramref name="assembly"/></returns>
        public static IEnumerable<Type> GetTypes(this Assembly assembly)
        {
            return assembly.DefinedTypes.Select(x => x.DeclaringType);
        }

        /// <summary>
        /// Find all derived types from assembly. If assembly is not given, given type assembly is used.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] GetDerivedTypes(this Type type)
        {
            return type.GetDerivedTypes(false);
        }

        /// <summary>
        /// Find all derived types from assembly. If assembly is not given, given type assembly is used.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeItself"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetDerivedTypes(this Type type, bool includeItself, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = type
                    .GetTypeInfo()
                    .Assembly;

            return assembly.GetTypes().Where(t => (includeItself || t != type) && type.IsAssignableFrom(t)).ToArray();
        }

        public static Type GetEffectiveType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }


        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static bool IsNumericType(this Type type)
        {
            if (type == null) return false;

            var effectiveType = type.GetEffectiveType();

            if (effectiveType.IsEnum()) return false;

            return NumericTypes.Contains(effectiveType);
        }

        public static bool IsEnumerable(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType != typeof(string) && propertyInfo.GetMethod.ReturnType
                .GetInterfaces()
                .Any(i => i == typeof(IEnumerable));
        }


        /// <summary>
        /// Determine if a type is a collection.
        /// </summary>
        /// <param name="clrType">The type to test.</param>
        /// <returns>True if the type is an enumeration; false otherwise.</returns>
        public static bool IsCollection(this Type clrType)
        {
            return IsCollection(clrType, out _);
        }

        /// <summary>
        /// Determine if a type is a collection.
        /// </summary>
        /// <param name="clrType">The type to test.</param>
        /// <param name="elementType">out: the element type of the collection.</param>
        /// <returns>True if the type is an enumeration; false otherwise.</returns>
        public static bool IsCollection(this Type clrType, out Type elementType)
        {
            if (clrType == null)
            {
                throw new ArgumentNullException(nameof(clrType));
            }

            elementType = clrType;

            // see if this type should be ignored.
            if (clrType == typeof(string))
            {
                return false;
            }

            Type collectionInterface
                = clrType.GetInterfaces()
                    .Union(new[] { clrType })
                    .FirstOrDefault(
                        t => IsGenericType(t)
                             && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (collectionInterface != null)
            {
                elementType = collectionInterface.GetGenericArguments().Single();
                return true;
            }

            return false;
        }


    }
}