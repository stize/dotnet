using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Stize.DotNet.Delta
{
    /// <summary>
    /// A class the tracks changes (i.e. the Delta) for an object.
    /// </summary>
    public abstract class Delta : DynamicObject, IDelta
    {
        /// <summary>
        /// Clears the Delta and resets the underlying object.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Attempts to set the Property called <paramref name="name"/> to the <paramref name="value"/> specified.
        /// <remarks>
        /// Only properties that exist on object can be set.
        /// If there is a type mismatch the request will fail.
        /// </remarks>
        /// </summary>
        /// <param name="name">The name of the Property</param>
        /// <param name="value">The new value of the Property</param>
        /// <returns>True if successful</returns>
        public abstract bool TrySetPropertyValue(string name, object value);

        /// <summary>
        /// Attempts to get the value of the Property called <paramref name="name"/> from the underlying object.
        /// <remarks>
        /// Only properties that exist on object can be retrieved.
        /// Both modified and unmodified properties can be retrieved.
        /// </remarks>
        /// </summary>
        /// <param name="name">The name of the Property</param>
        /// <param name="value">The value of the Property</param>
        /// <returns>True if the Property was found</returns>
        public abstract bool TryGetPropertyValue(string name, out object value);

        /// <summary>
        /// Attempts to get the <see cref="Type"/> of the Property called <paramref name="name"/> from the underlying object.
        /// <remarks>
        /// Only properties that exist on object can be retrieved.
        /// Both modified and unmodified properties can be retrieved.
        /// </remarks>
        /// </summary>
        /// <param name="name">The name of the Property</param>
        /// <param name="type">The type of the Property</param>
        /// <returns>Returns <c>true</c> if the Property was found and <c>false</c> if not.</returns>
        public abstract bool TryGetPropertyType(string name, out Type type);

        /// <summary>
        /// Overrides the DynamicObject TrySetMember method, so that only the properties
        /// of object can be set.
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            return this.TrySetPropertyValue(binder.Name, value);
        }

        /// <summary>
        /// Overrides the DynamicObject TryGetMember method, so that only the properties
        /// of object can be got.
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            return this.TryGetPropertyValue(binder.Name, out result);
        }

        /// <summary>
        /// Returns the Properties that have been modified through this Delta as an
        /// enumeration of Property Names
        /// </summary>
        public abstract IEnumerable<string> GetChangedPropertyNames();

        /// <summary>
        /// Returns the Properties that have not been modified through this Delta as an
        /// enumeration of Property Names
        /// </summary>
        public abstract IEnumerable<string> GetUnchangedPropertyNames();

        public static bool IsDelta(Type elementType)
        {
            return typeof(IDelta).IsAssignableFrom(elementType);
        }

        /// <summary>
        /// Helper method to check whether the given type is Delta generic type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if it is a DeltaList type; false otherwise.</returns>
        public static bool IsDeltaCollection(Type type)
        {
            return type != null && type.IsCollection(out var elementType) && IsDelta(elementType);
        }
    }
}